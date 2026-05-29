using StateMachineCore;
using System;
using UnityEngine;

public class SpriteFlipper : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    IMovable2D movable;
    float lastMoveDirX = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        movable = GetComponentInParent<IMovable2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float curDir = Mathf.Sign(movable.Velocity.x);
        if (movable.Velocity.x != 0 && 
            curDir != lastMoveDirX)
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;
            lastMoveDirX = curDir;
        }
    }
}
