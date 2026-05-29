using UnityEngine;
using System.Collections;
using System;
namespace StateMachineCore
{
    public static class GameEvents
    {
        public static event Action<RespawnPoint> OnCheckpointReached;
        public static event Action OnPlayerDied;
        public static event Action OnPlayerRespawned;

        public static void CheckpointReached(RespawnPoint p) => OnCheckpointReached?.Invoke(p);
        public static void PlayerDied() => OnPlayerDied?.Invoke();
        public static void PlayerRespawned() => OnPlayerRespawned?.Invoke();
    }
}