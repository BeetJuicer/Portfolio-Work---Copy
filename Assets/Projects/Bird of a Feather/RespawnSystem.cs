using System.Collections;
using UnityEngine;

namespace StateMachineCore
{ 
    public class RespawnSystem : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float waitDuration;

    private void OnEnable() => GameEvents.OnPlayerDied += StartRespawnSequence;
    private void OnDisable() => GameEvents.OnPlayerDied -= StartRespawnSequence;

    private void StartRespawnSequence()
    {
        StartCoroutine(RespawnSequence());
    }

    private void Respawn()
    {
        RespawnPoint p = CheckpointManager.Instance.Current;
        player.SetActive(false);
        player.transform.SetPositionAndRotation(p.Position, p.Rotation);
        player.SetActive(true);

        GameEvents.PlayerRespawned();
    }

        private IEnumerator RespawnSequence()
        {
            yield return new WaitForSeconds(waitDuration);
            Respawn();
        }
    }
}