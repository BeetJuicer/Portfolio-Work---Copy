using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FPSMovement : MonoBehaviour, IMovable3D, IJumpable
{
    #region Serialized Fields
    [Header("Look")]
    [SerializeField] private Camera fpsCamera;

    [Header("Movement")]
    [SerializeField] private float gravityAmount = 9.8f;
    [SerializeField] private float decelerationAmount;
    private float maxFallSpeed = 10f;
    private float gravityScale = 1f;
    private float decelerationScale;
    private Vector3 currentVelocity = Vector3.zero;
    #endregion

    #region References
    private CharacterController characterController;
    #endregion

    #region IMovable Properties
    private Vector3 lastMoveDirection;
    private Vector3 moveDirection; // NOTE: Ensure an external input script sets this via your interface methods if needed, or process it internally.

    public Vector3 FacingDirection3D { get; private set; } = Vector3.forward;
    public Vector3 MoveDirection => moveDirection;
    public Vector3 LastMoveDirection => lastMoveDirection;
    public Vector3 Velocity => currentVelocity;
    public float GravityScale => gravityScale;
    public bool IsMoving => currentVelocity.sqrMagnitude > 0.0001f;
    #endregion

    private float pitch = 0f;
    private float minLookAngle;
    private float maxLookAngle;

    [Header("Platform Tracking")]
    [SerializeField] private LayerMask platformLayer;
    private MovingPlatform3D activePlatform;
    private Vector3 activePlatformVelocity;

    private bool movementEnabled = true;

    #region Unity Methods
    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    public void EnableMovement() => movementEnabled = true;
    public void DisableMovement() => movementEnabled = false;

    private void FixedUpdate()
    {
        // 1. Apply Deceleration & Gravity forces to raw velocity
        float currentDeceleration = decelerationAmount * decelerationScale * Time.fixedDeltaTime;
        currentVelocity.x = Mathf.MoveTowards(currentVelocity.x, 0f, currentDeceleration);
        currentVelocity.z = Mathf.MoveTowards(currentVelocity.z, 0f, currentDeceleration);

        if (!IsGrounded())
        {
            float currentGravity = gravityAmount * gravityScale * Time.fixedDeltaTime;
            currentVelocity.y = Mathf.MoveTowards(currentVelocity.y, -maxFallSpeed, currentGravity);
        }

        // 2. Track platform state
        CheckForPlatform();

        // 3. Construct Final Movement Vector
        Vector3 finalMoveDelta = Vector3.zero;

        if (movementEnabled)
        {
            // Transform local velocities relative to character orientation
            finalMoveDelta += (currentVelocity.x * transform.right +
                               currentVelocity.z * transform.forward +
                               currentVelocity.y * transform.up) * Time.fixedDeltaTime;
        }
        else
        {
            // If movement is disabled, still inherit vertical gravity tracking
            finalMoveDelta += (currentVelocity.y * transform.up) * Time.fixedDeltaTime;
        }

        // 4. Inject Platform Delta (Ensures player stays attached even if movement is disabled)
        if (activePlatform != null)
        {
            finalMoveDelta += activePlatformVelocity * Time.fixedDeltaTime;
        }

        // 5. Single, definitive physics calculation per frame
        characterController.Move(finalMoveDelta);
    }

    private void CheckForPlatform()
    {
        RaycastHit hit;
        // Shift origin slightly up from base pivot to handle clipping variations safely
        Vector3 origin = transform.position + (Vector3.up * 0.1f);

        if (Physics.Raycast(origin, Vector3.down, out hit, 0.3f, platformLayer))
        {
            MovingPlatform3D platform = hit.collider.GetComponent<MovingPlatform3D>();
            if (platform != null)
            {
                activePlatform = platform;
                activePlatformVelocity = platform.PlatformVelocity;
                return;
            }
        }

        // If we left the platform, hand off momentum to our internal horizontal velocities
        if (activePlatform != null)
        {
            // Convert global platform velocity to local direction vectors so deceleration slides it out cleanly
            Vector3 localPlatformVel = transform.InverseTransformDirection(activePlatformVelocity);
            currentVelocity.x += localPlatformVel.x;
            currentVelocity.z += localPlatformVel.z;

            activePlatform = null;
            activePlatformVelocity = Vector3.zero;
        }
    }
    #endregion

    #region IMovable
    public void AddVelocity(Vector3 velocity) => currentVelocity += velocity;
    public void Move(Vector3 moveAmount) => characterController.Move(moveAmount);
    public void SetVelocity(Vector3 velocity) => currentVelocity = velocity;
    public void SetVelocityX(float velocityX) => currentVelocity.x = velocityX;
    public void SetVelocityY(float velocityY) => currentVelocity.y = velocityY;
    public void SetVelocityZ(float velocityZ) => currentVelocity.z = velocityZ;

    public void ClampVelocityY(float max) => currentVelocity.y = Mathf.Min(currentVelocity.y, max);
    public void StopMovement() => currentVelocity = Vector3.zero;
    public void SetGravityScale(float scale) => gravityScale = scale;
    public void SetDeceleration(float scale) => decelerationScale = scale;
    public void SetMaxFallSpeed(float speed) => maxFallSpeed = speed;

    public void LookAt(Vector2 delta)
    {
        transform.Rotate(Vector3.up * delta.x);

        pitch -= delta.y;
        pitch = Mathf.Clamp(pitch, minLookAngle, maxLookAngle);
        fpsCamera.transform.localRotation = Quaternion.Euler(pitch, 0, 0);
    }
    #endregion

    #region IJumpable
    public bool IsGrounded() => characterController.isGrounded;
    public void Jump(float force) { if (movementEnabled && IsGrounded()) currentVelocity.y += force; }

    public void SetVerticalLookRange(float min, float max)
    {
        minLookAngle = min;
        maxLookAngle = max;
    }
    #endregion
}