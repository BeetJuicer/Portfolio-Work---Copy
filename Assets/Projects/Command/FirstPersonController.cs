using UnityEngine;
using System.Collections;

namespace CommandPattern
{
    /// <summary>
    /// First Person Controller using Unity's CharacterController.
    /// 
    /// Setup:
    ///   1. Add this script to your Player GameObject (which has a CharacterController component).
    ///   2. Assign the Camera (child of Player) to the 'playerCamera' field.
    ///   3. Optionally adjust values in the Inspector.
    /// 
    /// Controls:
    ///   WASD / Arrow Keys  — Move
    ///   Mouse              — Look
    ///   Space              — Jump
    ///   Left Shift         — Sprint
    ///   Left Ctrl          — Crouch
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class FirstPersonController : MonoBehaviour
    {
        // ─── References ────────────────────────────────────────────────────────────
        [Header("References")]
        [Tooltip("The camera attached as a child of this object.")]
        public Camera playerCamera;

        // ─── Look ──────────────────────────────────────────────────────────────────
        [Header("Look")]
        public float mouseSensitivity = 2f;
        public float maxLookAngle = 85f;   // degrees up/down clamp
        public bool invertY = false;

        // ─── Movement ──────────────────────────────────────────────────────────────
        [Header("Movement")]
        public float walkSpeed = 5f;
        public float sprintSpeed = 9f;
        public float crouchSpeed = 2.5f;

        [Header("Crouch")]
        public float standingHeight = 2f;
        public float crouchHeight = 1f;
        public float crouchTransitionSpeed = 10f;

        [Header("Jump & Gravity")]
        public float jumpHeight = 1.2f;
        public float gravity = -18f;  // stronger than default -9.8 feels better
        public float groundedGravity = -2f; // small constant push down when grounded to stay grounded on slopes

        [Header("Ground Check")]
        [Tooltip("A point at the base of the character to sphere-cast from.")]
        public Transform groundCheck;
        public float groundCheckRadius = 0.25f;
        public LayerMask groundMask;

        // ─── Internal State ────────────────────────────────────────────────────────
        private CharacterController _cc;

        // Look
        private float _pitch = 0f;           // vertical look angle (clamped)

        // Movement
        private Vector3 _velocity;           // current XZ + Y velocity
        private bool _isGrounded;
        private bool _isCrouching;

        // ─────────────────────────────────────────────────────────────────────────

        void Awake()
        {
            _cc = GetComponent<CharacterController>();

            // Auto-find camera if not assigned
            if (playerCamera == null)
                playerCamera = GetComponentInChildren<Camera>();

            // Auto-create ground check if not assigned
            if (groundCheck == null)
            {
                GameObject gc = new GameObject("GroundCheck");
                gc.transform.SetParent(transform);
                gc.transform.localPosition = new Vector3(0f, -standingHeight * 0.5f, 0f);
                groundCheck = gc.transform;
            }

            // Lock and hide cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        void Update()
        {
            HandleLook();
            HandleCrouch();
            HandleMovement();

            // Toggle cursor lock with Escape
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else if (Input.GetMouseButtonDown(0) && Cursor.lockState == CursorLockMode.None)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        // ─── Look ──────────────────────────────────────────────────────────────────

        void HandleLook()
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * (invertY ? 1f : -1f);

            // Rotate the player body left/right
            transform.Rotate(Vector3.up * mouseX);

            // Rotate the camera up/down (pitch), clamped
            _pitch = Mathf.Clamp(_pitch + mouseY, -maxLookAngle, maxLookAngle);
            playerCamera.transform.localEulerAngles = new Vector3(_pitch, 0f, 0f);
        }

        // ─── Crouch ────────────────────────────────────────────────────────────────

        void HandleCrouch()
        {
            bool wantCrouch = Input.GetKey(KeyCode.LeftControl);

            // Prevent standing up if something is above
            if (_isCrouching && !wantCrouch)
            {
                if (CeilingBlocked()) return;
            }

            _isCrouching = wantCrouch;

            float targetHeight = _isCrouching ? crouchHeight : standingHeight;
            float currentHeight = _cc.height;

            if (!Mathf.Approximately(currentHeight, targetHeight))
            {
                float newHeight = Mathf.Lerp(currentHeight, targetHeight, Time.deltaTime * crouchTransitionSpeed);

                // Adjust center and position so feet stay on ground
                float heightDiff = newHeight - currentHeight;
                _cc.height = newHeight;
                _cc.center = new Vector3(0f, newHeight * 0.5f, 0f);

                // Move player up/down to keep feet planted
                transform.position += new Vector3(0f, heightDiff * 0.5f, 0f);
            }
        }

        bool CeilingBlocked()
        {
            // Sphere cast upward to check for overhead obstruction
            return Physics.SphereCast(
                transform.position + Vector3.up * crouchHeight,
                _cc.radius,
                Vector3.up,
                out _,
                standingHeight - crouchHeight
            );
        }

        // ─── Movement & Gravity ────────────────────────────────────────────────────

        void HandleMovement()
        {
            // Ground check
            _isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundMask);

            // Gravity
            if (_isGrounded && _velocity.y < 0f)
                _velocity.y = groundedGravity;

            _velocity.y += gravity * Time.deltaTime;

            // Jump
            if (_isGrounded && Input.GetButtonDown("Jump"))
                _velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

            // Horizontal input
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            // Pick speed
            float speed = _isCrouching ? crouchSpeed
                        : Input.GetKey(KeyCode.LeftShift) ? sprintSpeed
                        : walkSpeed;

            // Move relative to the player's facing direction (XZ only)
            Vector3 move = transform.right * h + transform.forward * v;
            move = Vector3.ClampMagnitude(move, 1f); // normalize diagonals

            _cc.Move((move * speed + _velocity) * Time.deltaTime);
        }

        // ─── Gizmos ────────────────────────────────────────────────────────────────

        void OnDrawGizmosSelected()
        {
            if (groundCheck != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
            }
        }
    }
}