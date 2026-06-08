namespace StateMachineCore
{
    using UnityEngine;
    using UnityEngine.InputSystem;

    [RequireComponent(typeof(PlayerInput))]
    public class InputHandler : MonoBehaviour
    {
        public static InputHandler Instance { get; private set; }

        public BufferedInput RollInput { get; private set; }
        public BufferedInput AttackInput { get; private set; }
        public BufferedInput JumpInput { get; private set; }
        public BufferedInput DiveInput { get; private set; }
        public BufferedInput CrouchInput { get; private set; }
        public bool JumpHeld { get; private set; }
        public bool JumpReleased { get; private set; }
        public bool DiveReleased { get; private set; }
        public bool CrouchHeld { get; private set; }
        public bool CrouchReleased { get; private set; }
        public bool TimeReversalHeld { get; private set; }

        public Vector2 ActiveMoveInput { get; private set; } = Vector2.zero;
        public Vector2 BufferedMoveInput { get; private set; } = Vector2.zero;
        public Vector2 LookInput { get; private set; } = Vector2.zero;
        private float moveInputBufferTime = 0.2f;
        private float lastMoveTime = 0;

        [Header("Buffer Times")]
        private float attackInputBuffer = 0.2f;
        private float rollInputBuffer = 0.2f;
        private float jumpInputBuffer = 0.2f;
        private float diveInputBuffer = 0.2f;
        private float crouchInputBuffer = 0.2f;


        private InputAction moveAction;
        private InputAction lookAction;
        private InputAction jumpAction;
        private InputAction diveAction;
        private InputAction crouchAction;
        private InputAction timeReversalAction;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;

            RollInput = new BufferedInput(rollInputBuffer);
            AttackInput = new BufferedInput(attackInputBuffer);
            JumpInput = new BufferedInput(jumpInputBuffer);
            DiveInput = new BufferedInput(diveInputBuffer);
            CrouchInput = new BufferedInput(crouchInputBuffer);

            PlayerInput playerInput = GetComponent<PlayerInput>();
            InputActionMap map = playerInput.actions.FindActionMap("Player", throwIfNotFound: true);

            moveAction = map.FindAction("Move", throwIfNotFound: true);
            jumpAction = map.FindAction("Jump", throwIfNotFound: true);
            diveAction = map.FindAction("Dive", throwIfNotFound: true);
            crouchAction = map.FindAction("Crouch", throwIfNotFound: true);
            lookAction = map.FindAction("Look", throwIfNotFound: true);
            timeReversalAction = map.FindAction("Time Reversal", throwIfNotFound: true);

            map.FindAction("Roll", throwIfNotFound: true).performed += _ => RollInput.Press();
            map.FindAction("Attack", throwIfNotFound: true).performed += _ => AttackInput.Press();
            jumpAction.performed += _ => JumpInput.Press();
            diveAction.performed += _ => DiveInput.Press();
            crouchAction.performed += _ => CrouchInput.Press();
        }

        private void Update()
        {
            JumpHeld = jumpAction.IsPressed();
            JumpReleased = jumpAction.WasReleasedThisFrame();

            if(JumpReleased)
                Debug.Log("Jump released at time: " + Time.time);

            DiveReleased = diveAction.WasReleasedThisFrame();

            CrouchHeld = crouchAction.IsPressed();
            CrouchReleased = crouchAction.WasReleasedThisFrame();

            TimeReversalHeld = timeReversalAction.IsPressed();

            ActiveMoveInput = moveAction.ReadValue<Vector2>();
            LookInput = lookAction.ReadValue<Vector2>();

            if (ActiveMoveInput != Vector2.zero)
            {
                BufferedMoveInput = ActiveMoveInput;
                lastMoveTime = Time.time;
            }
            else if (Time.time > lastMoveTime + moveInputBufferTime)
            {
                BufferedMoveInput = Vector2.zero;
            }
        }
    }

    [System.Serializable]
    public class BufferedInput
    {
        public BufferedInput(float bufferTime)
        {
            this.bufferTime = bufferTime;
        }

        public bool InputActive => Time.time < lastInputTime + bufferTime;
        public bool ConsumeIfActive()
        {
            if(InputActive)
            {
                Consume();
                return true;
            }

            return false;
        }

        private float lastInputTime = float.NegativeInfinity;
        private float bufferTime;
        public void Press() => lastInputTime = Time.time;
        public void Consume() => lastInputTime = 0;
    }
}