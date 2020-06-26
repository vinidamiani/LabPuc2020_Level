using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Player spell attack class
public class PlayerSpellAttack : MonoBehaviour{
    // Spell components
    [Header ("Attack Setup")]
    public SOPlayerSpellAttack soSpellAttack;
    public Transform spellHolder;

    private Transform attackPoint;

    // Spell pool components
    private List<PlayerSpellProjectile> projectilesInPool = new List<PlayerSpellProjectile> ();
    private int initialProjectileCount = 20;                    // The number of projectiles initially spawned in the pool

    // Ready-state properties
    private bool attackReady = true;       // If we are ready to attack again

    #region INITIALIZE
    public void initialize_components (Transform attackPoint) {
        // Initialize attack components
        this.attackPoint = attackPoint;
        this.pool_create ();
    }
    #endregion

    #region SPELL POOL
    private void pool_create () {
        // Create all initially-spawned projectiles
        for (int i = 0; i < this.initialProjectileCount; i++) {
            this.pool_projectile_create ();
        }
    }
    private void pool_projectile_create () {
        // Create projectile instance and add to pool
        PlayerSpellProjectile projectile = Instantiate (this.soSpellAttack.projectilePrefab, this.spellHolder) as PlayerSpellProjectile;
        projectile.transform.localPosition = Vector3.zero;
        this.projectilesInPool.Add (projectile);

        // Initialize projectile
        projectile.initialize_components (this.spellHolder);
        projectile.initialize_properties (this.soSpellAttack.projectileSpeed, this.soSpellAttack.projectileLifeTime, this.soSpellAttack.collisionTags);
        projectile.hide ();
    }
    private PlayerSpellProjectile pool_projectile_get () {
        // Iterate through our list of pre-made projectiles
        for (int i = 0; i < this.projectilesInPool.Count; i++) {
            // If the current projectile isn't being used, then use it
            if (!this.projectilesInPool [i].is_activated ()) {
                return this.projectilesInPool [i];
            }
        }

        // Otherwise, create a new effect instance
        this.pool_projectile_create ();
        return this.projectilesInPool [this.projectilesInPool.Count - 1];
    }
    #endregion

    #region SPELL ATTACK
    public void attack () {
        // Get a projectile from our pool
        PlayerSpellProjectile projectile = this.pool_projectile_get ();

        // Launch projectile
        projectile.attack (this.soSpellAttack.damage (), this.attackPoint.position, this.attackPoint.forward);

        // Start cool down
        this.attackReady = false;
        this.StartCoroutine ("attack_cool_down");
    }
    private IEnumerator attack_cool_down () {
        // Wait cool down time
        float t = 0f;
        while (t < this.soSpellAttack.attackCoolDown) {
            t += Time.deltaTime;
            yield return new WaitForEndOfFrame ();
        }

        // We are ready to attack again
        this.attackReady = true;
        this.StopAllCoroutines ();
    }

    public bool attack_ready () {
        return this.attackReady;
    }
    #endregion
}
