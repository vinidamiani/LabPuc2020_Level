using UnityEngine;

// AI combat state enumerator
public enum AICombatState {
    Idle, Charging, Attacking
}

// AI combat handler interface
public interface IAICombatEvent {
    // Combat events 
    void i_attack (SOAIAttack soAttack, int attackIndex);

    void i_target_found (Transform target);
    void i_target_lost (Vector3 targetPrevPosition);
}

// AI combat handler class
public class AICombatHandler : MonoBehaviour{
    // AI components
    private Transform agentTransform;

    // Combat components
    private SOAITargetter soTargetter;
    public IAICombatEvent iAICombatEvent;

    private AIAttack [] attacks;
    private AIAttack attackCurrent;

    private Transform attackPoint;

    // State properties
    private AICombatState combatState = AICombatState.Idle;

    // Target properties
    private bool targetSet = false;
    private Transform targetCurrent;

    // Debugging properties
    [Header ("Debug AI")]
    public bool debugEnabled = false;

    #region INITIALIZE
    public void initialize_components (Transform agentTransform, SOAITargetter soTargetter, IAICombatEvent iAICombatEvent) {
        // Initialize AI components
        this.agentTransform = agentTransform;

        // Initialize combat components
        this.soTargetter = soTargetter;
        this.iAICombatEvent = iAICombatEvent;
        this.attacks = this.GetComponentsInChildren<AIAttack> ();
        this.attackPoint = this.transform.Find ("Attack Point");

        foreach (AIAttack attack in this.attacks) {
            attack.initialize_components (this.iAICombatEvent, attackPoint);
        }
    }
    public void initialize_properties () {
        // Initialize combat properties
        for (int i = 0; i < this.attacks.Length; i ++) {
            this.attacks [i].initialize_properties (i);
        }

        // Select the first attack by default
        this.attack_select (0);
    }
    #endregion

    #region AI
    private Vector3 view_position () {
        // Return the position of the AI's 'eyes'
        return this.agentTransform.position + Vector3.up * this.soTargetter.viewOffset;
    }
    #endregion

    #region ATTACK
    public void attack_update_late () {
        // Only allow attack if we are in the correct state
        if (this.combatState != AICombatState.Attacking)
            return;

        // Otherwise, allow our AI to attack
        this.attackCurrent.attack ();
    }
    public void attack_anim_event () {
        // This is when we actually deal damage to our target
        this.attackCurrent.attack_anim_event ();
    }
    #endregion

    #region ATTACK SELECTION
    public void attack_select (int attackIndex) {
        // Make sure the index is in range
        if (attackIndex > this.attacks.Length - 1) {
            Debug.LogWarning ("AI Combat Handler: Cannot select attack with index of " + attackIndex);
            return;
        }

        // Unequip our current attack
        if (this.attackCurrent != null)
            this.attackCurrent.attackEquipped = false;

        // Select our new attack
        this.attackCurrent = this.attacks [attackIndex];
        this.attackCurrent.attackEquipped = true;
    }
    private void attack_select_random () {
        // Randomly select an attack to use
        this.attack_select (Random.Range (0, this.attacks.Length));
    }
    #endregion

    #region COMBAT STATE
    public AICombatState combat_state () {
        // Return our current combat state
        return this.combatState;
    }
    #endregion

