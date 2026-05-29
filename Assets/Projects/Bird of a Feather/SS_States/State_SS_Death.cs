using StateMachineCore;
using System.Collections;
using UnityEngine;

namespace Assets.Projects.StateMachine.SideScroll.SS_States
{
	public class State_SS_Death: AnimatedState<SidescrollerCharacterStateMachine>
	{

        public State_SS_Death(string animBool, Animator animator, SidescrollerCharacterStateMachine stateMachine)
                   : base(animBool, animator, stateMachine) 
        {
            GameEvents.OnPlayerRespawned += () =>
            {
                stateMachine.ChangeState(stateMachine.idleState);
            };
        }

        protected override void Enter(State previousState)
        {
            base.Enter(previousState);
            stateMachine.Movable.StopMovement();
            stateMachine.Movable.SetGravityScale(0f);
        }

        protected override void StateUpdate()
        {
            base.StateUpdate();
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