using Assets.Projects.Command.SM_FPS;
using CommandPattern.FPS;
using StateMachineCore;
using UnityEngine;

public class SM_FP_Spellslinger : StateMachine
{
    public State_FP_Shooter_Idle idleState { get; private set; }
    public State_FP_Shooter_Move moveState { get; private set; }
    public State_FP_Shooter_Jump jumpState { get; private set; }

    public IShooter Shooter { get; private set; }
    public IMovable3D Movable { get; private set; }
    public IJumpable Jumpable { get; private set; }
    public Animator Animator { get; private set; }
    public IFPSController Controller { get; private set; }

    [SerializeField] private SO_FPS_PlayerData playerData;
    [SerializeField] private Animator handAnim;
    public SO_FPS_PlayerData PlayerData => playerData;

    protected override void Start()
    {
        Controller = new FPS_PlayerController();

        Animator = handAnim;
        Movable = GetComponent<IMovable3D>();
        Jumpable = GetComponent<IJumpable>();
        Shooter = GetComponentInChildren<IShooter>();

        Movable.SetMaxFallSpeed(playerData.maxFallSpeed);

        idleState = new State_FP_Shooter_Idle(this);
        moveState = new State_FP_Shooter_Move(this);
        jumpState = new State_FP_Shooter_Jump(this);

        ChangeState(idleState);
        base.Start();
    }

    protected override void Update()
    {
        Controller.Update();
        base.Update();
    }
}