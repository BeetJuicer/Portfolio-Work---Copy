using UnityEngine;
using DG.Tweening;
using StateMachineCore;

public class SpriteWalkAnimation : MonoBehaviour
{
    [Header("Walk Settings")]
    public float stepDuration = 0.3f;

    [Header("Squash & Stretch")]
    public float stretchY = 1.15f;
    public float squashY = 0.85f;

    [Header("Tilt")]
    public float tiltAngle = 5f;

    [Header("Bob")]
    public float bobHeight = 0.05f;

    private Sequence _walkSequence;
    private Vector3 _originScale;
    private Vector3 _originPos;

    SidescrollerCharacterStateMachine sm;
    bool walking = false;
    void Start()
    {
        _originScale = transform.localScale;
        _originPos = transform.localPosition;

        sm = GetComponentInParent<SidescrollerCharacterStateMachine>();
    }

    private void Update()
    {
        if(!walking && sm.currentState == sm.walkState)
        {
            PlayWalk();
            walking = true;
        }
        else if(walking && sm.currentState != sm.walkState)
        {
            StopWalk();
            walking=false;
        }
    }

    public void PlayWalk()
    {
        _walkSequence?.Kill(true);
        _walkSequence = DOTween.Sequence().SetLoops(-1, LoopType.Restart);

        float s = stepDuration;

        // --- Left step ---
        // Rise + stretch + tilt left
        _walkSequence.Append(transform.DOScaleY(_originScale.y * stretchY, s * 0.35f).SetEase(Ease.OutQuad));
        _walkSequence.Join(transform.DOScaleX(_originScale.x * (1f / stretchY), s * 0.35f).SetEase(Ease.OutQuad));
        _walkSequence.Join(transform.DOLocalMoveY(_originPos.y + bobHeight, s * 0.35f).SetEase(Ease.OutQuad));
        _walkSequence.Join(transform.DOLocalRotate(new Vector3(0, 0, tiltAngle), s * 0.35f).SetEase(Ease.OutQuad));

        // Drop + squash
        _walkSequence.Append(transform.DOScaleY(_originScale.y * squashY, s * 0.25f).SetEase(Ease.InQuad));
        _walkSequence.Join(transform.DOScaleX(_originScale.x * (1f / squashY), s * 0.25f).SetEase(Ease.InQuad));
        _walkSequence.Join(transform.DOLocalMoveY(_originPos.y, s * 0.25f).SetEase(Ease.InQuad));

        // Recover
        _walkSequence.Append(transform.DOScale(_originScale, s * 0.4f).SetEase(Ease.OutBack));
        _walkSequence.Join(transform.DOLocalRotate(Vector3.zero, s * 0.2f).SetEase(Ease.OutQuad));

        // --- Right step (mirror tilt) ---
        _walkSequence.Append(transform.DOScaleY(_originScale.y * stretchY, s * 0.35f).SetEase(Ease.OutQuad));
        _walkSequence.Join(transform.DOScaleX(_originScale.x * (1f / stretchY), s * 0.35f).SetEase(Ease.OutQuad));
        _walkSequence.Join(transform.DOLocalMoveY(_originPos.y + bobHeight, s * 0.35f).SetEase(Ease.OutQuad));
        _walkSequence.Join(transform.DOLocalRotate(new Vector3(0, 0, -tiltAngle), s * 0.35f).SetEase(Ease.OutQuad));

        // Drop + squash
        _walkSequence.Append(transform.DOScaleY(_originScale.y * squashY, s * 0.25f).SetEase(Ease.InQuad));
        _walkSequence.Join(transform.DOScaleX(_originScale.x * (1f / squashY), s * 0.25f).SetEase(Ease.InQuad));
        _walkSequence.Join(transform.DOLocalMoveY(_originPos.y, s * 0.25f).SetEase(Ease.InQuad));

        // Recover
        _walkSequence.Append(transform.DOScale(_originScale, s * 0.4f).SetEase(Ease.OutBack));
        _walkSequence.Join(transform.DOLocalRotate(Vector3.zero, s * 0.2f).SetEase(Ease.OutQuad));
    }

    public void StopWalk()
    {
        _walkSequence?.Kill();
        transform.DOScale(_originScale, 0.15f);
        transform.DOLocalMove(_originPos, 0.15f);
        transform.DOLocalRotate(Vector3.zero, 0.15f);
    }

    void OnDestroy() => _walkSequence?.Kill();
}