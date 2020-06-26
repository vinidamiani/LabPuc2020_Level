using UnityEngine;

// Player spell attack scriptable object
[CreateAssetMenu (fileName = "New Spell Attack", menuName = "Player/Spell Attack")]
public class SOPlayerSpellAttack : ScriptableObject {
    // Attack properties
    [Header ("Spell Setup")]
    public PlayerSpellProjectile projectilePrefab;

    [Header ("Attack")]
    [Range (0.1f, 5.0f)]
    public float attackCoolDown = 1.0f;             // How much time the AI has to wait after the attack before attacking again

    [Range (0, 1000)]
    public int minDamage = 5;                       // The minimum amount of damage the attack can deal
    [Range (0, 1000)]
    public int maxDamage = 10;                      // The maximum amount of damage the attack can deal

    [Header ("Projectile")]
    [Range (10f, 1000f)]
    public float projectileSpeed = 200f;            // How fast the projectile moves
    [Range (1f, 20f)]
    public float projectileLifeTime = 5.0f;         // How long the projectile last for

    [Header ("Masking")]
    public string [] collisionTags;

    #region RETURN
    public int damage () {
        // Return a random damage value within the given range
        return Random.Range (this.minDamage, this.maxDamage);
    }
    #endregion
}
