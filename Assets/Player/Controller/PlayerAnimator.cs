using UnityEngine;

public class PlayerAnimator : MonoBehaviour{
    // Player components
    private Player player;

    // Animator components
    private Animator animator;

    // Controller tag properties
    private readonly string animAttackIndex = "Attack Index";
    private readonly string animAttack = "Attack";
    private readonly string animDie = "Die";

    #region INITIALIZE
    public void initialize_components (Player player) {
        // Initialize player components
        this.player = player;

        // Initialize animator components
        this.animator = this.GetComponent<Animator> ();
    }
    #endregion

    #region COMBAT
    public void animate_attack_trigger (int attackIndex = 0) {
        // Update animator
        // ...
    }
    public void animate_die_trigger () {
        // Update animator
        this.animator.SetTrigger (this.animDie);
    }
    #endregion

    #region COMBAT ANIMATION EVENT
    public void animate_event_attack () {
        // <NOTE>
        // This method is called directly from the attack animation clip
        // </NOTE>

        // Send event to AI combat handler
        this.player.combatHandler.attack_anim_event ();
    }
    #endregion
}
