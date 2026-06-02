using System;
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
    private Vector3 moveDirection;

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
        print("movement: " + movementEnabled);
        if (!movementEnabled)
            return;

        float currentDeceleration = decelerationAmount * decelerationScale * Time.fixedDeltaTime;
        currentVelocity.x = Mathf.MoveTowards(currentVelocity.x, 0f, currentDeceleration);
        currentVelocity.z = Mathf.MoveTowards(currentVelocity.z, 0f, currentDeceleration);

        if (!IsGrounded())
        {
            float currentGravity = gravityAmount * gravityScale * Time.fixedDeltaTime;
            currentVelocity.y = Mathf.MoveTowards(currentVelocity.y, -maxFallSpeed, currentGravity);
        }

        Vector3 move = (currentVelocity.x * transform.right +
                        currentVelocity.z * transform.forward +
                        currentVelocity.y * transform.up) * Time.fixedDeltaTime;

        characterController.Move(move);
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
        //horizontal -- whole body
        transform.Rotate(Vector3.up * delta.x);

        //vertical only camera
        pitch -= delta.y;
        pitch = Mathf.Clamp(pitch, minLookAngle, maxLookAngle);
        fpsCamera.transform.localRotation = Quaternion.Euler(pitch, 0, 0);
    }
    #endregion

    #region IJumpable
    public bool IsGrounded() => characterController.isGrounded;
    public void Jump(float force) => currentVelocity.y += force;

    public void SetVerticalLookRange(float min, float max)
    {
        minLookAngle = min;
        maxLookAngle = max;
    }

    #endregion
}
