namespace StateMachineCore
{
using UnityEngine;

public class SM_TD_AI : StateMachine
{

    public IMovable2D Movable { get; private set; }
    public IAttacker Attacker { get; private set; }
    public Animator Animator { get; private set; }

    //[SerializeField] private SO_AttackData attackData; // change when moving to multiple attacks
    //[SerializeField] private SO_TD_PlayerData playerData;
    //public SO_TD_PlayerData PlayerData => playerData;


    // ----- STATES
    public State_AI_Idle idleState { get; private set; }
    public State_AI_Patrol patrolState { get; private set; }
    //public State_TD_Moving moveState { get; private set; }
    //public State_TD_Attack attackState { get; private set; }
    //public State_TD_Roll rollState { get; private set; }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        Animator = GetComponentInChildren<Animator>();
        Attacker = GetComponentInChildren<IAttacker>();

        Movable = GetComponent<IMovable2D>();

        idleState = new State_AI_Idle("st_idle", Animator, this);
        //patrolState = new State_AI_Patrol("st_patrol", Animator, this);
        //moveState = new State_TD_Moving("st_move", Animator, this);
        //attackState = new State_TD_Attack("st_attack", attackData.data, Animator, this);
        //rollState = new State_TD_Roll("st_roll", Animator, this);

        ChangeState(idleState);
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}

}