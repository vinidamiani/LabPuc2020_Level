using UnityEngine;

[RequireComponent (typeof (Animator))]
public class LevelTriggerUI : MonoBehaviour{
    // Animator and level components
    private Animator animator;
    private LevelTrigger currentLevelTrigger;

    // Animator properties
    private readonly string animFadeIn = "Fade In";

    #region INITIALIZE
    private void Awake () {
        // Initialize UI components
        this.animator = this.GetComponent<Animator> ();
    }
    #endregion

    #region ANIMATOR
    public void animate_level_load_in (LevelTrigger levelTrigger) {
        // Set our level trigger
        this.currentLevelTrigger = levelTrigger;

        // Animate the fade in
        this.animator.SetTrigger (this.animFadeIn);
    }

    public void event_level_load () {
        // <NOTE>
        // This method is valled from the animator itself
        // </NOTE>

        // Ignore method if our level trigger does not exist
        if (this.currentLevelTrigger == null)
            return;

        // Otherwise, load the level
        this.currentLevelTrigger.level_load ();
    }
    #endregion
}
