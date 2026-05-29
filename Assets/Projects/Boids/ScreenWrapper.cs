using UnityEngine;

public class ScreenWrapper : MonoBehaviour
{
    private Camera cam;
    private float screenHalfWidth;
    private float screenHalfHeight;

    void Start()
    {
        cam = Camera.main;
        screenHalfHeight = cam.orthographicSize;
        screenHalfWidth = screenHalfHeight * cam.aspect;
    }

    void Update()
    {
        Vector3 pos = transform.position;

        if (pos.x > screenHalfWidth) pos.x = -screenHalfWidth;
        else if (pos.x < -screenHalfWidth) pos.x = screenHalfWidth;

        if (pos.y > screenHalfHeight) pos.y = -screenHalfHeight;
        else if (pos.y < -screenHalfHeight) pos.y = screenHalfHeight;

        transform.position = pos;
    }
}