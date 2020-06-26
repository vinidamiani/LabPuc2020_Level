using UnityEngine;

// Health component interface
public interface IHealth {
    // <NOTE>
    // Objects that have health and can take damage should implement this interface
    // </NOTE>

    // Health methods
    void i_take_damage (int damage, Vector3 pointOfAttack);
}

// Health event interface
public interface IHealthEvent {
    // Health methods
    void i_take_damage (int damage, Vector3 pointOfAttack);

    void i_health_changed ();
    void i_armour_changed ();

    void i_die ();
}

// Universal health class
public class Health : MonoBehaviour, IHealth {
    // Health components
    private SOHealth soHealth;
    private IHealthEvent iHealthEvent;

    // Health properties
    private int health;
    private int armour;

    #region INITIALIZE
    public void initialize_components (SOHealth soHealth, IHealthEvent iHealthEvent) {
        // Initialize health components
        this.soHealth = soHealth;
        this.iHealthEvent = iHealthEvent;
    }
    public void initialize_properties () {
        // Initialize health properties
        this.health_reset ();
        this.armour_reset ();
    }
    #endregion

    #region GENERAL
    public void i_take_damage (int damage, Vector3 pointOfAttack) {
        // Only take damage if we are still alive
        if (!this.is_alive ())
            return;

        // Call event
        this.iHealthEvent.i_take_damage (damage, pointOfAttack);

        // See how much damage will apply to the armour
        int damageToArmour = (damage > this.armour) ? this.armour : damage;

        // If we still have armour, reduce that first
        if (damageToArmour > 0) {
            this.armour_take_damage (damageToArmour);
        }
        // If there is still damage to be dealt, or we ran out of armour, apply to health directly
        if (this.armour == 0 && damage - damageToArmour > 0) {
            this.health_take_damage (damage - damageToArmour);
        }
    }
    public bool is_alive () {
        // We are alive if our health is above 0
        return this.health > 0;
    }
    #endregion

    #region HEALTH
    private void health_reset () {
        // Reset the AI's health
        this.health = this.soHealth.healthMax;

        // Call event
        if (this.iHealthEvent != null)
            this.iHealthEvent.i_health_changed ();
    }
    private void health_take_damage (int amount) {
        // Take damage
        this.health -= Mathf.CeilToInt (amount);
        this.health = Mathf.Clamp (this.health, 0, this.soHealth.healthMax);

        // Call event
        if (this.iHealthEvent != null)
            this.iHealthEvent.i_health_changed ();

        // If our AI has died
        if (!this.is_alive ()) {
            // Call event
            if (this.iHealthEvent != null)
                this.iHealthEvent.i_die ();
        }
    }

    public int health_current () {
        // Return our health amount
        return this.health;
    }
    #endregion

    #region ARMOUR
    private void armour_reset () {
        // Reset the AI's armour
        this.armour = this.soHealth.armourMax;

        // Call event
        if (this.iHealthEvent != null)
            this.iHealthEvent.i_armour_changed ();
    }
    private void armour_take_damage (int amount) {
        // Take damage
        this.armour -= Mathf.CeilToInt (amount);
        this.armour = Mathf.Clamp (this.armour, 0, this.soHealth.armourMax);

        // Call event
        if (this.iHealthEvent != null)
            this.iHealthEvent.i_armour_changed ();
    }

    public int armour_current () {
        // Return our armour amount
        return this.armour;
    }
    #endregion
}
