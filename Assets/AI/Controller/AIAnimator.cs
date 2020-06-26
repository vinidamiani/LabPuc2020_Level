using UnityEngine;

[RequireComponent (typeof (Animator))]
public class AIAnimator : MonoBehaviour{
    // AI components
    private AI ai;

    // Animator components
    private Animator animator;

    // Controller tag properties
    private readonly string animMoveMagnitude = "Velocity";
    private readonly string animAttackIndex = "Attack Index";
    private readonly string animAttack = "Attack";
    private readonly string animDie = "Die";

    // Animator smoothing properties
    private readonly float deltaTimeMultiplier = 10f;

    #region INITIALIZE
    public void initialize_components (AI ai) {
        // Initialize AI components
        this.ai = ai;

        // Initialize animator components
        this.animator = this.GetComponent<Animator> ();
    }
    #endregion

    #region CONTROLLER
    public void animate_movement_update_late (Vector3 movementDirection, float movementMagnitude) {
        // Update animator 
        this.animator.SetFloat (this.animMoveMagnitude, movementMagnitude, 1f, Time.smoothDeltaTime * this.deltaTimeMultiplier);
    }
    #endregion

    #region COMBAT
    public void animate_attack_trigger (int attackIndex = 0) {
        // Update animator
        this.animator.SetInteger (this.animAttackIndex, attackIndex);
        this.animator.SetTrigger (this.animAttack);
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
        this.ai.aiCombatHandler.attack_anim_event ();
    }
    #endregion
}
