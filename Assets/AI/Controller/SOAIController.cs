using UnityEngine;

// AI controller scriptable object
[CreateAssetMenu (fileName = "New Controller", menuName = "AI/Controller")]
public class SOAIController : ScriptableObject{
    // Controller properties
    [Header ("Movement")]
    [Range (0.01f, 10f)]
    public float walkSpeed = 1.5f;
    [Range (1f, 3f)]
    public float runSpeedMultiplier = 2f;

    #region RETURN
    public float move_speed (AIMovementState movementState, AICombatState combatState = AICombatState.Idle) {
        // Based on our movement and combat state, return the correct movement speed
        switch (movementState) {
            case (AIMovementState.Idle):
                return 0f;
            case AIMovementState.Walking:
                return this.walkSpeed;
            case AIMovementState.Running:
                return this.walkSpeed * this.runSpeedMultiplier;
            default:
                return this.walkSpeed;
        }
    }
    public float movement_magnitude (AIMovementState movementState) {
        // <NOTE>
        // These values are based on the velocity-transition values you have in 
        // your enemy's animator.
        // </NOTE>

        // Based on the movement state, determine what the magnitude is
        switch (movementState) {
            case (AIMovementState.Idle):
                return 0f;
            case AIMovementState.Walking:
                return 1f;
            case AIMovementState.Running:
                return 2f;
            default:
                return 0f;
        }
    }
    #endregion
}
