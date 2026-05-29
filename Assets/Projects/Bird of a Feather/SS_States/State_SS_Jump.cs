namespace StateMachineCore
{
    using UnityEngine;
    public class State_SS_Jump : AnimatedState<SidescrollerCharacterStateMachine>
    {

        public State_SS_Jump(string animBool, Animator animator, SidescrollerCharacterStateMachine stateMachine)
            : base(animBool, animator, stateMachine)
        {
            GameEvents.OnPlayerDied += () => stateMachine.ChangeState(stateMachine.deathState);
        }

        protected override void Enter(State previousState)
        {
            base.Enter(previousState);

            SS_PlayerData data = stateMachine.CharacterData.data;

            stateMachine.Jumpable.Jump(data.jumpForce);
            stateMachine.Movable.SetGravityScale(data.jumpGravityScale);
            stateMachine.Movable.SetDeceleration(data.airDeceleration);
            stateMachine.Movable.SetMaxFallSpeed(data.maxFallSpeed);
        }

        protected override void StateUpdate()
        {
            base.StateUpdate();
            IMovable2D movable = stateMachine.Movable;

            if (movable.Velocity.y < 0f ||
                InputHandler.Instance.JumpReleased)
            {
                stateMachine.ChangeState(stateMachine.fallState);
                return;
            }
            else if (InputHandler.Instance.DiveInput.InputActive)
            {
                stateMachine.ChangeState(stateMachine.diveState);
                return;
            }
        }

        protected override void StateFixedUpdate()
        {
            base.StateFixedUpdate();

            float moveDir = stateMachine.Controller.MoveInput.x;
            float targetX = moveDir * stateMachine.CharacterData.data.moveSpeed;
            float newX = Mathf.MoveTowards(stateMachine.Movable.Velocity.x, targetX, stateMachine.CharacterData.data.airAcceleration * Time.fixedDeltaTime);
            stateMachine.Movable.SetVelocityX(newX);
        }


        protected override void Exit() { base.Exit(); }
    }
}