using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;
using System.Collections.Generic;

[RequireComponent(typeof(PolygonCollider2D))]
public class MySplineColliderGenerator : MonoBehaviour
{
    [SerializeField] private int pointCount = 20;
    private SplineContainer container;
    List<Vector3> points = new();
    List<Vector3> leftEdgePoints = new();
    List<Vector3> rightEdgePoints = new();

    private float width = 2;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Generate();
    }

    public void SetWidth(float f)
    {
        width = f;
    }

#if UNITY_EDITOR
    private void OnValidate() => UnityEditor.EditorApplication.delayCall += () =>
    {
        if (this != null) Generate();
    };
#endif

    void Generate()
    {
        container = GetComponent<SplineContainer>();

        points.Clear();
        leftEdgePoints.Clear();
        rightEdgePoints.Clear();

        //float length = container.Spline.GetLength();
        for (int i = 0; i <= pointCount; i++)
        {
            float t = (float)i / pointCount;
            container.Spline.Evaluate(t, out float3 pos, out float3 tangent, out float3 upVector);

            Vector3 worldPos = container.transform.TransformPoint(pos);
            points.Add(worldPos);
            Vector3 nTan = (container.transform.TransformDirection(tangent)).normalized;
            Vector3 left = (worldPos + new Vector3(-nTan.y, nTan.x) * width);
            Vector3 right = (worldPos + new Vector3(nTan.y, -nTan.x) * width);
            leftEdgePoints.Add(left);
            rightEdgePoints.Add(right);
        }

        //generate polygon
        var poly = GetComponent<PolygonCollider2D>();

        // Go along left edge forward, then right edge backward to form a closed loop
        var path = new List<Vector2>();
        foreach (var p in leftEdgePoints)
        {
            Vector3 local = transform.InverseTransformPoint(p);
            path.Add(new Vector2(local.x, local.y));
        }
        for (int i = rightEdgePoints.Count - 1; i >= 0; i--)
        {
            Vector3 local = transform.InverseTransformPoint(rightEdgePoints[i]);
            path.Add(new Vector2(local.x, local.y));
        }

        poly.SetPath(0, path);
    }

    //private void OnDrawGizmos()
    //{
    //    for (int i = 0;i <= pointCount;i++)
    //    {
    //        Gizmos.color = Color.red;
    //        Gizmos.DrawSphere(points[i], 0.2f);
    //        Gizmos.color = Color.yellow;
    //        Gizmos.DrawSphere(leftEdgePoints[i], 0.2f);
    //        Gizmos.color = Color.green;
    //        Gizmos.DrawSphere(rightEdgePoints[i], 0.2f);
    //        Gizmos.color = Color.white;
    //        Gizmos.DrawLine(points[i], leftEdgePoints[i]);
    //        Gizmos.DrawLine(points[i], rightEdgePoints[i]);
            
    //        Gizmos.color = Color.gray;
    //        if (i < pointCount)
    //        {
    //            Gizmos.DrawLine(leftEdgePoints[i], leftEdgePoints[i + 1]);
    //            Gizmos.DrawLine(rightEdgePoints[i], rightEdgePoints[i + 1]);                
    //        }
    //    }
    //}
}
