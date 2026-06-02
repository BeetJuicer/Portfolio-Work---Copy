namespace CommandPattern.FPS
{
    using StateMachineCore;
    using UnityEngine;

    public class State_FP_Shooter_Jump : AState_FPS_ShooterState
    {
        public State_FP_Shooter_Jump(SM_FP_Spellslinger stateMachine) : base(stateMachine) { }

        protected override void Enter(State previousState)
        {
            base.Enter(previousState);
            stateMachine.Movable.SetDeceleration(stateMachine.PlayerData.deceleration);
            stateMachine.Jumpable.Jump(stateMachine.PlayerData.jumpForce);
        }

        protected override void StateUpdate()
        {
            base.StateUpdate();

            if (stateMachine.Jumpable.IsGrounded())
            {
                stateMachine.ChangeState(stateMachine.idleState);
                return;
            }
        }

        protected override void StateFixedUpdate()
        {
            base.StateFixedUpdate();

            Vector2 moveDir = stateMachine.Controller.MoveInput;
            stateMachine.Movable.SetVelocityX(moveDir.x * stateMachine.PlayerData.airControlSpeed);
            stateMachine.Movable.SetVelocityZ(moveDir.y * stateMachine.PlayerData.airControlSpeed);
        }
    }
}