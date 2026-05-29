using StateMachineCore;

namespace Assets.Projects.StateMachine.SideScroll
{
    using System;
    using UnityEngine;

    [RequireComponent(typeof(Rigidbody2D))]
    public class Movement_Physics : MonoBehaviour//, IMovable, IJumpable
    {
        private Rigidbody2D rb; 
        public int FacingDirection => throw new System.NotImplementedException();

        public Vector3 MoveDirection => throw new System.NotImplementedException();

        public Vector3 LastMoveDirection => throw new System.NotImplementedException();

        public Vector3 Velocity => currentVelocity;

        private Vector3 currentVelocity = Vector3.zero;
        [SerializeField] private Transform groundCheckPos;
        [SerializeField] private Vector2 groundCheckSize;
        [SerializeField] private LayerMask whatIsGround;



        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        public void AddVelocity(Vector3 velocity)
        {
            throw new System.NotImplementedException();
        }

        public bool IsGrounded()
        {
            return Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0f, whatIsGround);
        }

        public void Jump(float force)
        {
            rb.AddForceY(force, ForceMode2D.Impulse);
        }

        public void Move(Vector3 moveAmount)
        {
            throw new System.NotImplementedException();
        }

        public void SetVelocityX(float velocityX)
        {
            currentVelocity.x = velocityX;
        }

        public void SetVelocityY(float velocityY)
        {
            currentVelocity.y = velocityY;
        }

        public void StopMovement()
        {
            currentVelocity = Vector3.zero;
        }

        public void Jump()
        {
            throw new System.NotImplementedException();
        }

        private void FixedUpdate()
        {
            rb.linearVelocity = currentVelocity;
        }

        public void SetGravityScale(float gravity)
        {
            rb.gravityScale = gravity;
        }
    }
}