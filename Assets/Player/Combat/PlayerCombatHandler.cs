using UnityEngine;

// Player combat handler interface
public interface IPlayerCombatEvent {
    // Combat events 
    void i_attack (SOPlayerMeleeAttack soAttack);
}

// Player combat handler class
public class PlayerCombatHandler : MonoBehaviour{
    // Combat components
    [Header ("Combat Setup")]
    public Transform weaponHolder;
    public Transform attackPoint;

    private PhisicalWeapon weapon;
    private PlayerSpellAttack spellAttack;
    private PlayerMeleeAttack meleeAttack;

    // Combat input
    [Header ("Combat Input")]
    public string inputDropWeapon = "q";
    public string inputSpellAttack = "Fire1";

    #region INITIALIZE
    public void initialize_components (IPlayerCombatEvent combatEvent) {
        // Initialize player components
        this.spellAttack = this.GetComponentInChildren<PlayerSpellAttack> ();
        this.meleeAttack = this.GetComponentInChildren<PlayerMeleeAttack> ();

        this.spellAttack.initialize_components (this.attackPoint);
        this.meleeAttack.initialize_components (combatEvent, this.attackPoint);
    }
    #endregion

    #region COMBAT
    public void combat_update_late () {
        // Check if we have a weapon equipped
        this.weapon = this.weaponHolder.GetComponentInChildren<PhisicalWeapon> ();

        // Allow spell attack if we don't have a sword out and there is input
        if (this.weapon == null && this.combat_input () && this.spellAttack.attack_ready ()) {
            this.spellAttack.attack ();
        }
        // Allow melee attack if we do have our sword out
        else if (this.weapon != null && this.combat_input () && this.meleeAttack.attack_ready ()) {
            this.meleeAttack.attack ();
        }
        // Otherwise, see if we want to drop the weapon
        else if (this.weapon != null && this.drop_weapon_input ()) {
            // Drop weapon here...
        }
    }

    private bool drop_weapon_input () {
        // You can change this input mapping to how you see fit
        return Input.GetKeyDown (this.inputDropWeapon);
    }
    private bool combat_input () {
        // You can change this input mapping to how you see fit
        return Input.GetButton (this.inputSpellAttack);
    }
    #endregion

    #region ATTACK
    public void attack_anim_event () {
        // This is when we actually deal damage to our target
        this.meleeAttack.attack_anim_event ();
    }
    #endregion
}
