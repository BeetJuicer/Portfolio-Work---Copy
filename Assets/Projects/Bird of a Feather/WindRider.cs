using StateMachineCore;
using UnityEngine;

public class WindRider : MonoBehaviour
{
    [SerializeField] private bool ride;

    private Wind currentWind;
    private IMovable2D movable;

    private void Awake()
    {
        TryGetComponent<IMovable2D>(out movable);
    }

    private void Update()
    {
        if (currentWind != null && !ride)
        {
            currentWind.ExitWind(movable);
            currentWind = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!ride) return;
        if (collision.TryGetComponent<Wind>(out Wind wind) && movable != null)
        {
            currentWind = wind;
            currentWind.RideWind(movable, this);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Wind>(out Wind wind) && movable != null)
        {
            wind.ExitWind(movable);
            if (currentWind == wind) currentWind = null;
        }
    }
}