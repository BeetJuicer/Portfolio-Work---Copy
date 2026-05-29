namespace StateMachineCore
{
using UnityEngine;


public class Movement_NonPhysics : MonoBehaviour, IMovable2D
{
    [SerializeField] private float speed;

    [Header("Detection")]
    [SerializeField] private Vector2 boxCastSize;
    [SerializeField] private LayerMask whatIsObstacle;
    [SerializeField] private Transform castPos;
    [SerializeField] private float cornerCorrection = 0.1f;

    private bool isMoving = false;
    private Vector3 direction = Vector2.right;
    private bool canMove = true;

    public int FacingDirection => facingDirection;

    public Vector3 MoveDirection => throw new System.NotImplementedException();

    public Vector3 LastMoveDirection => throw new System.NotImplementedException();

        public Vector3 Velocity => throw new System.NotImplementedException();

        public float GravityScale => throw new System.NotImplementedException();

        public bool IsMoving => throw new System.NotImplementedException();

        private int facingDirection;

    private void Update()
    {
        if (!isMoving)
            return;


        Vector2 moveAmount = (Vector2)direction * speed * Time.deltaTime;
        Vector2 adjustedMove = GetAdjustedMovement(moveAmount);

        transform.Translate(adjustedMove, Space.World);

        if (direction.x != 0)
        {
            facingDirection = (int)Mathf.Sign(direction.x);
        }
    }

    private Vector2 GetAdjustedMovement(Vector2 moveAmount)
    {
        Vector2 xMove = new Vector2(moveAmount.x, 0);
        Vector2 yMove = new Vector2(0, moveAmount.y);

        bool canMoveX = CanMove(xMove.normalized, Mathf.Abs(moveAmount.x));
        bool canMoveY = CanMove(yMove.normalized, Mathf.Abs(moveAmount.y));

        return new Vector2(
            canMoveX ? xMove.x : 0,
            canMoveY ? yMove.y : 0
        );
    }

    private bool CanMove(Vector2 direction, float distance)
    {
        RaycastHit2D hit = PhysicsHelper.BoxCast(
            castPos.position,
            boxCastSize,
            0f,
            direction,
            distance,
            whatIsObstacle
        );

        if (hit.collider == null) return true;

        Vector2 perpendicular = new Vector2(direction.y, -direction.x);

        RaycastHit2D hitRight = PhysicsHelper.BoxCast(
            (Vector2)castPos.position + perpendicular * cornerCorrection,
            boxCastSize, 0f, direction, distance, whatIsObstacle
        );
        if (hitRight.collider == null)
        {
            transform.position += (Vector3)(perpendicular * cornerCorrection);
            return true;
        }

        RaycastHit2D hitLeft = PhysicsHelper.BoxCast(
            (Vector2)castPos.position - perpendicular * cornerCorrection,
            boxCastSize, 0f, direction, distance, whatIsObstacle
        );
        if (hitLeft.collider == null)
        {
            transform.position -= (Vector3)(perpendicular * cornerCorrection);
            return true;
        }

        return false;
    }

    public void Move(Vector3 moveAmount)
    {
        throw new System.NotImplementedException();
    }

    public void StopMovement()
    {
        throw new System.NotImplementedException();
    }

        public void AddVelocity(Vector3 velocity)
        {
            throw new System.NotImplementedException();
        }

        public void SetVelocityX(float velocityX)
        {
            throw new System.NotImplementedException();
        }

        public void SetVelocityY(float velocityY)
        {
            throw new System.NotImplementedException();
        }

        public void SetGravityScale(float gravity)
        {
            throw new System.NotImplementedException();
        }

        public void SetDeceleration(float deceleration)
        {
            throw new System.NotImplementedException();
        }

        public void SetMaxFallSpeed(float speed)
        {
            throw new System.NotImplementedException();
        }

        public void ClampVelocityY(float max)
        {
            throw new System.NotImplementedException();
        }

        public void SetVelocity(Vector3 velocity)
        {
            throw new System.NotImplementedException();
        }
    }
}