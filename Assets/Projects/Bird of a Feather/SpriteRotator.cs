using DG.Tweening;
using StateMachineCore;
using System.Collections;
using UnityEngine;

public class SpriteRotator : MonoBehaviour
{
    IMovable2D movable;
    float facingDir;
    SidescrollerCharacterStateMachine sm;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        movable = GetComponentInParent<IMovable2D>();
        sm = GetComponentInParent<SidescrollerCharacterStateMachine>();
    }

    // Update is called once per frame
    void Update()
    {
        if (sm.currentState == sm.fallState)
        {
            RotateByAngleFaceDirection(120f);
        }
        else if (sm.currentState == sm.rideWindState ||
                 sm.currentState == sm.diveState)
        {
            RotateByAngleFaceDirection(360f);
        }
        else
        {
            transform.rotation = Quaternion.identity;
        }
    }

    void RotateByAngleFaceDirection(float rotationSpeed)
    {
        bool flipping = false;
        Vector3 velNormalized = movable.Velocity.normalized;

        if(velNormalized.x != 0 && Mathf.Sign(velNormalized.x) != facingDir)
        {
            facingDir = velNormalized.x;
            flipping = true;
        }

        float zRotation = Mathf.Atan2(velNormalized.y, Mathf.Abs(velNormalized.x)) * Mathf.Rad2Deg;
        zRotation *= Mathf.Sign(facingDir);


        Quaternion targetRotation = Quaternion.Euler(0, 0, zRotation);
        if(flipping)
        {
            // instant snap when going to a different direction
            transform.rotation = targetRotation;
            return;
        }

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    public void RotateX(float amount, float duration)
    {
        print("rotating");
        Vector3 target = transform.localEulerAngles + new Vector3(amount, 0f, 0f);
        transform.DOLocalRotate(target, duration).SetEase(Ease.Linear);
    }

    public void StretchX(float amount)
    {
        transform.DOScaleX(amount, 0f);
    }

    public void StretchY(float amount)
    {
        transform.DOScaleY(amount, 0f);
    }


}
