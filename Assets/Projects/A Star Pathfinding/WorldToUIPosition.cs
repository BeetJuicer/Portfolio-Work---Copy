using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Copies a worldspace GameObject's screen position to a UI RectTransform world position.
/// Attach this to the UI element you want to reposition.
/// </summary>
public class WorldToUIPosition : MonoBehaviour
{
    [Header("References")]
    [Tooltip("The worldspace GameObject to track.")]
    public Transform worldTarget;

    [Tooltip("The Canvas containing this UI element.")]
    public Canvas canvas;

    [Tooltip("The camera used for rendering (leave null to use Camera.main).")]
    public Camera renderCamera;

    [Header("Settings")]
    [Tooltip("Optional Z offset in UI space (useful for layering).")]
    public float uiZOffset = 0f;

    [Tooltip("Update every frame (true) or only on demand (false).")]
    public bool updateEveryFrame = true;

    private RectTransform _rectTransform;
    private Camera _cam;

    void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _cam = renderCamera != null ? renderCamera : Camera.main;
    }

    void LateUpdate()
    {
        if (updateEveryFrame)
            Sync();
    }

    /// <summary>
    /// Call this manually if updateEveryFrame is false.
    /// </summary>
    public void Sync()
    {
        if (worldTarget == null || canvas == null || _cam == null) return;

        // 1. Get screen position of the worldspace object
        Vector3 screenPos = _cam.WorldToScreenPoint(worldTarget.position);

        // Object is behind the camera — hide or skip
        if (screenPos.z < 0f)
        {
            _rectTransform.gameObject.SetActive(false);
            return;
        }

        _rectTransform.gameObject.SetActive(true);

        // 2. Convert screen position → UI world position
        Vector3 uiWorldPos = ScreenToCanvasWorld(screenPos);
        uiWorldPos.z += uiZOffset;

        _rectTransform.position = uiWorldPos;
    }

    /// <summary>
    /// Converts a screen-space position to a world position on the Canvas plane.
    /// Works for Screen Space - Camera and World Space canvas render modes.
    /// </summary>
    private Vector3 ScreenToCanvasWorld(Vector3 screenPos)
    {
        switch (canvas.renderMode)
        {
            case RenderMode.ScreenSpaceOverlay:
                // In overlay mode, screen pos == canvas pos directly
                return new Vector3(screenPos.x, screenPos.y, 0f);

            case RenderMode.ScreenSpaceCamera:
            case RenderMode.WorldSpace:
            default:
                // Use the canvas's world camera (may differ from render camera)
                Camera canvasCam = canvas.worldCamera != null ? canvas.worldCamera : _cam;
                RectTransformUtility.ScreenPointToWorldPointInRectangle(
                    canvas.GetComponent<RectTransform>(),
                    screenPos,
                    canvasCam,
                    out Vector3 worldPoint
                );
                return worldPoint;
        }
    }
}