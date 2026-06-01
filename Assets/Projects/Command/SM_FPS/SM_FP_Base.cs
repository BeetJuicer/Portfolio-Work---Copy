using CommandPattern.FPS;
using StateMachineCore;
using UnityEngine;

public abstract class SM_FP_Base : StateMachine
{
    public State_FP_Base_Idle idleState { get; private set; }
    public State_FP_Base_Move moveState { get; private set; }
    public State_FP_Base_Jump jumpState { get; private set; }

    public IMovable3D Movable { get; private set; }
    public IJumpable Jumpable { get; private set; }
    public Animator Animator { get; private set; }

    [SerializeField] private SO_FPS_PlayerData playerData;
    [SerializeField] private Animator handAnim;
    public SO_FPS_PlayerData PlayerData => playerData;
    public IFPSController Controller => controller;
    private IFPSController controller;
    public bool LookEnabled { get; private set; } = true;
    protected override void Start()
    {
        controller = CreateController();  // <-- hook for subclasses

        Animator = handAnim;
        Movable = GetComponent<IMovable3D>();
        Jumpable = GetComponent<IJumpable>();

        idleState = new State_FP_Base_Idle(this);
        moveState = new State_FP_Base_Move(this);
        jumpState = new State_FP_Base_Jump(this);

        Movable.SetMaxFallSpeed(playerData.maxFallSpeed);

        InitStates();  // <-- hook for subclass-specific states
        ChangeState(idleState);
        base.Start();
    }

    public void EnableLook() => LookEnabled = true;
    public void DisableLook() => LookEnabled = false;

    protected override void Update()
    {
        controller.Update();
        base.Update();
    }

    protected abstract IFPSController CreateController();
    protected virtual void InitStates() { }
}