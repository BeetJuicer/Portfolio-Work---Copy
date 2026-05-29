namespace StateMachineCore
{
    using UnityEngine;

    public interface IController
    {
        Vector2 MoveInput { get; }
        public void Update();
    }

    public interface ITopDownController : IController
    {
        bool RollInput { get; }
        bool AttackInput { get; }

    }

    public interface ISideScrollerController : IController
    {
        bool JumpInput { get; }
        bool CrouchInput { get; }
    }

    public interface IFPSController : IController
    {
        bool JumpInput { get; }
        bool CrouchInput { get; }
        bool ShootInput { get; }
        Vector2 LookDelta { get; }
        bool SkillInput { get; }
    }
}