using UnityEngine;
using Unity.Cinemachine;

public class CameraRoom : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayer;

    private static CinemachineCamera virtualCamera;
    private static CinemachineConfiner2D confiner;
    private Collider2D roomCollider;

    private void Awake()
    {
        roomCollider = GetComponent<Collider2D>();

        if (virtualCamera == null)
        {
            virtualCamera = GameObject.FindAnyObjectByType<CinemachineCamera>();
            if (virtualCamera != null)
                confiner = virtualCamera.GetComponent<CinemachineConfiner2D>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsPlayer(collision))
        {
            SwitchToThisRoom();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // If the player is inside/touching this room, but the camera is 
        // pointing elsewhere, pull it back here.
        if (IsPlayer(collision) && confiner.BoundingShape2D != roomCollider)
        {
            SwitchToThisRoom();
        }
    }

    private bool IsPlayer(Collider2D other)
    {
        return ((1 << other.gameObject.layer) & playerLayer) != 0;
    }

    private void SwitchToThisRoom()
    {
        if (confiner != null && confiner.BoundingShape2D != roomCollider)
        {
            confiner.BoundingShape2D = roomCollider;
            confiner.InvalidateBoundingShapeCache();
        }
    }
}