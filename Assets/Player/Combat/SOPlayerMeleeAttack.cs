using UnityEngine;

// Player melee attack scriptable object
[CreateAssetMenu (fileName = "New Melee Attack", menuName = "Player/Melee Attack")]
public class SOPlayerMeleeAttack : ScriptableObject{
    // Attack properties
    [Header ("Attack")]
    [Range (0.1f, 5.0f)]
    public float attackCoolDown = 1.0f;             // How much time the player has to wait after the attack before attacking again
    [Range (10f, 360f)]
    public float attackAngle = 50f;                 // How wide (as an angle) the attack can reach
    [Range (1f, 40f)]
    public float attackDistance = 5f;               // How far the attack can reach

    [Header ("Damage")]
    [Range (0, 1000)]
    public int minDamage = 5;                       // The minimum amount of damage the attack can deal
    [Range (0, 1000)]
    public int maxDamage = 10;                      // The maximum amount of damage the attack can deal

    [Header ("Masking")]
    public LayerMask attackMask;

    #region RETURN
    public int damage () {
        // Return a random damage value within the given range
        return Random.Range (this.minDamage, this.maxDamage);
    }
    #endregion
}
