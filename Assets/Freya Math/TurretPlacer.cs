using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class TurretPlacer : MonoBehaviour
{
    [SerializeField] private GameObject turret;
    [SerializeField] private float maxDistance;
    [SerializeField] private LayerMask whatIsGround;

    [SerializeField] private InputActionAsset inputActions;
    [SerializeField] private float sensitivity;

    [SerializeField] private float rotationAmount;
    
    float yaw = 0f;
    float pitch = 0f;

    float turretYawOffset = 0f;

    private void Start()
    {
    }


    private void Update()
    {
        Vector2 lookInput = inputActions.FindAction("Look").ReadValue<Vector2>();


        yaw += lookInput.x * sensitivity;
        pitch -= lookInput.y * sensitivity;
        pitch = Mathf.Clamp(pitch, -90, 90);

        transform.rotation  = Quaternion.Euler(pitch, yaw, 0);

        Ray ray = new Ray(transform.position, transform.forward);
        if(Physics.Raycast(ray, out RaycastHit hitInfo, maxDistance, whatIsGround))
        {
            Vector3 yAxis = hitInfo.normal;
            Vector3 zAxis = Vector3.Cross(transform.right, yAxis).normalized;

            turret.transform.position = hitInfo.point;
            turret.transform.rotation = Quaternion.LookRotation(zAxis, yAxis);
            turret.transform.rotation *= Quaternion.Euler(0, turretYawOffset, 0);
        }

        Vector2 scroll = inputActions.FindAction("Scroll").ReadValue<Vector2>();
        float scrollY = scroll.y;

        if (Mathf.Abs(scrollY) > 0.01f)
        {
            print("attempting to rotate. Sign(scrollY): " + Mathf.Sign(scrollY));
            turretYawOffset += rotationAmount * Mathf.Sign(scrollY);
        }
    }

    private void OnDrawGizmos()
    {
        Ray ray = new Ray(transform.position, transform.forward);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(ray);
    }
}
