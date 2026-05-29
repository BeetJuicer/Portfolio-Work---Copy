namespace StateMachineCore
{
    using UnityEngine;
    public abstract class State
    {
        public void EnterState(State previousState) => Enter(previousState);
        public void UpdateState() => StateUpdate();
        public void FixedUpdateState() => StateFixedUpdate();
        public void ExitState() => Exit();

        protected abstract void Enter(State previousState);
        protected abstract void StateUpdate();
        protected abstract void StateFixedUpdate();
        protected abstract void Exit();
    }

    public abstract class BaseState<T> : State where T: StateMachine
    {
        protected T stateMachine;

        protected BaseState(T stateMachine)
        {
            this.stateMachine = stateMachine;
        }
        protected override void Enter(State previousState) { }
        protected override void Exit() { }
        protected override void StateUpdate() { }
        protected override void StateFixedUpdate() { }
    }

    public abstract class AnimatedState<T> : BaseState<T> where T : StateMachine
    {
        private string animationBoolean;
        private Animator animator;

        public AnimatedState(string animBool, Animator animator, T stateMachine) : base(stateMachine)
        {
           this.animationBoolean = animBool;
           this.animator = animator;
        }

        protected override void Enter(State previousState)
        {
            base.Enter(previousState);
            animator.SetBool(animationBoolean, true);
        }

        protected override void Exit()
        {
            base.Exit();
            animator.SetBool(animationBoolean, false);
        }
    }
}

