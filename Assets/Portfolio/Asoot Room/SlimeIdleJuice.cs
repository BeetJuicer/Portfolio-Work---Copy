using UnityEngine;
using DG.Tweening;

public class SlimeIdleJuice : MonoBehaviour
{
    [Header("Squish Settings")]
    [SerializeField] private float duration = 0.6f;
    [SerializeField] private Vector3 squishScale = new Vector3(1.2f, 0.8f, 1f); // Wide and short
    [SerializeField] private Vector3 stretchScale = new Vector3(0.9f, 1.1f, 1f); // Narrow and tall

    [Header("Ground Anchoring (Optional)")]
    [SerializeField] private bool anchorToGround = true;
    [SerializeField] private float yOffset = -0.1f; // Adjust based on sprite pivot

    private Sequence idleSequence;
    private Vector3 originalScale;
    private Vector3 originalPosition;

    void Start()
    {
        originalScale = transform.localScale;
        originalPosition = transform.localPosition;

        StartSquishAnimation();
    }

    void StartSquishAnimation()
    {
        // Kill any existing tween on this object just in case
        idleSequence?.Kill();

        // Create a looping sequence
        idleSequence = DOTween.Sequence();

        // 1. Squish Down
        idleSequence.Append(transform.DOScale(Vector3.Scale(originalScale, squishScale), duration)
            .SetEase(Ease.InOutQuad));
        if (anchorToGround)
        {
            idleSequence.Join(transform.DOLocalMoveY(originalPosition.y + (yOffset * (1 - squishScale.y)), duration)
                .SetEase(Ease.InOutQuad));
        }

        // 2. Stretch Up
        idleSequence.Append(transform.DOScale(Vector3.Scale(originalScale, stretchScale), duration)
            .SetEase(Ease.InOutQuad));
        if (anchorToGround)
        {
            idleSequence.Join(transform.DOLocalMoveY(originalPosition.y + (yOffset * (1 - stretchScale.y)), duration)
                .SetEase(Ease.InOutQuad));
        }

        // Set the loop to infinite and alternate back and forth
        idleSequence.SetLoops(-1, LoopType.Yoyo);
    }

    private void OnDestroy()
    {
        // Clean up your tweens when the object is destroyed
        idleSequence?.Kill();
    }
}