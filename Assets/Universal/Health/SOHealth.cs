using UnityEngine;

// Universal health scriptable object
[CreateAssetMenu (fileName = "New Health", menuName = "Game/Health")]
public class SOHealth : ScriptableObject {
    // Health properties
    [Header ("Health Setup")]
    [Range (1, 1000)]
    public int healthMax = 10;
    [Range (0, 1000)]
    public int armourMax = 0;
}
