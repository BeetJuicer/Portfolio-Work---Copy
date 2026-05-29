using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class ConcaveTriangulation : MonoBehaviour
{
    Vector3[] square = { new(0, 0, 0), new(0, 1, 0), new(1, 1, 0), new(1, 0, 0) };

    Vector3[] arrow = { new(0, 0, 0), new(0, 2, 0), new(1, 1, 0), new(2, 2, 0), new(2, 0, 0) };

    Vector3[] lShape = { new(0, 0, 0), new(0, 3, 0), new(2, 3, 0), new(2, 1, 0), new(1, 1, 0), new(1, 0, 0) };

    Vector3[] uShape = { new(0, 0, 0), new(0, 3, 0), new(3, 3, 0), new(3, 0, 0), new(2, 0, 0), new(2, 1, 0), new(1, 1, 0), new(1, 0, 0) };

    Vector3[] spiky = { new(0, 0, 0), new(0, 3, 0), new(4, 3, 0), new(4, 0, 0), new(3, 1, 0), new(2, 0, 0), new(1, 1, 0) };

    Vector3[] nearCollinear = { new(0, 0, 0), new(0, 2, 0), new(3, 2, 0), new(3, 0, 0), new(2, 0.001f, 0), new(1, 0, 0) };
    Vector3[] fan = { new(0, 1, 0), new(0, 4, 0), new(1, 3, 0), new(2, 4, 0), new(2, 6, 0), new(4, 6, 0), new(5, 3, 0), new(3, 1, 0), new(1, 0, 0) };

    [SerializeField] private Vector3[] vertices;
    private List<int> triangles = new();

    private void OnValidate()
    {
        vertices = fan;
        Triangulate(vertices);
    }

    //Algorithm:
    // Start at 0, and add each 3 consecutive vertices to triangles.
    // Once all vertices added, finish the triangle off by making a triangle with the last endPoint of the previous triangle and the "startPoints" of previous triangles.
    // if only one startPoint remains, we cant make a triangle with that. end algorithm.
    //private void Triangulate(Vector3[] vertices)
    //{
    //    triangles.Clear();
    //    if (vertices.Length < 3) return;

    //    Queue<int> startPoints = new Queue<int>();
    //    int lastEndPoint = 0;
    //    int totalTriangles = 0;

    //    // vertices - 1. no need to check the last vertex.
    //    for (int i = 0; i < vertices.Length - 1; i += 2)
    //    {
    //        startPoints.Enqueue(i);

    //        triangles.Add(i);
    //        triangles.Add(i + 1);
    //        lastEndPoint = (i + 2) % vertices.Length;
    //        triangles.Add(lastEndPoint);

    //        totalTriangles++;
    //    }
    //    //initial exploration done, finish triangle.

    //    while (totalTriangles < vertices.Length - 2)
    //    {
    //        totalTriangles++;

    //        startPoints.Enqueue(lastEndPoint);
    //        triangles.Add(lastEndPoint);
    //        triangles.Add(startPoints.Dequeue());

    //        lastEndPoint = startPoints.Dequeue();
    //        triangles.Add(lastEndPoint);
    //    }
    //}

    //private void Triangulate(Vector3[] vertices)
    //{
    //    triangles.Clear();
    //    if (vertices.Length < 3) return;

    //    List<int> concavePoints = new();
    //    for (int i = 0; i < vertices.Length; i++)
    //    {
    //        int t1 = i, t2 = (i + 1) % vertices.Length, t3 = (i + 2) % vertices.Length;

    //        Vector3 p1 = vertices[t1], p2 = vertices[t2], p3 = vertices[t3];
    //        Vector3 toB = p2 - p1;
    //        Vector3 toC = p3 - p2;

    //        print($"wedge: {t1}, {t2}, {t3}: " + WedgeProduct(toB, toC));
    //        if (!IsClockWise(toB, toC))
    //        {
    //            concavePoints.Add(t2); // the center point is the concave point
    //        }
    //        else
    //        {
    //            triangles.Add(t1);
    //            triangles.Add(t2);
    //            triangles.Add(t3);
    //        }
    //    }

    //    foreach (var concave in concavePoints)
    //    {
    //        print($"concave point: {concave}");
    //        for (int i = 0; i < vertices.Length; i++)
    //        {
    //            if (i == concave) continue;

    //            int t1 = concave, t2 = i, t3 = (i + 1) % vertices.Length;

    //            Vector3 p1 = vertices[t1], p2 = vertices[t2], p3 = vertices[t3];
    //            Vector3 toB = p2 - p1;
    //            Vector3 toC = p3 - p2;

    //            print($"2wedge: {t1}, {t2}, {t3}: " + WedgeProduct(toB, toC));
    //            if (IsClockWise(toB, toC) && !TriangleCrossesOtherPointsInMesh((p1, p2, p3), (t1, t2, t3)) && !TriangleCrossesShapeEdges((p1,p2,p3), (t1, t2, t3))) 
    //            {
    //                triangles.Add(t1);
    //                triangles.Add(t2);
    //                triangles.Add(t3);
    //            }
    //        }
    //    }
    //}

    private void Triangulate(Vector3[] vertices)
    {
        triangles.Clear();
        if (vertices.Length < 3) return;

        for (int i = 0; i < vertices.Length; i++)
        {
            int t1 = i, t2 = (i + 1) % vertices.Length, t3 = (i + 2) % vertices.Length;

            Vector3 p1 = vertices[t1], p2 = vertices[t2], p3 = vertices[t3];
            Vector3 toB = p2 - p1;
            Vector3 toC = p3 - p2;

            print($"wedge: {t1}, {t2}, {t3}: " + WedgeProduct(toB, toC));
            if (!IsClockWise(toB, toC))
            {
                //concavePoints.Add(t2); // the center point is the concave point
            }
            else
            {
                triangles.Add(t1);
                triangles.Add(t2);
                triangles.Add(t3);
            }
        }
    }

    private bool TriangleCrossesOtherPointsInMesh((Vector3, Vector3, Vector3) triangle, (int,int,int) vertIndices)
    {
        for (int i = 0; i < vertices.Length; i++)
        {
            //skip the points of this triangle
            if (i == vertIndices.Item1 || i == vertIndices.Item2 || i == vertIndices.Item3)
            {
                continue;
            }

            if (TriangleContainsPoint(triangle, vertices[i]))
                return true;
        }

        return false;
    }

    private bool TriangleCrossesShapeEdges((Vector3, Vector3, Vector3) triangle, (int, int, int) vertIndices)
    {
        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 edgeP1 = vertices[i];
            Vector3 edgeP2 = vertices[(i + 1) % vertices.Length];

            if (LinesIntersect((triangle.Item1, triangle.Item2), (edgeP1, edgeP2)) ||
                LinesIntersect((triangle.Item2, triangle.Item3), (edgeP1, edgeP2)) ||
                LinesIntersect((triangle.Item3, triangle.Item1), (edgeP1, edgeP2)))
            {
                return true;
            }
        }

        return false;
    }

    private bool LinesIntersect((Vector3, Vector3) line1, (Vector3, Vector3) line2)
    {
        float wedge1 = WedgeProduct(line1.Item1 - line1.Item2, line2.Item1 - line1.Item1);
        float wedge2 = WedgeProduct(line1.Item1 - line1.Item2, line2.Item2 - line1.Item1);
        float wedge3 = WedgeProduct(line2.Item2 - line2.Item1, line1.Item1 - line2.Item1);
        float wedge4 = WedgeProduct(line2.Item2 - line2.Item1, line1.Item2 - line2.Item1);

        return Mathf.Sign(wedge1) != Mathf.Sign(wedge2) && Mathf.Sign(wedge3) != Mathf.Sign(wedge4);
    }

    private bool TriangleOverlapsOtherTriangles((Vector3, Vector3, Vector3) triangle, (int,int,int) vertIndices)
    {
        for (int i = 0; i < triangles.Count; i += 3)
        {
            int t1 = triangles[i], t2 = triangles[i + 1], t3 = triangles[i + 2];
            //skip if shares a vertex
            if (t1 == vertIndices.Item1 || t1 == vertIndices.Item2 || t1 == vertIndices.Item3 ||
                t2 == vertIndices.Item1 || t2 == vertIndices.Item2 || t2 == vertIndices.Item3 ||
                t3 == vertIndices.Item1 || t3 == vertIndices.Item2 || t3 == vertIndices.Item3)
            {
                continue;
            }
         
            if (TrianglesOverlap(triangle, (vertices[t1], vertices[t2], vertices[t3])))
                return true;
        }
        return false;
    }

    private bool TrianglesOverlap((Vector3, Vector3, Vector3) t1, (Vector3, Vector3, Vector3) t2)
    {
        // Check if any vertex of t1 is inside t2
        if (TriangleContainsPoint(t2, t1.Item1) || TriangleContainsPoint(t2, t1.Item2) || TriangleContainsPoint(t2, t1.Item3))
            return true;
        // Check if any vertex of t2 is inside t1
        if (TriangleContainsPoint(t1, t2.Item1) || TriangleContainsPoint(t1, t2.Item2) || TriangleContainsPoint(t1, t2.Item3))
            return true;
     


        return false;
    }

    private bool IsClockWise(Vector3 d1, Vector3 d2)
    {
        return WedgeProduct(d1, d2) < 0;
    }

    private float GetAngle(Vector3 a, Vector3 b)
    {
        return Mathf.Acos(Vector3.Dot(a.normalized, b.normalized)) * Mathf.Rad2Deg;
    }


    private Vector3 GetTriangleCenter(Vector3 p1, Vector3 p2, Vector3 p3)
    {
        return (p1 + p2 + p3) / 3f;
    }

    //takes directional vectors
    private float WedgeProduct(Vector3 d1, Vector3 d2)
    {
        return (d1.x * d2.y) - (d1.y * d2.x);
    }

    private bool TriangleContainsPoint((Vector3, Vector3, Vector3) triangle, Vector3 point)
    {
        Vector3 a = triangle.Item1, b = triangle.Item2, c = triangle.Item3;

        float w1 = WedgeProduct(a - point, b - point);
        if (w1 == 0) return true;

        float w2 = WedgeProduct(b - point, c - point);
        if (w2 == 0) return true;

        float w3 = WedgeProduct(c - point, a - point);
        if(w3 == 0) return true;

        float s1 = Mathf.Sign(w1);
        float s2 = Mathf.Sign(w2);
        float s3 = Mathf.Sign(w3);
        return s1 == s2 && s2 == s3;
    }

    private void OnDrawGizmos()
    {
        Gizmos.matrix = Handles.matrix = transform.localToWorldMatrix;

        void DrawTriangleHandles(Vector3 p1, Vector3 p2, Vector3 p3)
        {
            Vector3[] verts = new Vector3[]
            {
                p1,
                p2,
                p3
            };

            Handles.color = Color.white;
            Handles.DrawAAConvexPolygon(verts);
        }

        // Draw the triangles
        Gizmos.color = Color.red;

        for (var i = 0; i < triangles.Count; i += 3)
        {
            print($"Drawing: {triangles[i]}, {triangles[i + 1]}, {triangles[i + 2]}");

            DrawTriangleHandles(vertices[triangles[i]], vertices[triangles[i + 1]], vertices[triangles[i + 2]]);

            Gizmos.DrawLine(vertices[triangles[i]], vertices[triangles[i + 1]]);
            Gizmos.DrawLine(vertices[triangles[i + 1]], vertices[triangles[i + 2]]);
            Gizmos.DrawLine(vertices[triangles[i + 2]], vertices[triangles[i]]);
        }

        // Draw the shape
        Gizmos.color = Color.blue;

        foreach (var v in vertices)
        {
            Gizmos.DrawSphere(v, 0.01f);
        }

        for (var i = 0; i < vertices.Length - 1; i++)
        {
            Gizmos.DrawLine(vertices[i], vertices[i + 1]);
        }

        //line from last to first
        if (vertices.Length > 1)
        {
            Gizmos.DrawLine(vertices[vertices.Length - 1], vertices[0]);
        }

    }
}
