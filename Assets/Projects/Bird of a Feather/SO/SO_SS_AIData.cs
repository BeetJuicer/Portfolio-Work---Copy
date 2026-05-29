namespace StateMachineCore
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "SS_AIData", menuName = "Scriptable Objects/StateMachine/Sidescroller/AIData")]
    public class SO_SS_AIData : ScriptableObject
    {
        public SS_AIData data;
    }

    [System.Serializable]
    public class SS_AIData
    {
        [Header("Patrol")]
        public float patrolSpeed = 3f;
        public float patrolWaitTime = 1.5f;
        public float patrolDistance = 5f;

        [Header("Detection")]
        public float detectionRange = 6f;
        public float detectionHeight = 2f;
        public LayerMask whatIsPlayer;

        [Header("Jump")]
        public bool canJump = true;
        public float jumpDetectionRange = 1.5f;  // how close an obstacle needs to be before AI jumps
        public LayerMask whatIsObstacle;

        [Header("Edge Detection")]
        public bool stopAtEdges = true;
        public float edgeCheckDistance = 0.5f;   // raycast down distance to detect ledge drop-offs
    }
}