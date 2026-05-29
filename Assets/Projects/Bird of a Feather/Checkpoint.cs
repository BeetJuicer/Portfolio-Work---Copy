using UnityEngine;

namespace StateMachineCore
{
    public class Checkpoint : MonoBehaviour
    {
        [SerializeField] private Transform spawnPoint;
        private void Start()
        {
            if(spawnPoint == null)
            {
                spawnPoint = transform;
                Debug.LogWarning("Spawn point not set.");
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
                GameEvents.CheckpointReached(new RespawnPoint { Position = spawnPoint.position });
        }
        
    }
}