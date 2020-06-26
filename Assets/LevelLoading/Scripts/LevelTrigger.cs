using UnityEngine;

[RequireComponent (typeof (BoxCollider))]
[RequireComponent (typeof (Rigidbody))]
public class LevelTrigger : MonoBehaviour{
    // Trigger components
    private BoxCollider boxCollider;
    private Rigidbody rigidBody;

    // Level properties
    [Header ("Level Load Setup")]
    [SerializeField]
    private int levelIndex = 0;
    [SerializeField]
    private string levelName = "Level Name";
    [SerializeField]
    private bool loadByName = true;          // If you would rather use the scene index, then set this to false

    private bool isLaodingLevel = false;

    #region INITIALIZE
    private void Awake () {
        // Initialize trigger components
        this.boxCollider = this.GetComponent<BoxCollider> ();
        this.rigidBody = this.GetComponent<Rigidbody> ();
    }
    private void Start () {
        // Initialize trigger properties
        this.boxCollider.isTrigger = true;
        this.rigidBody.isKinematic = true;
    }
    #endregion

    #region LEVEL LOADING AND PLAYER COLLISION
    public void level_load () {
        // Otherwise, directly load the level now
        if (this.loadByName)
            LevelManager.level_load (this.levelName);
        else 
            LevelManager.level_load (this.levelIndex);
    }
    private bool level_can_load ()
        => (this.loadByName) ? LevelManager.level_can_load (this.levelName) : LevelManager.level_can_load (this.levelIndex);

    public void OnTriggerEnter (Collider col) {
        // If we are already loading then ignore
        if (this.isLaodingLevel)
            return;
        // We are not colliding with the player so ignore
        if (!col.CompareTag (LevelManager.playerTag))
            return;
        // Check if we can actually load the level
        if (!this.level_can_load ())
            return;

        // Since we can load the level, show the fade effect (which will load the level itself)
        this.isLaodingLevel = true;
        LevelManager.levelTriggerUI.animate_level_load_in (this);
    }
    #endregion
}
