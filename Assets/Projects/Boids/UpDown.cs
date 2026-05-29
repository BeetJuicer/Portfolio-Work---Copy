using UnityEngine;

public class UpDown : MonoBehaviour
{
    public float speed = 2f;
    public float distance = 1f;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float offset = Mathf.Sin(Time.time * speed) * distance;
        transform.position = startPos + transform.up * offset;
    }
}