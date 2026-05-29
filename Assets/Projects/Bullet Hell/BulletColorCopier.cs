using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BulletColorCopier : MonoBehaviour
{
    Light2D myLight;
    Bullet bullet;
    SpriteRenderer spriteRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myLight = GetComponent<Light2D>();
        spriteRenderer = transform.parent.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        myLight.color = spriteRenderer.color; //for realtime 
    }
}
