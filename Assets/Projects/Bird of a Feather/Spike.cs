using Assets.Projects.StateMachine;
using UnityEngine;

namespace StateMachineCore
{

public class Spike : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            GameEvents.PlayerDied();
        }
    }
}
}
