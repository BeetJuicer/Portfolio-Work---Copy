using System;
using UnityEngine;

public class Spline : MonoBehaviour
{
    [SerializeField] private GameObject point1;
    [SerializeField] private GameObject point2;
    [SerializeField] private GameObject point3;
    [SerializeField] private GameObject point4;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnDrawGizmos()
    {
        Vector3 point1Pos = point1.transform.position;
        Vector3 point2Pos = point2.transform.position;
        Vector3 point3Pos = point3.transform.position;
        Vector3 point4Pos = point4.transform.position;

        int points = 100;

        Vector3[] line1 = new Vector3[points];
        Vector3[]  line2 = new  Vector3[points];
        Vector3[]  line3 = new Vector3[points];
        Vector3[]  curve1 = new Vector3[points];
        Vector3[]  curve2 = new Vector3[points];

        Vector3[]  quadraticCurve = new Vector3[points];

        for (int i = 0; i < points; i+=1)
        {
            float t = (float)i / points;

            line1[i] = Vector3.Lerp(point1Pos, point2Pos, t);
            line2[i] = Vector3.Lerp(point2Pos, point3Pos, t);
            curve1[i] = Vector3.Lerp(line1[i], line2[i], t);

            line3[i] = Vector3.Lerp(point3Pos, point4Pos, t);
            curve2[i] = Vector3.Lerp(line2[i], line3[i], t);

            quadraticCurve[i] = Vector3.Lerp(curve1[i], curve2[i], t);
        }

        Gizmos.color = Color.white;
        Gizmos.DrawLineStrip(line1, false);
        //Gizmos.DrawLineStrip(line2, false);
        Gizmos.DrawLineStrip(line3, false);

        //Gizmos.color = Color.red;
        //Gizmos.DrawLineStrip(curve1, false);
        //Gizmos.DrawLineStrip(curve2, false);

        Gizmos.color = Color.green;
        Gizmos.DrawLineStrip(quadraticCurve, false);
    }

    private Vector3[] GetLineAsPoints(Vector3 p1, Vector3 p2, int pointCount)
    {
        Vector3[] points = new Vector3[pointCount];
        for (int i = 0; i < pointCount; i++)
        {
            float t = i / pointCount;
            points[i] = Vector3.Lerp(p1, p2, t);
        }

        return points;
    }

    private Vector3[] GetQuadraticCurveAsPoints(Vector3 p1, Vector3 p2, Vector3 p3, int pointCount)
    {
        Vector3[] line1 = GetLineAsPoints(p1, p2, pointCount);
        Vector3[] line2 = GetLineAsPoints(p2, p3, pointCount);


        Vector3[] points = new Vector3[pointCount];
        for (int i = 0; i < pointCount; i++)
        {
            float t = i / pointCount;
            points[i] = Vector3.Lerp(line1[i], line2[i], t);
        }

        return points;
    }

    private Vector3[] GetQuadraticCurveAsPoints(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, int pointCount)
    {
        Vector3[] curve1 = GetQuadraticCurveAsPoints(p1, p2, p3, pointCount);
        Vector3[] curve2 = GetQuadraticCurveAsPoints(p2, p3, p4, pointCount);

        Vector3[] points = new Vector3[pointCount];

        for (int i = 0; i < pointCount; i++)
        {
            float t = i / pointCount;

            points[i] = Vector3.Lerp(curve1[i], curve2[i], t);
        }

        return points;
    }
}
