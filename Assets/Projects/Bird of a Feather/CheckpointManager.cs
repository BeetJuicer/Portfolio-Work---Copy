using UnityEngine;
using System.Collections;

namespace StateMachineCore
{
    [System.Serializable]
    public struct RespawnPoint
    {
        public Vector3 Position;
        public Quaternion Rotation;
        public string SceneName;
        public int CheckpointID;
    }

    public class CheckpointManager : MonoBehaviour
    {
        private static CheckpointManager instance;
        public static CheckpointManager Instance => instance;
        public RespawnPoint Current { get; private set; }

        private void Awake()
        {
            if(Instance == null)
                instance = this;
            else
                Destroy(gameObject);
        }

        private void OnEnable() => GameEvents.OnCheckpointReached += OnCheckpointReached;
        private void OnDisable() => GameEvents.OnCheckpointReached -= OnCheckpointReached;

        private void OnCheckpointReached(RespawnPoint p) => Current = p;
    }

}