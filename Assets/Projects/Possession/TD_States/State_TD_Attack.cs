namespace StateMachineCore
{
using UnityEngine;

public class State_TD_Attack : AnimatedState<TopDownCharacterStateMachine>
{
    private AttackData attackData;
    public State_TD_Attack(string animBool, AttackData attackData, Animator animator, TopDownCharacterStateMachine stateMachine) : base(animBool, animator, stateMachine)
    {   
        this.attackData = attackData;
    }

    protected override void Enter(State previousState)
    {
        base.Enter(previousState);

        InputHandler.Instance?.AttackInput.Consume();

        stateMachine.Attacker.SetAttackData(attackData);
        stateMachine.Attacker.StartAttack();
        stateMachine.Attacker.OnAttackComplete += ReturnToIdle;
    }

    protected override void StateUpdate()
    {
        base.StateUpdate();
    }

    protected override void Exit()
    {
        base.Exit();
        stateMachine.Attacker.OnAttackComplete -= ReturnToIdle;
    }
    private void ReturnToIdle() => stateMachine.ChangeState(stateMachine.idleState);
}

}