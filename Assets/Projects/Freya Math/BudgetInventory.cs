using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using Unity.Mathematics;

public class BudgetInventory : MonoBehaviour
{
    [SerializeField] private List<float> radii;
    [SerializeField] private Transform controller;
    [SerializeField] private float bigRadius;
    
    private double tau = Mathf.PI * 2;
    [SerializeField] private float radsTest;
    [Range(0, 6.28f)]
    [SerializeField] private float maxAngleRads;
    int currentIndex = 0;

    private void OnDrawGizmos()
    {
        currentIndex = Mathf.Min(currentIndex, radii.Count);
        currentIndex = Mathf.Max(currentIndex, 0);

        Gizmos.matrix = Handles.matrix = transform.localToWorldMatrix;

        Vector3 circleOrigin = Vector3.zero;
        //Handles.color = Color.blue;
        //Handles.DrawWireDisc(circleOrigin, Vector3.up, bigRadius);


        Handles.color = Color.black;
        float prevRadianPosition = 0f;
        float prevRadius = 0f;

        float totalDistForAllItems = 0f;
        foreach (var r in radii)
        {
            totalDistForAllItems += r * 2;
        }

        // We want the items to start displaying before the center point, so we offset the start position by half of the total distance of all items.
        float radianForAllItems = GetRadianEquivalent(bigRadius, totalDistForAllItems);
        float halfTotalRadForItems = radianForAllItems / 2;

        Vector3 startPoint = GetPointOnCircle(circleOrigin, bigRadius, -halfTotalRadForItems);
        Vector3 endPoint = GetPointOnCircle(circleOrigin, bigRadius, halfTotalRadForItems);

        Gizmos.DrawLine(circleOrigin, startPoint);
        Gizmos.DrawLine(circleOrigin, endPoint);
        for (int i = 0; i < radii.Count; i++)
        {
            float radianPos = GetRadianEquivalent(bigRadius, radii[i]) + GetRadianEquivalent(bigRadius, prevRadius);
            Vector3 point = GetPointOnCircle(circleOrigin, bigRadius, (prevRadianPosition + radianPos) - halfTotalRadForItems);
            
            Handles.DrawWireDisc(point, Vector3.up, radii[i]);

            prevRadius = radii[i];
            prevRadianPosition += radianPos;
        }

    }

    private Vector3 GetPointOnCircle(Vector3 origin, float radius, float angleRads)
    {
        return (new Vector3(Mathf.Sin(angleRads), 0f, Mathf.Cos(angleRads)) * radius);// + origin;
    }

    private float GetRadianEquivalent(float radius, float dist)
    {
        float totalDistance = radius * (float)tau;
        float percentage = (dist / totalDistance);

        return percentage * (float)tau;
    }
}
