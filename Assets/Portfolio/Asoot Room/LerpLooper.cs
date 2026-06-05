using UnityEngine;
using DG.Tweening;

public class LerpLooper : MonoBehaviour
{
    [Header("Positions")]
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;

    [Header("Settings")]
    [SerializeField] private float duration = 1f;
    [SerializeField] private float pauseDuration = 0.5f;
    [SerializeField] private Ease easeType = Ease.InOutSine;

    private void Start()
    {
        PlaySequence();
    }

    private void PlaySequence()
    {
        Sequence seq = DOTween.Sequence();

        seq.Append(transform.DOMove(pointB.position, duration).SetEase(easeType));
        seq.AppendInterval(pauseDuration);
        seq.Append(transform.DOMove(pointA.position, duration).SetEase(easeType));
        seq.AppendInterval(pauseDuration);

        seq.SetLoops(-1, LoopType.Restart);
    }
}