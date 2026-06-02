using UnityEngine;

public class CursorSettings : MonoBehaviour
{
    [SerializeField] private CursorLockMode lockMode = CursorLockMode.None;
    [SerializeField] private bool visible = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState  = lockMode;
        Cursor.visible = visible;
    }
}
