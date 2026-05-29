namespace StateMachineCore
{
    using UnityEngine;
    public class State_SS_Move : AnimatedState<SidescrollerCharacterStateMachine>
    {
        public State_SS_Move(string animBool, Animator animator, SidescrollerCharacterStateMachine stateMachine)
            : base(animBool, animator, stateMachine) { 
            GameEvents.OnPlayerDied += () => stateMachine.ChangeState(stateMachine.deathState);
        }

        protected override void Enter(State previousState)
        {
            base.Enter(previousState);
            stateMachine.Movable.SetDeceleration(stateMachine.CharacterData.data.groundDeceleration);
        }

        protected override void StateUpdate()
        {
            base.StateUpdate();

            if (stateMachine.Controller.JumpInput)
            {
                stateMachine.ChangeState(stateMachine.jumpState);
                //stateMachine.Movable.
                return;
            }
            else if (stateMachine.Controller.MoveInput.x == 0f)
            {
                stateMachine.ChangeState(stateMachine.idleState);
                return;
            }

        }

        protected override void StateFixedUpdate()
        {
            base.StateFixedUpdate();
            
            float moveDir = stateMachine.Controller.MoveInput.x;
            stateMachine.Movable.SetVelocityX(moveDir * stateMachine.CharacterData.data.moveSpeed);
        }

        protected override void Exit()
        {
            base.Exit();
        }
    }
}