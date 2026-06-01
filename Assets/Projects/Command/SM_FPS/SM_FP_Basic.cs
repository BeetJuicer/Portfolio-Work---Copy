// Basic — no shooting, maybe an AI controller later
using Assets.Projects.Command.SM_FPS;
using StateMachineCore;

class SM_FP_Basic : SM_FP_Base
{
    protected override IFPSController CreateController()
        => new FPS_PlayerController();

    protected override void InitStates()
    {
        // basic-specific setup only
    }
}