using Assets.Projects.Command.SM_FPS;
using StateMachineCore;
using System;
using UnityEngine;

namespace CommandPattern.FPS
{
    class SM_FP_SpellSlinger : StateMachine
    {
        public State_FP_Idle idleState { get; private set; }
        public State_FP_Move moveState { get; private set; }
        public State_FP_Jump jumpState { get; private set; }
        //public State_FP_Move fallState { get; private set; }

        public IMovable3D Movable { get; private set; }
        public IJumpable Jumpable { get; private set; }
        public IShooter Shooter { get; private set; }
        public Animator Animator { get; private set; }

        [SerializeField] private SO_FPS_PlayerData playerData;
        [SerializeField] private Animator handAnim;
        public SO_FPS_PlayerData PlayerData => playerData;
        public  IFPSController Controller => controller;
        private IFPSController controller;

        protected override void Start()
        {
            controller = new FPS_PlayerController();

            Animator = handAnim;
            Movable = GetComponent<IMovable3D>();
            Jumpable = GetComponent<IJumpable>();
            Shooter = GetComponentInChildren<IShooter>();

            idleState = new State_FP_Idle(this);
            moveState = new State_FP_Move(this);
            jumpState = new State_FP_Jump(this);
            //fallState = new State_FP_Move("st_fall", Animator, this);

            Movable.SetMaxFallSpeed(playerData.maxFallSpeed);

            ChangeState(idleState);
            base.Start();
        }

        protected override void Update()
        {
            controller.Update();
            base.Update();
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
        }

    }
}
