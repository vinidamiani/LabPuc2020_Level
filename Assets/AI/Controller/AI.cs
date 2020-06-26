using UnityEngine;
using System;

[RequireComponent (typeof (AIController))]
[RequireComponent (typeof (AICombatHandler))]
[RequireComponent (typeof (Health))]
public class AI : MonoBehaviour, IHealthEvent, IAICombatEvent{
    // AI components
    [NonSerialized]
    public AIController aiController;
    [NonSerialized]
    public AIAnimator aiAnimator;
    [NonSerialized]
    public AICombatHandler aiCombatHandler;

    // Universal components
    [NonSerialized]
    public Health health;

    // AI properties
    [Header ("AI Setup")]
    public SOAIController soController;
    public SOAITargetter soTargetter;

    public SOHealth soHealth;

    #region INITIALIZE
    public void initialize_components () {
        // Get AI components
        this.aiController = this.GetComponent<AIController> ();
        this.aiAnimator = this.GetComponentInChildren<AIAnimator> ();
        this.aiCombatHandler = this.GetComponent<AICombatHandler> ();
        this.health = this.GetComponent<Health> ();

        // Initialize AI components
        this.aiController.initialize_components (this.aiCombatHandler, this.soController);
        this.aiAnimator.initialize_components (this);
        this.aiCombatHandler.initialize_components (this.aiController.agentTransform, this.soTargetter, this);
        this.health.initialize_components (this.soHealth, this);
    }
    public void initialize_properties () {
        // Initialize AI properties
        this.aiController.initialize_properties (this.soTargetter);
        this.aiCombatHandler.initialize_properties ();
        this.health.initialize_properties ();
    }
    #endregion

    #region UPDATE
    public void ai_update () {
        // Only update if we are alive
        if (this.health.is_alive ()) {
            // Update controller (movement and pathfinding)
            this.aiController.states_update (this.aiCombatHandler.combat_state ());
            this.aiController.pathfind_update ();
        }
    }
    public void ai_update_late () {
        // Only update if we are alive
        if (this.health.is_alive ()) {
            // Update combat handler
            this.aiCombatHandler.attack_update_late ();
        }

        // Update the animator
        this.aiAnimator.animate_movement_update_late (this.aiController.movement_direction (), 
            this.aiController.movement_magnitude ());
    }
    #endregion

    #region HEALTH EVENT INTERFACE
    public void i_take_damage (int damage, Vector3 pointOfAttack) {
        // Have the AI be directed to the point of attack
        this.aiController.destination_set (pointOfAttack);
    }
    public void i_health_changed () {
        // <IDEAS>
        // You can add a health (UI or Sprite) bar
        // </IDEAS>
    }
    public void i_armour_changed () {
        // <IDEAS>
        // You can add an armour (UI or Sprite) bar
        // </IDEAS>
    }
    public void i_die () {
        // Stop the AI from moving
        this.aiController.destination_set (this.aiController.transform.position);

        // Update animator
        this.aiAnimator.animate_die_trigger ();

        // Destroy AI after a few seconds
        Destroy (this.gameObject, 4f);
    }
    #endregion

    #region COMBAT EVENT INTERFACE
    public void i_attack (SOAIAttack soAttack, int attackIndex) {
        // <IDEAS>
        // Based on the attack we are using, you could have different animations pertaining to that attack
        // Such having a light, medium, or heavy attack
        // </IDEAS>

        // Animate the attack
        this.aiAnimator.animate_attack_trigger (attackIndex);
    }

    public void i_target_found (Transform target) {
        // We have found the target
        this.aiController.i_target_found (target);
    }

    public void i_target_lost (Vector3 targetPrevPosition) {
        // We have lost the target (so move to the last position that we saw them at)
        this.aiController.i_target_lost (targetPrevPosition);
    }
    #endregion
}
