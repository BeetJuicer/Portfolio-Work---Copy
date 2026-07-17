using UnityEngine;
using DG.Tweening;

public class BobbingIdle : MonoBehaviour
{
    [Header("Bobbing Settings")]
    [SerializeField] private float bobHeight = 0.5f; // How high/low it goes
    [SerializeField] private float duration = 1.5f;   // Time for one full up-and-down cycle
    [SerializeField] private Ease easeType = Ease.InOutQuad; // Smooth deceleration at the peaks

    private Tween bobTween;

    void Start()
    {
        StartBobbing();
    }

    void StartBobbing()
    {
        // Calculate the target relative position (Y-axis)
        float targetY = transform.localPosition.y + bobHeight;

        // Create the upward movement, then configure it to loop and bounce
        bobTween = transform.DOLocalMoveY(targetY, duration * 0.5f)
            .SetEase(easeType)
            .SetLoops(-1, LoopType.Yoyo); // -1 means infinite loops, Yoyo makes it reverse direction
    }

    private void OnDestroy()
    {
        // Always kill your tweens when the object is destroyed to avoid memory leaks
        if (bobTween != null)
        {
            bobTween.Kill();
        }
    }
}