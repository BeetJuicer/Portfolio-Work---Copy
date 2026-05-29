using JetBrains.Annotations;
using System.Collections.Generic;
using System.Linq;
using Unity.Cinemachine;
using Unity.Mathematics;
using UnityEngine;

public class Freya : MonoBehaviour
{
    [SerializeField] private Transform t1;
    [SerializeField] private Transform t2;
    [SerializeField] private Transform t3;
    [SerializeField] int DETAIL = 1;


    Vector3 p1;
    Vector3 p2;
    Vector3 p3;

    List<Vector3> vertices = new();
    List<int> tris = new();
    Mesh mesh;

    private float bezierLength = 0;
    private List<float> bezierPointCumulativeDistance = new();
    private List<Vector3> bezierPoints = new();

    private void Start()
    {
        mesh = new Mesh();

        mesh.Clear();
        mesh.SetVertices(vertices);
        mesh.SetTriangles(tris, 0);
        mesh.RecalculateNormals();

        GetComponent<MeshFilter>().sharedMesh = mesh;
        p1 = t1.position; p2 = t2.position; p3 = t3.position;
    }

    [ContextMenu("Generate Bezier")]
    public void GenerateBezier()
    {
        p1 = t1.position; p2 = t2.position; p3 = t3.position;

        bezierLength = 0;
        bezierPointCumulativeDistance.Clear();
        bezierPoints.Clear();

        // Get the total distances per point
        for (int i = 0; i < DETAIL; i++)
        {
            float t = (float)i / DETAIL;
            float t2 = (float)(i + 1) / DETAIL;

            Vector3 pt1 = GetPointOnBezier(p1, p2, p3, t);
            Vector3 pt2 = GetPointOnBezier(p1, p2, p3, t2); //we're calculating p2 twice. this iteration, and next iteration it'll be p1

            Vector3 from1to2 = (pt2 - pt1);
            float dist = from1to2.magnitude;
         
            bezierPointCumulativeDistance.Add(bezierLength);
            bezierLength += dist;
        }
        bezierPointCumulativeDistance.Add(bezierLength); // add the total length at the end

        // get the points at equal distances apart
        for (int i = 0;i < DETAIL;i++)
        {
            float percentage = (float)i / DETAIL;
            float desiredDist = percentage * bezierLength;

            float t = GetTByDistance(desiredDist);
            bezierPoints.Add(GetPointOnBezier(p1, p2, p3, t));
        }

        bezierPoints.Add(p3); // add the end point at the end

    }

    [ContextMenu("Generate Mesh")]
    public void GenerateMesh()
    {
        vertices.Clear();
        tris.Clear();

        for (int i = 0; i < bezierPoints.Count - 1; i++)
        {
            Vector3 vecToNext = (bezierPoints[i + 1] - bezierPoints[i]);
            float dist = vecToNext.magnitude;
            Vector3 fwd = vecToNext.normalized;
            Vector3 right = Vector3.right;
            
            // Make vertices surrounding this point.
            List<Vector3> verticesForThisPoint = GenerateVerticesAroundPoint(dist, 1, bezierPoints[i], fwd, right);
            tris.AddRange(CustomTriangulation(vertices.Count));
            vertices.AddRange(verticesForThisPoint);
        }


        print("Bezier Length: " + bezierLength);
        SetupMesh();
    }

    private void OnValidate()
    {
        GenerateBezier();
        GenerateMesh();
    }

    private void OnDrawGizmos()
    {
        //Draw The Bezier Mesh

        //foreach (var vertex in vertices)
        //{
        //    Gizmos.DrawSphere(vertex, 0.05f);
        //}        

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(bezierPoints[0], 0.1f);



        Gizmos.color = Color.white;
        Gizmos.DrawLine(bezierPoints[0], bezierPoints[1]);
        Gizmos.color = Color.white;
        for (int i = 1; i < DETAIL; i++)
        {
            Gizmos.DrawSphere(bezierPoints[i], 0.1f);
            Gizmos.DrawLine(bezierPoints[i], bezierPoints[i + 1]);
        }

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(bezierPoints[DETAIL], 0.1f);

    }

    private float GetTByDistance(float desiredDistance) {
        
        if(desiredDistance >= bezierLength)
        {
            return bezierLength;
        }

        float tIncrement = (float)1f / DETAIL;

        for (int i = 0; i < DETAIL; i++)
        {
            print($"Comparing desired: {desiredDistance} with {i + 1}: {bezierPointCumulativeDistance[i + 1]}");
            if(desiredDistance <= bezierPointCumulativeDistance[i + 1])
            {
                print($"Desired Distance {desiredDistance} found before bezier point ({i + 1}): {bezierPointCumulativeDistance[i + 1]}");
                float tEquivalent = tIncrement * i;
                float t2Equivalent = tIncrement * (i + 1);
                
                return math.remap(bezierPointCumulativeDistance[i], bezierPointCumulativeDistance[i + 1], tEquivalent, t2Equivalent, desiredDistance);
            }
        }

        Debug.LogError("Desired distance improper. This should not happen if the method is used correctly.");
        //error.
        return -1f;
    }
    private void SetupMesh()
    {
        print("Mesh setup");
        // Mesh Setup
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

    private Vector3 GetPointOnBezier(Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        Vector3 p = (1 - t) * (1 - t) * p1 + 2 * (1 - t) * t * p2 + t * t * p3;
        return p;
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
            backCenter - rightOffset, // backleft
            backCenter + verticalOffset - rightOffset, // back top left
            backCenter + verticalOffset, // back top center
            backCenter + verticalOffset + rightOffset, // back top right
            backCenter + rightOffset, // backright
            backCenter - verticalOffset + rightOffset, // back bottom right
            backCenter - verticalOffset,                 //back  bottom center 
            
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
        print("Start point: " + startPoint);
        List<int> tris = new();
        // built specifically for the GenerateVerticesAroundPoint method
        // we know adding 8 leads to the vertex in front.
        int topfaceEndPoint = startPoint + 7;
        int bottomEndPoint = startPoint + 15;
        int bottomStartPoint = topfaceEndPoint + 1;
        print($"start: {startPoint}, top end: {topfaceEndPoint}, bottom end: {bottomEndPoint}, bottomstart: {bottomStartPoint}"); 
        // we're splitting up each section into quads = two triangles per quad
        for (int i = startPoint; i <= topfaceEndPoint; i++)
        {
            //first tri
            tris.Add(i); //me
            tris.Add(LimitInRange((i + 1), startPoint, topfaceEndPoint)); //next to me, within the same face
            tris.Add(LimitInRange((i + 8 + 1), bottomStartPoint, bottomEndPoint)); // next to whats under me, different face

            //second tri
            tris.Add(LimitInRange((i + 8), bottomStartPoint, bottomEndPoint)); // under me, diff face
            tris.Add(i); // me
            tris.Add(LimitInRange((i + 8 + 1), bottomStartPoint, bottomEndPoint)); //next to under me

            print($"Tris added: ({i},{LimitInRange((i + 1), startPoint, topfaceEndPoint)},{LimitInRange((i + 8 + 1), bottomStartPoint, bottomEndPoint)}), " +
                $"({LimitInRange((i + 8), bottomStartPoint, bottomEndPoint)}, {i}, {LimitInRange((i + 8 + 1), bottomStartPoint, bottomEndPoint)})");
        }

        return tris;
    }

    private int LimitInRange(int i, int start, int end)
    {
        int range = end - start + 1; // +1 makes end inclusive
        return start + ((i - start) % range);
    }
}
