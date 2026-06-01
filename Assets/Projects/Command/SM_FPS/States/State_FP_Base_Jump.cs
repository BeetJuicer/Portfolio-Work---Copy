using System;
using System.Collections.Generic;
using System.Text;

namespace CommandPattern.FPS
{
    using CommandPattern.FPS;
    using StateMachineCore;
    using UnityEngine;

    public class State_FP_Base_Jump : AState_FPS_ControllableState
    {
        public State_FP_Base_Jump(SM_FP_Base stateMachine) : base(stateMachine)
        {
        }

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

        protected override void Exit()
        {
            base.Exit();
        }
    }
}
