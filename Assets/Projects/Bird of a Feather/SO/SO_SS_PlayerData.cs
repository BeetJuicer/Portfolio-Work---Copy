namespace StateMachineCore
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "SS_PlayerData", menuName = "Scriptable Objects/StateMachine/Sidescroller/PlayerData")]
    public class SO_SS_PlayerData : ScriptableObject
    {
        public SS_PlayerData data;
    }

    [System.Serializable]
    public class SS_PlayerData
    {
        [Header("Movement")]
        public float moveSpeed = 8f;
        public float groundAcceleration = 12f;
        public float airAcceleration = 5f;
        public float groundDeceleration = 20f;
        public float airDeceleration = 2f;

        [Header("Jump")]
        public float jumpForce = 16f;
        public float jumpCutMultiplier = 0.5f;
        public float coyoteTime = 0.12f;
        public float jumpBufferTime = 0.1f;
        public float jumpGravityScale = 3f;

        [Header("Fall")]
        public float maxFallSpeed = 20f;
        public float fallGravityScale = 6f;

        public GlideStateData glideStateData;

        [Header("Wind")]
        public float windRideMaxSpeed = 20f;
        public float windRideAcceleration = 5f;
        public Vector2 windDetectionSize;
        public LayerMask whatIsWind;

        [Header("Dive")]
        public float diveSpeed = 20f;
        public float diveAirControlSpeed = 2f;
    }
}