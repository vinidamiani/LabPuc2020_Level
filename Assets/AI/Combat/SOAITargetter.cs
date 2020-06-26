using UnityEngine;

[CreateAssetMenu (fileName = "New AI (Targetter)", menuName = "AI/Targetter")]
public class SOAITargetter : ScriptableObject{
    // Targetter properties
    #region TARGETTING STRATEGIES
    [Header ("Targetting Strategy")]
    public bool alwaysFollowTarget = false;         // If we want the AI to always follow the target
    #endregion

    #region DISTANCING PROPERTIES
    [Header ("Distancing")]
    [Range (0.1f, 5.0f)]
    [Tooltip ("The distance for the AI to stop in front of the target.")]
    public float stoppingDistance = 2.0f;

    [Range (2.0f, 100f)]
    [Tooltip ("The maximum distance the AI can a notice a target from.")]
    public float awarenessDistance = 10f;

    [Range (5.0f, 100f)]
    [Tooltip ("The maximum distance for the AI to stop following the target.")]
    public float forgetDistance = 20f;
    #endregion

    #region VIEWPORT PROPERTIES
    [Header ("Viewport")]
    [Range (0f, 10f)]
    [Tooltip ("The vertical offset from the base of the AI in which the 'eyes' are to be located.")]
    public float viewOffset = 2.0f;
    [Range (10f, 360f)]
    [Tooltip ("The angle at which a target can be in for the AI to notice (360 means that the view is irrelevant).")]
    public float viewAngle = 70f;

    [Tooltip ("Masking used for obstruction tests (including the target's layer).")]
    public LayerMask viewObstructionMask;
    #endregion
}
