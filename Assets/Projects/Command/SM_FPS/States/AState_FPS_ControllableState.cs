using CommandPattern.FPS;
using StateMachineCore;
using UnityEngine;

namespace CommandPattern
{
    abstract class AState_FPS_ControllableState : BaseState<SM_FP_SpellSlinger>
    {
        protected AState_FPS_ControllableState(SM_FP_SpellSlinger stateMachine) : base(stateMachine)
        {
        }
        bool hasFired;
        protected override void Enter(State previousState) {
            stateMachine.Shooter.SetMaxRayDistance(stateMachine.PlayerData.shootDistance);
        }
        protected override void Exit() { }
        protected override void StateUpdate() {
            stateMachine.Movable.SetVerticalLookRange(stateMachine.PlayerData.minLookAngle, stateMachine.PlayerData.maxLookAngle);
            stateMachine.Movable.LookAt(stateMachine.Controller.LookDelta * stateMachine.PlayerData.mouseSensitivity);

            if (stateMachine.Controller.ShootInput && !hasFired)
            {
                hasFired = true;
                stateMachine.Shooter.Shoot(stateMachine.PlayerData.explosionPrefab);
                stateMachine.Animator.SetTrigger("fire");
                stateMachine.Animator.SetBool("isHolding", false);
            }
            
            if (!stateMachine.Controller.ShootInput)
            {
                hasFired = false; // reset when button released
            }
            
            if(stateMachine.Controller.SkillInput)
            {
                TimeManager.Instance.StartReversing();
                Debug.Log("isHolding..");
                stateMachine.Animator.SetBool("isHolding", true);
            }
            
            if(!stateMachine.Controller.SkillInput)
            {
                Debug.Log(" not isHolding..");
                TimeManager.Instance.StopReversing();
                stateMachine.Animator.SetBool("isHolding", false);
            }
        }
        protected override void StateFixedUpdate() { }
    }
}
 