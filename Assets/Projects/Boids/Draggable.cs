using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider2D))]
public class Draggable : MonoBehaviour
{
    private bool isDragging;
    private Vector2 offset;
    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void OnMouseDown()
    {
        print("Mouse!");
        Vector2 mouseWorld = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        offset = (Vector2)transform.position - mouseWorld;
        isDragging = true;
    }

    private void OnMouseUp()
    {
        isDragging = false;
    }

    private void Update()
    {
        if (!isDragging) return;
        Vector2 mouseWorld = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        transform.position = mouseWorld + offset;
    }
}