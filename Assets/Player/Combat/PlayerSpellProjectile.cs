using System.Collections;
using UnityEngine;

[RequireComponent (typeof (Rigidbody))]
[RequireComponent (typeof (BoxCollider))]
public class PlayerSpellProjectile : MonoBehaviour{
    // Controller components
    private Rigidbody rigidBody;
    private BoxCollider boxCollider;

    private GameObject visualObject;
    private ParticleSystem [] particles;

    private Transform projectileTransform;
    private Transform projectileHolder;

    // Projectile properties
    private int damage;

    private float speed;
    private float lifeTime;

    private string [] collisionTags;

    // Pool properties
    private bool isActivated = false;

    #region INITIALIZE
    public void initialize_components (Transform projectileHolder) {
        // Initialize spell components
        this.rigidBody = this.GetComponent<Rigidbody> ();
        this.boxCollider = this.GetComponent<BoxCollider> ();

        this.visualObject = this.transform.Find ("Visual").gameObject;
        this.particles = this.GetComponentsInChildren<ParticleSystem> ();

        this.projectileTransform = this.transform;
        this.projectileHolder = projectileHolder;
    }
    public void initialize_properties (float speed, float lifeTime, string [] collisionTags) {
        // Initialize spell properties
        this.speed = speed * this.rigidBody.mass;
        this.lifeTime = lifeTime;
        this.collisionTags = collisionTags;
    }
    #endregion

    #region SPELL
    public void attack (int damage, Vector3 spawnPos, Vector3 direction) {
        // Set our damage
        this.damage = damage;

        // Activate projectile
        this.isActivated = true;
        this.boxCollider.enabled = true;
        this.visualObject.SetActive (true);

        // Spawn and apply force to the projectile
        this.projectileTransform.localPosition = Vector3.zero;
        this.projectileTransform.parent = null;

        this.rigidBody.position = spawnPos;
        this.rigidBody.AddForce (direction * this.speed);

        // Show all spell particles
        for (int i = 0; i < this.particles.Length; i++) {
            this.particles [i].Play ();
        }

        // Hide the projectile after it has reached its lifetime
        this.StartCoroutine ("hide_after_lifetime");
    }

    private IEnumerator hide_after_lifetime () {
        // Continue life timer
        float t = 0;
        while (t < this.lifeTime) {
            t += Time.deltaTime;
            yield return new WaitForEndOfFrame ();
        }

        // Once completed, hide the projectile
        this.hide ();
    }
    public void hide () {
        // <NOTE>
        // This method is called from initialization and when 
        // the projectile gets destroyed.
        // </NOTE>

        // Stop any iterations
        this.StopAllCoroutines ();

        // Hide all spell particles
        for (int i = 0; i < this.particles.Length; i ++) {
            this.particles [i].Stop ();
        }

        // Stop projectile
        this.rigidBody.velocity = Vector3.zero;

        // Deactivate projectile
        this.isActivated = false;
        this.boxCollider.enabled = false;
        this.visualObject.SetActive (false);

        // Reset position
        this.projectileTransform.parent = this.projectileHolder;
        this.projectileTransform.localPosition = Vector3.zero;
    }
    public bool is_activated () {
        return this.isActivated;
    }
    #endregion

    #region COLLISION
    private void OnCollisionEnter (Collision col) {
        // Check if we can collide with the object based on its tag
        if (!this.can_collide (col.collider.tag))
            return;

        // Check if the collider has a health components
        Health healthComponent = col.collider.GetComponent<Health> ();

        // Apply damage to object
        if (healthComponent) {
            healthComponent.i_take_damage (this.damage, this.rigidBody.position);
        }

        // Hide the projectile
        this.hide ();
    }

    private bool can_collide (string tag) {
        // Check tag with all of our collision tags
        for (int i = 0; i < this.collisionTags.Length; i ++) {
            // Found a match
            if (this.collisionTags [i].Equals (tag))
                return true;
        }

        // No match
        return false;
    }
    #endregion
}
