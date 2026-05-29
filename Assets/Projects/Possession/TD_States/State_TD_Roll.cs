namespace StateMachineCore
{
using UnityEngine;

public class State_TD_Roll : AnimatedState<TopDownCharacterStateMachine>
{
    RollStateData data;
    float endTime;
    Vector2 direction;
    public State_TD_Roll(string animBool, Animator animator, TopDownCharacterStateMachine stateMachine) : base(animBool, animator, stateMachine)
    {
        data = stateMachine.CharacterData.rollStateData;
    }

    protected override void Enter(State previousState)
    {
        base.Enter(previousState);

        // Because our character doesnt have a faceup or face down idle, it feels weird to suddenly roll downward when we're idly facing right.
        // Use face direction when coming from idle.
        if (InputHandler.Instance.BufferedMoveInput != Vector2.zero)
        {
            direction = InputHandler.Instance.BufferedMoveInput;
        }
        else
        {
            direction = new Vector2(stateMachine.Movable.FacingDirection, 0);
        }
            
        InputHandler.Instance?.RollInput.Consume();

        endTime = Time.time + data.duration;
    }

    protected override void StateUpdate()
    {
        base.StateUpdate();

        if(Time.time > endTime)
        {
            stateMachine.ChangeState(stateMachine.idleState);
            return;
        }
    }

    protected override void StateFixedUpdate()
    {
        base.StateFixedUpdate();

        Vector2 moveAmount = direction * Time.fixedDeltaTime * data.speed;
        stateMachine.Movable.Move(moveAmount);
    }

    protected override void Exit() { base.Exit(); }
}

}