namespace StateMachineCore
{
using UnityEngine;
using System.Collections.Generic;
public class State_AI_Patrol : AnimatedState<TopDownCharacterStateMachine>
{
    private float minDuration = 0.5f;
    private float maxDuration = 1f;
    private float duration;
    private float patrolWalkSpeed = 7f;

    private float endTime;
    private Vector2 direction;

    public State_AI_Patrol(string animBool, Animator animator, TopDownCharacterStateMachine stateMachine) : base(animBool, animator, stateMachine)
    {
    }

    protected override void Enter(State previousState)
    {
        base.Enter(previousState);

        duration = Random.Range(minDuration, maxDuration);
        endTime = Time.time + duration;

        Vector2 lastDir = stateMachine.Movable.LastMoveDirection;
        direction = GetRandomDirExcluding(lastDir);
    }

    protected override void StateUpdate()
    {
        base.StateUpdate();

        if (stateMachine.Controller.MoveInput == Vector2.zero /* || vision.SeesObstacle(range)*/)
        {
            stateMachine.ChangeState(stateMachine.idleState);
        }
    }

    protected override void StateFixedUpdate()
    {
        base.StateFixedUpdate();

        Vector2 moveAmount = direction * patrolWalkSpeed * Time.fixedDeltaTime;
        stateMachine.Movable.Move(moveAmount);
    }

    private Vector2 GetRandomDirExcluding(Vector2 dirToExclude)
    {
        Vector2 topRight = new Vector2(1, 1);
        Vector2 topLeft = new Vector2(-1, 1);
        Vector2 bottomRight = new Vector2(1, -1);
        Vector2 bottomLeft = new Vector2(-1, -1);

        List<Vector2> directions = new()
        {
            Vector2.right, Vector2.left, Vector2.up, Vector2.down,
            topLeft, topRight, bottomLeft, bottomRight
        };

        directions.Remove(dirToExclude);
        return directions[Random.Range(0, directions.Count)];
    }
}

}