using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Spring : MonoBehaviour
{
    [SerializeField] private float turns = 4;
    [SerializeField] private float height = 3;
    [SerializeField] private float minorRadius = 3;
    [SerializeField] private float majorRadius = 10;
    [SerializeField] private int pointsCountPerTurn = 8;

    private float TAU = Mathf.PI * 2;
    List<Vector3> points = new();
    List<Vector3> points2 = new();
    List<Vector3> centerPoints = new();
    List<Color> colors = new();

    [Header("Colors")]

    [SerializeField] private Color startColor;
    [SerializeField] private Color endColor;

    [SerializeField] private Vector3 offset;
    [SerializeField] private int targetPoint;

#if UNITY_EDITOR
    private void OnValidate()
    {
        GetPointsAndColors(pointsCountPerTurn, minorRadius, majorRadius, height, turns, points, colors, centerPoints, offset);
    }
#endif

    private void GetPointsAndColors(int pointsCountPerTurn, float radius, float majorRadius, float height, float turns, in List<Vector3> points, in List<Color> colors, in List<Vector3> centerPoints, Vector3 directionOffset)
    {
        points.Clear();
        colors.Clear();
        centerPoints.Clear();

        int totalPoints = Mathf.CeilToInt((float)pointsCountPerTurn * turns);
             
        for (int i = 0; i < totalPoints; i++)
        {
            float t = (float)i / totalPoints;

            points.Add(GetSpringPoint(t));
            colors.Add(Color.Lerp(startColor, endColor, t));
        }
    }

    Vector3 GetSpringPoint(float t)
    {
        float coilAngle = t * TAU * turns;
        Vector2 xyVec = AngToDir(coilAngle) * minorRadius;

        Vector3 pDir = AngToDir(t * TAU);
        pDir = new(pDir.x, pDir.z, pDir.y);
        Vector3 localUp = Vector3.up; 
        Vector3 p = pDir * majorRadius;

        return p + xyVec.x * pDir + xyVec.y * localUp;

        //return new Vector3(xzVec.x, t * height, xzVec.y); //circle mode
    }

    private void OnDrawGizmos()
    {
        Handles.matrix = Gizmos.matrix = transform.localToWorldMatrix;
        Handles.DrawAAPolyLine(10f, colors.ToArray(), points.ToArray());
        //Handles.DrawAAPolyLine(10f, centerPoints.ToArray());

        Handles.color = Color.white;
        //Gizmos.DrawSphere(centerPoints[targetPoint], 0.1f);
    }

    Vector2 AngToDir(float angle) => new (Mathf.Cos(angle), Mathf.Sin(angle));
}
