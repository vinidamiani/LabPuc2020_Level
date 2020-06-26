using UnityEngine;

public class AIManager : MonoBehaviour{
    // AI components
    private AI [] aiInScene;

    #region INITIALIZE
    private void Awake () {
        // Initialize AI components
        this.aiInScene = Object.FindObjectsOfType<AI> ();

        foreach (AI ai in this.aiInScene) {
            ai.initialize_components ();
            ai.initialize_properties ();
        }
    }
    #endregion

    #region UPDATE
    private void Update () {
        // Update all the AI
        foreach (AI ai in this.aiInScene) {
            if (ai == null) {
                continue;
            }

            ai.ai_update ();
        }
    }
    private void LateUpdate () {
        // Update all the AI
        foreach (AI ai in this.aiInScene) {
            if (ai == null) {
                continue;
            }

            ai.ai_update_late ();
        }
    }
    #endregion
}
