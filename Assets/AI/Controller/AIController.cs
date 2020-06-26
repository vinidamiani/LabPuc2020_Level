using UnityEngine;
using UnityEngine.AI;

// AI movement state enumerator
public enum AIMovementState {
    Idle, Walking, Running
}

// AI controller class
[RequireComponent (typeof (NavMeshAgent))]
public class AIController : MonoBehaviour{
    // AI components
    private AICombatHandler aiCombatHandler;
    private SOAIController soController;

    private NavMeshAgent agent;
    [System.NonSerialized]
    public Transform agentTransform;

    // State properties
    private AIMovementState movementState = AIMovementState.Idle;
    private AICombatState combatState = AICombatState.Idle;

    // Movement properties
    private float movementThreshold = 0.1f;

    // Destination properties
    private Vector3 destinationCurrent;

    // Target properties
    private Transform target;

    #region INITIALIZE
    public void initialize_components (AICombatHandler aiCombatHandler, SOAIController soController) {
        // Initialize controller components
        this.aiCombatHandler = aiCombatHandler;
        this.soController = soController;

        this.agent = this.GetComponent<NavMeshAgent> ();
        this.agentTransform = this.agent.transform;
    }
    public void initialize_properties (SOAITargetter soTargetter) {
        // Initialize controller properties
        this.agent.stoppingDistance = soTargetter.stoppingDistance;
    }
    #endregion

    #region SPAWNING
    public void set_spawn_position (Vector3 spawnPosition) {
        // Warp the agent to the spawn position
        this.agent.Warp (spawnPosition);
    }
    #endregion

    #region STATE SET
    public void states_update (AICombatState combatState) {
        // Determine if our controller is already moving
        bool isMoving = this.is_moving ();

        // Set our combat state directly
        this.combatState = combatState;
        // Based on our movement, determine what our movement state is (Run when have aquired the target)
        this.movementState = (isMoving || this.combatState == AICombatState.Charging) ? (this.combatState == AICombatState.Charging)
            ? AIMovementState.Running :
            AIMovementState.Walking :
            AIMovementState.Idle;
    }
    #endregion

    #region PATH FIND
    public void pathfind_update () {
        // Set the agent's move speed 
        this.agent.speed = this.soController.move_speed (this.movementState, this.combatState);

        // Prioritize target over destination
        if (this.aiCombatHandler.target_acquired (this.target_path_reachable ())) {
            this.destination_set (this.target.position, true);

            // If we are not moving then turn towards the target
            if (this.combatState == AICombatState.Attacking) {
                Vector3 lookPosition = new Vector3 (this.target.position.x, this.agentTransform.position.y, this.target.position.z);
                this.agentTransform.LookAt (lookPosition);
            }
        }
    }
    public float stopping_distance_get () {
        // Return the agent's stopping distance
        return this.agent.stoppingDistance;
    }
    #endregion

    #region DESTINATION
    public void destination_set (Vector3 destination, bool destinationIsTarget = false) {
        // Set the AI's destination
        this.destinationCurrent = destination;
        this.agent.SetDestination (this.destinationCurrent);
    }
    private bool destination_reached () {
        // Still processing
        if (this.agent.pathPending)
            return false;
        // Within distance
        if (this.agent.remainingDistance <= this.agent.stoppingDistance)
            return true;
        // Invalid path
        if (this.agent.pathStatus == NavMeshPathStatus.PathInvalid || this.agent.pathStatus == NavMeshPathStatus.PathPartial)
            return true;
        // Otherwise, not reached
        return false;
    }
    public Vector3 destination_get () {
        // Return the current destination
        return this.destinationCurrent;
    }
    #endregion

    #region TARGET
    public void i_target_found (Transform target) {
        // Set our current target 
        this.target = target;
    }
    public void i_target_lost (Vector3 targetPrevPosition) {
        // Reset our current target
        this.target = null;

        // Set our destination to the target's last known position
        this.destination_set (targetPrevPosition);
    }

    private bool target_path_reachable () {
        return this.agent.pathStatus == NavMeshPathStatus.PathInvalid || this.agent.pathStatus == NavMeshPathStatus.PathPartial;
    }
    #endregion

    #region MOVEMENT STATES
    private bool is_moving () {
        // Check if we are moving along any axis
        bool movingX = Mathf.Abs (this.agent.velocity.x) >= this.movementThreshold;
        bool movingZ = Mathf.Abs (this.agent.velocity.z) >= this.movementThreshold;

        // Return if we are moving in any direction
        return movingX || movingZ;
    }
    public float movement_magnitude () {
        // Based on our character's movement state, determine what the magnitude is
        // Idle movement
        if (this.movementState == AIMovementState.Idle)
            return 0f;
        // Walking movement
        else if (this.movementState == AIMovementState.Walking)
            return 0.5f;
        // Running movement
        else
            return 1f;
    }
    public Vector3 movement_direction () {
        // Return the agent's movement direction
        return this.agent.velocity.normalized;
    }
    #endregion
}
