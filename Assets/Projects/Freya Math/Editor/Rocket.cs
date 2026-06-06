using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Rocket : MonoBehaviour
{
    [SerializeField] PlayerInput input;
    [SerializeField] float rotateSpeed;

    private Vector2 velocity;
    private Vector2 acceleration;

    // meters per second.
    private float unityUnitPerMeter = 0.001f;
    [SerializeField] private float rocketAccel = 2f;

    private float gravity = -9.8f;
    [SerializeField] private float drag = -2f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gravity *= unityUnitPerMeter;
        rocketAccel *= unityUnitPerMeter;
        drag *= unityUnitPerMeter;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 turnInput = input.actions["Move"].ReadValue<Vector2>();
        if(Mathf.Abs(turnInput.x) > 0)
        {
            float xDir = Mathf.Sign(turnInput.x);
            Quaternion newRot = Quaternion.LookRotation(transform.forward, transform.right * xDir);
            transform.rotation = Quaternion.Lerp(transform.rotation, newRot, rotateSpeed * Time.deltaTime);
        }

        if (transform.position.y > -5f)
            velocity.y += gravity * Time.deltaTime;

        if (Mathf.Abs(velocity.x) > 0)
        {
            print("Velocity.x: " + velocity.x + ", drag: " + Mathf.Sign(velocity.x) * drag);
            velocity.x += Mathf.Sign(velocity.x) * drag * Time.deltaTime;
        }

            bool jumpInput = input.actions["Jump"].IsPressed();

        if(jumpInput)
        {
            velocity += (Vector2)transform.up * rocketAccel * Time.deltaTime;
        }

        transform.Translate(velocity, Space.World);
    }
}
