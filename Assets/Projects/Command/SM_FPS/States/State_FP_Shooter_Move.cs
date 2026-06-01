using System;
using System.Collections.Generic;
using System.Text;

namespace CommandPattern.FPS
{
    using CommandPattern.FPS;
    using StateMachineCore;
    using UnityEngine;

    class State_FP_Shooter_Move : AState_FPS_ShooterState
    {
        public State_FP_Shooter_Move(SM_FP_Spellslinger stateMachine) : base(stateMachine)
        {
        }

        protected override void Enter(State previousState)
        {
            base.Enter(previousState);
            stateMachine.Movable.SetDeceleration(stateMachine.PlayerData.deceleration);
        }

        protected override void StateUpdate()
        {
            base.StateUpdate();

            if (stateMachine.Controller.JumpInput)
            {
                stateMachine.ChangeState(stateMachine.jumpState);
                return;
            }
            else if (stateMachine.Controller.MoveInput == Vector2.zero)
            {
                stateMachine.ChangeState(stateMachine.idleState);
                return;
            }

        }

        protected override void StateFixedUpdate()
        {
            base.StateFixedUpdate();

            Vector2 moveDir = stateMachine.Controller.MoveInput;
            stateMachine.Movable.SetVelocityX(moveDir.x * stateMachine.PlayerData.walkSpeed);
            stateMachine.Movable.SetVelocityZ(moveDir.y * stateMachine.PlayerData.walkSpeed);
        }

        protected override void Exit()
        {
            base.Exit();
        }
    }
}
