using System;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class HealthbarF : MonoBehaviour
{
    [SerializeField] private float maxLength = 5f;

    [Range(0f, 100f)]
    [SerializeField] private float currentHealth = 50f;
    [SerializeField] private float maxHealth = 100f;

    [Range(0f, 100f)]
    [SerializeField] private float safeRange = 80f;
    [Range(0f, 100f)]
    [SerializeField] private float dangerRange = 20f;

    private void OnDrawGizmos()
    {
        Gizmos.matrix = Handles.matrix = transform.localToWorldMatrix;
        float currentLength = math.remap(0, maxHealth, 0, maxLength, (currentHealth / maxHealth));
        float colorT = Mathf.InverseLerp(dangerRange, safeRange, currentHealth);
        Handles.color = Color.Lerp(Color.red, Color.green, colorT);

        Handles.DrawLine(default, Vector3.right * currentLength, 10f);
    }
}
