namespace StateMachineCore
{
    using UnityEngine;
    public class State_SS_Fall : AnimatedState<SidescrollerCharacterStateMachine>
    {
        public State_SS_Fall(string animBool, Animator animator, SidescrollerCharacterStateMachine stateMachine)
            : base(animBool, animator, stateMachine) {
            GameEvents.OnPlayerDied += () => stateMachine.ChangeState(stateMachine.deathState); 
        }

        protected override void Enter(State previousState)
        {
            base.Enter(previousState);
            stateMachine.Movable.SetGravityScale(stateMachine.CharacterData.data.fallGravityScale);
            stateMachine.Movable.SetDeceleration(4f);

            GameEvents.OnPlayerDied += () => stateMachine.ChangeState(stateMachine.deathState);

        }

        protected override void StateUpdate()
        {
            base.StateUpdate();

            if (stateMachine.Jumpable.IsGrounded())
            {
                stateMachine.ChangeState(stateMachine.idleState);
                return;
            }
            else if (InputHandler.Instance.JumpHeld)
            {
                stateMachine.ChangeState(stateMachine.glideState);
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
            
            //only affect velocity with air control if the player explicitly inputs.
            //if not, keep previosu vel
            if (Mathf.Abs(moveDir) > 0)
            {
                float targetX = moveDir * stateMachine.CharacterData.data.moveSpeed;
                float newX = Mathf.MoveTowards(stateMachine.Movable.Velocity.x, targetX, stateMachine.CharacterData.data.airAcceleration * Time.fixedDeltaTime);
                stateMachine.Movable.SetVelocityX(newX);
            }
        }

        protected override void Exit() 
        { 
            base.Exit(); 
        }
    }
}