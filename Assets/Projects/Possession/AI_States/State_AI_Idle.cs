namespace StateMachineCore
{
using UnityEngine;

public class State_AI_Idle : AnimatedState<SM_TD_AI>
{
    private float idleDuration = 0.5f;
    private float endTime;

    public State_AI_Idle(string animBool, Animator animator, SM_TD_AI stateMachine) : base(animBool, animator, stateMachine)
    {
    }

    protected override void Enter(State previousState)
    {
        base.Enter(previousState);

        endTime = Time.time + idleDuration;
    }

    protected override void StateUpdate()
    {
        base.StateUpdate();

        if (Time.time > endTime)
        {
            stateMachine.ChangeState(stateMachine.patrolState);
        }
    }
}

}