using StateMachineCore;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Assets.Projects.Command.SM_FPS
{
    class FPS_PlayerController : IFPSController
    {
        public bool JumpInput => InputHandler.Instance.JumpInput.ConsumeIfActive();
        public bool ShootInput => InputHandler.Instance.AttackInput.ConsumeIfActive();
        public bool HoldSkillInput => InputHandler.Instance.AttackInput.ConsumeIfActive();

        public Vector2 MoveInput => InputHandler.Instance.ActiveMoveInput;

        public bool CrouchInput => throw new NotImplementedException();

        public bool SkillInput => InputHandler.Instance.TimeReversalHeld;


        public Vector2 LookDelta => InputHandler.Instance.LookInput;

        public void Update()
        {
        }
    }
}
