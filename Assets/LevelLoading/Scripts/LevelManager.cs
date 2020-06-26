using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {
    // Manager instance
    private static LevelManager instance;

    // Level components
    public Player playerPrefab;
    public static LevelTriggerUI levelTriggerUI;

    // Level properties
    public static string playerTag = "Player";              // Change this if you if you change your player's tag
    public static string spawnTag = "Spawn Point";

    #region INITIALIZE
    private void Awake () {
        // Make sure there is only one level manager instance
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad (instance);
        }
        else 
            Destroy (gameObject);
        

        this.level_ui_find ();
    }
    private void OnLevelWasLoaded (int level) {
        DontDestroyOnLoad (instance);

        // Check if we need to make another player
        if (!GameObject.FindObjectOfType<Player> ()) {
            Player player = Instantiate (this.playerPrefab);
            player.playerTransform.position = GameObject.FindGameObjectWithTag (spawnTag).transform.position;
        }

        this.level_ui_find ();
    }
    #endregion

    #region LEVEL
    private void level_ui_find () {
        // Find the UI if we haven't already
        if (levelTriggerUI == null)
            levelTriggerUI = GameObject.FindObjectOfType<LevelTriggerUI> ();
    }
    public static void level_load (int levelIndex) {
        // Ignore call if the scene does not exist
        if (!level_can_load (levelIndex))
            return;

        // Load scene by index
        SceneManager.LoadSceneAsync (levelIndex);
    }
    public static void level_load (string levelName) {
        // Ignore call if the scene does not exist
        if (!level_can_load (levelName))
            return;

        // Load scene by name
        SceneManager.LoadSceneAsync (levelName);
    }

    public static bool level_can_load (int levelIndex)
        => levelIndex <= SceneManager.sceneCountInBuildSettings - 1;
    public static bool level_can_load (string levelName) {
        // Ignore if name is empty
        if (string.IsNullOrEmpty (levelName))
            return false;

        // Search through all level names
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++) {
            // Get the name of the level
            var scenePath = SceneUtility.GetScenePathByBuildIndex (i);
            var lastSlash = scenePath.LastIndexOf ("/");
            var sceneName = scenePath.Substring (lastSlash + 1, scenePath.LastIndexOf (".") - lastSlash - 1);

            // Found a match
            if (string.Compare (levelName, sceneName, true) == 0)
                return true;
        }

        // No match was found
        return false;
    }
    #endregion
}
