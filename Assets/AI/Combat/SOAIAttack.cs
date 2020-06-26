using UnityEngine;

// AI attack scriptable object
[CreateAssetMenu (fileName = "New Attack", menuName = "AI/Attack")]
public class SOAIAttack : ScriptableObject{
    // Attack properties
    [Header ("Attack")]
    [Range (10f, 360f)]
    public float attackAngle = 50f;                 // How wide (as an angle) the attack can reach
    [Range (1f, 40f)]
    public float attackDistance = 5f;               // How far the attack can reach

    [Header ("Damage")]
    [Range (0, 1000)]
    public int minDamage = 5;                       // The minimum amount of damage the attack can deal
    [Range (0, 1000)]
    public int maxDamage = 10;                      // The maximum amount of damage the attack can deal

    [Header ("Attack Rate")]
    [Range (0f, 5f)]
    public float attackCoolDown = 1.5f;             // How much time the AI has to wait after the attack before attacking again

    [Header ("Masking")]
    public LayerMask attackMask;                    // The layers that the attack interacts with (i.e. walls, player)

    #region RETURN
    public int damage () {
        // Return a random damage value within the given range
        return Random.Range (this.minDamage, this.maxDamage);
    }
    #endregion
}
