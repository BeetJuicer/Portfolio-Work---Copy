using UnityEngine;
public interface IMovable
{
    void Move(Vector3 moveAmount);
    void AddVelocity(Vector3 velocity);
    void SetVelocity(Vector3 velocity);
    void SetVelocityX(float x);
    void SetVelocityY(float y);
    void StopMovement();

    void SetDeceleration(float deceleration);
    void SetMaxFallSpeed(float speed);
    void SetGravityScale(float scale);

    Vector3 MoveDirection { get; }
    Vector3 LastMoveDirection { get; }
    Vector3 Velocity { get; }
    float GravityScale { get; }
    bool IsMoving { get; }
}

public interface IMovable2D : IMovable
{
    public int FacingDirection { get; }
}

public interface IMovable3D : IMovable
{
    void SetVelocityZ(float z);
    Vector3 FacingDirection3D { get; }
    void LookAt(Vector2 direction);
    void SetVerticalLookRange(float min, float max);
}

public interface IJumpable
{
    void Jump(float force);
    bool IsGrounded();
}

public interface IDashable
{
    void Dash(Vector2 direction);
}

public interface IKnockbackable
{
    void Knockback(Vector2 direction, float force);
}
