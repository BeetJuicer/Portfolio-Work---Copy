using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    SpriteRenderer sp;
    float timeStart;

    public Color colorA = Color.cyan;
    public Color colorB = Color.magenta;
    public float speed = 2f;

    void Start()
    {
        sp = GetComponent<SpriteRenderer>();
        timeStart = Time.time;
    }

    void Update()
    {
        float t = Mathf.PingPong((Time.time - timeStart) * speed, 1f);
        sp.color = Color.Lerp(colorA, colorB, t);
    }
}