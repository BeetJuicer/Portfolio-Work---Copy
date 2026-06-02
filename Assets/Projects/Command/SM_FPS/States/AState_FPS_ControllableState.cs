using CommandPattern.FPS;
using StateMachineCore;
using UnityEngine;

namespace CommandPattern
{
    public abstract class AState_FPS_ControllableState : BaseState<SM_FP_Base>
    {
        protected AState_FPS_ControllableState(SM_FP_Base stateMachine) : base(stateMachine) { }

        protected override void Enter(State previousState) { }
        protected override void Exit() { }

        protected override void StateUpdate()
        {
            if (!stateMachine.LookEnabled)
                return;

            stateMachine.Movable.SetVerticalLookRange(
                stateMachine.PlayerData.minLookAngle,
                stateMachine.PlayerData.maxLookAngle);

            stateMachine.Movable.LookAt(
                stateMachine.Controller.LookDelta * stateMachine.PlayerData.mouseSensitivity);

        }

        protected override void StateFixedUpdate() { }
    }
}