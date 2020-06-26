using System;
using UnityEngine;

[RequireComponent (typeof (PlayerCombatHandler))]
public class Player : MonoBehaviour, IPlayerCombatEvent, IHealthEvent{
    // Player components
    [Header ("Player Setup")]
    public Transform playerTransform;
    public SOHealth soHealth;

    [NonSerialized]
    public PlayerAnimator animator;
    [NonSerialized]
    public PlayerCombatHandler combatHandler;
    [NonSerialized]
    public Health health;

    #region INITIALIZE
    private void Awake () {
        // Initialize player components
        this.animator = this.GetComponentInChildren<PlayerAnimator> ();
        this.combatHandler = this.GetComponent<PlayerCombatHandler> ();
        this.health = this.GetComponentInChildren<Health> ();

        this.animator.initialize_components (this);
        this.combatHandler.initialize_components (this);
        this.health.initialize_components (this.soHealth, this);
    }
    private void Start () {
        // Initialize player properties
        this.health.initialize_properties ();
    } 
    #endregion

    #region UPDATE
    private void LateUpdate () {
        // Only update if we are still alive
        if (this.health.is_alive ()) {
            // Update the combat handler
            this.combatHandler.combat_update_late ();
        }
    }
    #endregion

    #region HEALTH EVENT INTERFACE
    public void i_take_damage (int damage, Vector3 pointOfAttack) {
        // Animate the player to get hit
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
        // Animate the player to die
        this.animator.animate_die_trigger ();
    }
    #endregion

    #region COMBAT INTERFACE
    public void i_attack (SOPlayerMeleeAttack soAttack) {
        // Play attack animation through here...
    }
    #endregion
}
