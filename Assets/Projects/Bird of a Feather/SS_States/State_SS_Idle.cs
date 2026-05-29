namespace StateMachineCore
{
    using UnityEngine;
    public class State_SS_Idle : AnimatedState<SidescrollerCharacterStateMachine>
    {
        public State_SS_Idle(string animBool, Animator animator, SidescrollerCharacterStateMachine stateMachine)
            : base(animBool, animator, stateMachine) {

            GameEvents.OnPlayerDied += () => stateMachine.ChangeState(stateMachine.deathState);
        }

        protected override void Enter(State previousState)
        {
            base.Enter(previousState);
            stateMachine.Movable.SetVelocityX(0f);
            stateMachine.Movable.SetVelocityY(0f);
        }

        protected override void StateUpdate()
        {
            base.StateUpdate();

            if (stateMachine.Controller.JumpInput)
            {
                stateMachine.ChangeState(stateMachine.jumpState);
                return;
            }
            else if (stateMachine.Controller.MoveInput.x != 0f)
            {
                stateMachine.ChangeState(stateMachine.walkState);
                return;
            }
            else if (!stateMachine.Jumpable.IsGrounded())
            {
                stateMachine.ChangeState(stateMachine.fallState);
                return;
            }
        }

        protected override void Exit() { base.Exit(); }
    }
}