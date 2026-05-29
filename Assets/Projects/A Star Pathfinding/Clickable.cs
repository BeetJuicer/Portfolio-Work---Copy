using UnityEngine;
using System;

public class Clickable : MonoBehaviour
{
    public event Action OnClicked;

    void OnMouseDown()
    {
        OnClicked?.Invoke();
    }
}