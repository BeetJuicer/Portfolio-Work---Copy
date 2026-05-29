namespace StateMachineCore
{
    using UnityEngine;

    public abstract class StateMachine : MonoBehaviour 
    {
        public State currentState { get; private set; }

        protected virtual void Start()
        {
            currentState?.EnterState(null);
        }

        protected virtual void Update()
        {
            currentState?.UpdateState();
        }

        protected virtual void FixedUpdate()
        {
            currentState?.FixedUpdateState();
        }

        public virtual void ChangeState(State state)
        {
            currentState?.ExitState();
            State previous = currentState;  

            currentState = state;
            currentState?.EnterState(previous);
        }
    }
}