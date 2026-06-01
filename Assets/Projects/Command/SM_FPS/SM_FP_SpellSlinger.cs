// Spellslinger — has a Shooter, uses player controller
using Assets.Projects.Command.SM_FPS;
using StateMachineCore;

class SM_FP_Spellslinger : SM_FP_Base
{
    public IShooter Shooter { get; private set; }

    protected override IFPSController CreateController()
        => new FPS_PlayerController();

    protected override void InitStates()
    {
        Shooter = GetComponentInChildren<IShooter>();
        // add spellslinger-specific states here
    }
}