    #region TARGET
    public bool target_acquired () {
        // Simply return if we have a target or not
        return this.targetCurrent != null;
    }
    public bool target_acquired (bool pathToTargetReachable = true) {
        // Get the direct distance to the target/possible target
        // POSSIBLE TARGET IS SET DIRECTLY FOR TESTING
        Transform target = (this.targetSet) ? this.targetCurrent : GameObject.FindObjectOfType<TrdWalk> ().transform;
        float distance = Vector3.Distance (agentTransform.position, target.position);

        // DEBUGGING
        if (this.debugEnabled) {
            // DEBUG OBSTRUCTION
            Debug.DrawLine (this.view_position (), target.position, Color.red);
            // DEBUG VIEW ANGLE
            this.target_view_debug ();
        }

        // We currently have a target
        if (this.targetSet) {
            // Target is unreachable
            if (pathToTargetReachable) {
                this.target_reset ();
                return false;
            }
            // Target is out of range
            else if (distance > this.soTargetter.forgetDistance && !this.soTargetter.alwaysFollowTarget) {
                this.target_reset ();
                return false;
            }
            // Target cannot be seen
            else if (!this.target_within_view (target, true) && !this.soTargetter.alwaysFollowTarget) {
                this.target_reset ();
                return false;
            }

            // Choose a random attack (TESTING PURPOSES)
            this.attack_select_random ();
            // If we are close enough to the target, then attack
            if (distance <= this.attackCurrent.soAttack.attackDistance) {
                this.combatState = AICombatState.Attacking;
            }
            // Otherwise, we are directing our attention towards the target (charging)
            else {
                this.combatState = AICombatState.Charging;
            }

            // Otherwise, target is within range
            this.target_set (target);
            return true;
        }
        // Search for target otherwise
        else {
            // Possible target is out of range
            if (distance > this.soTargetter.awarenessDistance && !this.soTargetter.alwaysFollowTarget) {
                return false;
            }
            // Target cannot be seen
            if (!this.target_within_view (target) && !this.soTargetter.alwaysFollowTarget) {
                return false;
            }

            // Otherwise, target is within view
            this.target_set (target);
            return true;
        }
    }
    private bool target_within_view (Transform target, bool onlyCheckObstruction = false) {
        // Get the position of the AI's 'eyes'
        Vector3 viewPos = this.view_position ();
        // Offset the target position
        Vector3 targetPos = Vector3.up + target.position;

        // Check if target is being obstructed
        Ray ray = new Ray (viewPos, targetPos - viewPos);
        RaycastHit hitInfo;

        if (Physics.Raycast (ray, out hitInfo, this.soTargetter.awarenessDistance,
            this.soTargetter.viewObstructionMask)) {

            // Obstruction (Cannot see target)
            if (hitInfo.collider.transform != target) {
                return false;
            }
        }

        // If we are only checking obstruction, then ignore view angle
        if (onlyCheckObstruction)
            return true;

        // Check the view angle with the target
        float angle = Vector3.Angle (target.position - this.agentTransform.position, this.agentTransform.forward);
        return angle <= this.soTargetter.viewAngle;
    }
    #endregion

    #region TARGET GET AND SET
    public Transform target_get () {
        // <NOTE>
        // Currently, this is only being used by the AI debugger
        // </NOTE>

        return this.targetCurrent;
    }
    public void target_set (Transform target) {
        // Set our current target
        this.targetSet = true;
        this.targetCurrent = target;

        // Call event
        this.iAICombatEvent.i_target_found (this.targetCurrent);
    }
    public void target_reset () {
        // Call event
        this.iAICombatEvent.i_target_lost (this.targetCurrent.position);

        // Reset our current target
        this.targetSet = false;
        this.targetCurrent = null;

        // Reset our combat state
        this.combatState = AICombatState.Idle;
    }
    #endregion

    #region DEBUGGING
    private void target_view_debug () {
        // Get the position of the AI's 'eyes'
        Vector3 viewPos = this.view_position ();

        // Debug right side of view
        Quaternion rightRot = Quaternion.AngleAxis (this.soTargetter.viewAngle, Vector3.up);
        Vector3 rightDir = rightRot * this.agentTransform.forward * this.soTargetter.awarenessDistance;
        Debug.DrawRay (viewPos, rightDir, Color.yellow);

        // Debug left side of view
        Quaternion leftRot = Quaternion.AngleAxis (-this.soTargetter.viewAngle, Vector3.up);
        Vector3 leftDir = leftRot * this.agentTransform.forward * this.soTargetter.awarenessDistance;
        Debug.DrawRay (viewPos, leftDir, Color.yellow);

        // Debug forward distance
        Debug.DrawRay (viewPos, transform.forward * this.soTargetter.awarenessDistance, Color.green);

        // Debug target acquired
        if (this.targetSet) {
            Debug.DrawLine (this.view_position (), this.targetCurrent.position, Color.red);
        }
    }
    #endregion
}
