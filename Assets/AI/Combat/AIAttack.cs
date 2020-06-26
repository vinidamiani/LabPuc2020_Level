using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAttack : MonoBehaviour{
    // Attack components
    [Header ("Attack Setup")]
    public SOAIAttack soAttack;

    private IAICombatEvent combatEvent;
    private Transform attackPoint;

    // Attack properties
    private int attackIndex;

    private readonly int attackRayCount = 10;       // The number of rays casted per attack (Increase for higher accuracy)
    private float attackRayAngle;                   // The angle between each ray

    // Combat-state properties
    [NonSerialized]
    public AICombatState combatState = AICombatState.Idle;

    // Ready-state properties
    [NonSerialized]
    public bool attackEquipped = true;    // If the attack is equipped (Used for having multiple attacks)
    [NonSerialized]
    public bool attackReady = true;       // If we are ready to attack again
    [NonSerialized]
    public bool rechargeReady = true;     // If we are recharging the attack (i.e. reloading)

    #region INITIALIZE
    public void initialize_components (IAICombatEvent combatEvent, Transform attackPoint) {
        // Initialize attack components
        this.combatEvent = combatEvent;
        this.attackPoint = attackPoint;
    }

    public void initialize_properties (int attackIndex) {
        // Initialize attack properties
        this.attackIndex = attackIndex;

        // Calculate the AI's attack angle
        this.attackRayAngle = this.soAttack.attackAngle / this.attackRayCount;
    }
    #endregion

    #region ATTACKING
    public void attack () {
        // Only attack if we have finished our previous attack
        if (!this.attackEquipped || !this.attackReady || !this.rechargeReady)
            return;

        // Animate (the actual attack will occur during the animation)
        this.attackReady = false;
        this.combatEvent.i_attack (this.soAttack, this.attackIndex);

        // Audio effect
        // ...
    }
    public void attack_anim_event () {
        // <NOTE>
        // This method is called from the AI's animation in order to sync the animation
        // with the damage being dealt.
        // </NOTE>

        // Apply damage to all targets in range
        this.attack_rays ();

        // Apply the attack cooldown
        this.StopAllCoroutines ();
        this.StartCoroutine ("attack_cool_down");
    }

    private IEnumerator attack_cool_down () {
        // We are starting the cooldown
        yield return new WaitForSeconds (this.soAttack.attackCoolDown);

        // We are ready to attack again
        this.attackReady = true;
        this.StopCoroutine ("attack_cool_down");
    }
    private void attack_rays () {
        // Keep track of each object we attacked that contains a health component
        List<IHealth> healthComponents = new List<IHealth> ();

        // Create a ray encircling the attack angle
        for (int i = 0; i < this.attackRayCount; i++) {
            // Get the angle calculations
            float angle = i * this.attackRayAngle - this.soAttack.attackAngle / 2;
            Quaternion rot = Quaternion.AngleAxis (angle, Vector3.up);
            Vector3 dir = rot * this.attackPoint.forward * this.soAttack.attackDistance;

            // Create a ray
            Ray ray = new Ray (this.attackPoint.position, dir);
            RaycastHit hitInfo;

            if (Physics.Raycast (ray, out hitInfo, this.soAttack.attackDistance, this.soAttack.attackMask)) {
                // Check if the hit collider has a health component
                IHealth iHealth = hitInfo.collider.GetComponent<IHealth> ();

                // The object we hit does have health 
                // Only apply damage if haven't already done so to this component
                if (iHealth != null && !healthComponents.Contains (iHealth)) {
                    // Add to our list
                    healthComponents.Add (iHealth);

                    // Have the health component (i.e. object, player) take damage
                    int damage = this.soAttack.damage ();
                    iHealth.i_take_damage (damage, this.attackPoint.position);
                }
            }

            // DEBUGGING
            // Debug.DrawLine (ray.origin, ray.GetPoint (this.soAttack.attackDistance), Color.red, 1.0f);
        }
    }
    #endregion
}
