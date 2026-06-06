using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BezierCurve), typeof(MeshFilter))]
public class BezierMesh : MonoBehaviour
{
    private BezierCurve bezierCurve;
    private List<Vector3> vertices = new();
    private List<int> tris = new();
    private Mesh mesh;

    private void Start()
    {
        bezierCurve = GetComponent<BezierCurve>();
        mesh = new Mesh();
        GetComponent<MeshFilter>().sharedMesh = mesh;
    }

    private void OnValidate()
    {
        if (bezierCurve == null)
            bezierCurve = GetComponent<BezierCurve>();
        if (bezierCurve == null) return;

        GenerateMesh();
    }

    [ContextMenu("Generate Mesh")]
    public void GenerateMesh()
    {
        List<Vector3> bezierPoints = bezierCurve.BezierPoints;
        if (bezierPoints == null || bezierPoints.Count < 2) return;

        vertices.Clear();
        tris.Clear();

        AddSquare(new int[] { 6, 4, 2, 0 });
        for (int i = 0; i < bezierPoints.Count - 1; i++)
        {
            Vector3 vecToNext = (bezierPoints[i + 1] - bezierPoints[i]);
            float dist = vecToNext.magnitude;
            Vector3 fwd = vecToNext.normalized;
            Vector3 right = Vector3.Cross(fwd, Vector3.up).normalized;
            
            // handle the degenerate case when fwd ~= Vector3.up
            if (right.sqrMagnitude < 0.001f)
                right = Vector3.Cross(fwd, Vector3.forward).normalized;

            // Make vertices surrounding this point.
            List<Vector3> verticesForThisPoint = GenerateVerticesAroundPoint(dist, 1, bezierPoints[i], fwd, right);
            //make the face
            tris.AddRange(CustomTriangulation(vertices.Count));
            vertices.AddRange(verticesForThisPoint);
        }

        //add the top and bottom faces
        AddSquare(new int[] {
            vertices.Count - 8,
            vertices.Count - 6,
            vertices.Count - 4,
            vertices.Count - 2
        });

        SetupMesh();
    }

    private void AddSquare(int[] squareVertices)
    {
        tris.Add(squareVertices[0]);
        tris.Add(squareVertices[1]);
        tris.Add(squareVertices[2]);

        tris.Add(squareVertices[2]);
        tris.Add(squareVertices[3]);
        tris.Add(squareVertices[0]);
    }

    private void SetupMesh()
    {
        //print("Mesh setup");
        if (mesh == null)
        {
            mesh = new Mesh();
            GetComponent<MeshFilter>().sharedMesh = mesh;
        }

        mesh.Clear();
        mesh.SetVertices(vertices);
        mesh.SetTriangles(tris, 0);
        mesh.RecalculateNormals();
    }

    private List<Vector3> GenerateVerticesAroundPoint(float length, float width, Vector3 center, Vector3 fwdDir, Vector3 rightDir)
    {
        Vector3 upDir = Vector3.Cross(rightDir, fwdDir).normalized;

        float halfWidth = width / 2;
        float halfLength = length / 2;

        Vector3 backCenter = center - fwdDir * halfLength;
        Vector3 frontCenter = center + fwdDir * halfLength;

        Vector3 rightOffset = rightDir * halfWidth;
        Vector3 verticalOffset = upDir * halfWidth;

        //we need to return these vertices in order, clockwise.
        return new() {
            backCenter - verticalOffset - rightOffset, // back bottom left
            backCenter - rightOffset,                  // back left
            backCenter + verticalOffset - rightOffset, // back top left
            backCenter + verticalOffset,               // back top center
            backCenter + verticalOffset + rightOffset, // back top right
            backCenter + rightOffset,                  // back right
            backCenter - verticalOffset + rightOffset, // back bottom right
            backCenter - verticalOffset,               // back bottom center

            //fronts
            frontCenter - verticalOffset - rightOffset,
            frontCenter - rightOffset,
            frontCenter + verticalOffset - rightOffset,
            frontCenter + verticalOffset,
            frontCenter + verticalOffset + rightOffset,
            frontCenter + rightOffset,
            frontCenter - verticalOffset + rightOffset,
            frontCenter - verticalOffset,
        };
    }

    private List<int> CustomTriangulation(int startPoint)
    {
        //print("Start point: " + startPoint);
        List<int> tris = new();
        // built specifically for the GenerateVerticesAroundPoint method
        // we know adding 8 leads to the vertex in front.
        int topFaceEnd = startPoint + 7;
        int bottomStart = topFaceEnd + 1;
        int bottomEnd = startPoint + 15;
        //print($"start: {startPoint}, top end: {topFaceEnd}, bottom end: {bottomEnd}, bottomstart: {bottomStart}");
        // we're splitting up each section into quads = two triangles per quad
        for (int i = startPoint; i <= topFaceEnd; i++)
        {
            //first tri
            tris.Add(i);
            tris.Add(LimitInRange((i + 1), startPoint, topFaceEnd)); //next to i, within the same face
            tris.Add(LimitInRange((i + 8 + 1), bottomStart, bottomEnd)); // next to whats under i, different face

            //second tri
            tris.Add(LimitInRange((i + 8), bottomStart, bottomEnd)); // under i, diff face
            tris.Add(i); // i
            tris.Add(LimitInRange((i + 8 + 1), bottomStart, bottomEnd)); //next to under i

            //print($"Tris added: ({i},{LimitInRange((i + 1), startPoint, topFaceEnd)},{LimitInRange((i + 8 + 1), bottomStart, bottomEnd)}), " +
            //    $"({LimitInRange((i + 8), bottomStart, bottomEnd)}, {i}, {LimitInRange((i + 8 + 1), bottomStart, bottomEnd)})");
        }

        return tris;
    }

    private int LimitInRange(int i, int start, int end)
    {
        int range = end - start + 1; // +1 makes end inclusive
        return start + ((i - start) % range);
    }
}