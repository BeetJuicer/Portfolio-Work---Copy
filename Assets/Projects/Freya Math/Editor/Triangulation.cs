using System.Collections.Generic;
using UnityEngine;

public class Triangulation : MonoBehaviour
{
    [SerializeField] private Vector3[] vertices;
    private List<int> triangles = new();

    private void OnValidate()
    {
        Triangulate(vertices);
    }

    //Algorithm:
    // Start at 0, and add each 3 consecutive vertices to triangles.
    // Once all vertices added, finish the triangle off by making a triangle with the last endPoint of the previous triangle and the "startPoints" of previous triangles.
    // if only one startPoint remains, we cant make a triangle with that. end algorithm.
    private void Triangulate(Vector3[] vertices)
    {
        triangles.Clear();
        if (vertices.Length < 3) return;

        Queue<int> startPoints = new Queue<int>();
        int lastEndPoint = 0;
        int totalTriangles = 0;

        // vertices - 1. no need to check the last vertex.
        for (int i = 0; i < vertices.Length - 1; i+=2)
        {
            startPoints.Enqueue(i);

            triangles.Add(i);
            triangles.Add(i+1);
            lastEndPoint = (i + 2) % vertices.Length;
            triangles.Add(lastEndPoint);
            
            totalTriangles++;
        }
        //initial exploration done, finish triangle.

        while(totalTriangles < vertices.Length - 2)
        {
            totalTriangles++;

            startPoints.Enqueue(lastEndPoint);
            triangles.Add(lastEndPoint);
            triangles.Add(startPoints.Dequeue());

            lastEndPoint = startPoints.Dequeue();
            triangles.Add(lastEndPoint);
        }
    }

    private void OnDrawGizmos()
    {
        // Draw the shape
        Gizmos.color = Color.white;

        foreach (var v in vertices)
        {
            Gizmos.DrawSphere(v, 0.01f);
        }

        for (var i = 0; i < vertices.Length - 1; i++)
        {
            Gizmos.DrawLine(vertices[i], vertices[i + 1]);
        }

        //line from last to first
        if(vertices.Length > 1)
        {
            Gizmos.DrawLine(vertices[vertices.Length - 1], vertices[0]);
        }

        // Draw the triangles
        Gizmos.color = Color.red;

        for (var i = 0; i < triangles.Count; i += 3)
        {
            print($"{i}, {i + 1}, {i + 2}");

            Gizmos.DrawLine(vertices[triangles[i]], vertices[triangles[i + 1]]);
            Gizmos.DrawLine(vertices[triangles[i + 1]], vertices[triangles[i + 2]]);
            Gizmos.DrawLine(vertices[triangles[i + 2]], vertices[triangles[i]]);
        }
    }
}
