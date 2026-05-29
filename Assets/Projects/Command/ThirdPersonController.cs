using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CommandPattern
{

    [RequireComponent(typeof(CharacterController))]
    public class ThirdPersonController : TimeReversible
    {
        [Header("Movement")]
        [SerializeField] private float speed = 6f;
        [SerializeField] private float jumpHeight = 1.5f;
        [SerializeField] private float gravity = -9.81f;

        private CharacterController controller;
        private Vector3 velocity;

        //temp
        Animator animator;

        void Start()
        {
            controller = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();
        }

        void Update()
        {
            animator.SetBool("reversing", isReversing);

            if (isReversing)
                return;
                
            HandleMovement();
        }
        private void HandleMovement()
        {
            // Ground check (uses the CC's built-in flag)
            if (controller.isGrounded && velocity.y < 0)
                velocity.y = -2f; // Small negative to keep grounded

            // Horizontal input
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 move = Vector3.right * x + Vector3.forward * z;
            controller.Move(move * speed * Time.deltaTime);

            if (move.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(move.normalized);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
            }

            animator.SetBool("running", move.sqrMagnitude > 0.01f);

            // Jump
            if (Input.GetButtonDown("Jump") && controller.isGrounded)
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

            // Apply gravity
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }
    }
}