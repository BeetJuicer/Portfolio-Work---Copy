using System;
using System.Collections.Generic;
using System.Text;

namespace CommandPattern.FPS
{
    using StateMachineCore;
    using UnityEngine;

    public class State_FP_Base_Idle : AState_FPS_ControllableState
    {
        public State_FP_Base_Idle(SM_FP_Base stateMachine) : base(stateMachine)
        {
        }

        protected override void Enter(State previousState)
        {
            base.Enter(previousState);
            stateMachine.Movable.StopMovement();
        }

        protected override void StateUpdate()
        {
            base.StateUpdate();

            if (stateMachine.Controller.JumpInput)
            {
                stateMachine.ChangeState(stateMachine.jumpState);
                return;
            }
            else if (stateMachine.Controller.MoveInput != Vector2.zero)
            {
                stateMachine.ChangeState(stateMachine.moveState);
                return;
            }

        }

        protected override void StateFixedUpdate()
        {
            base.StateFixedUpdate();
        }

        protected override void Exit()
        {
            base.Exit();
        }
    }
}
