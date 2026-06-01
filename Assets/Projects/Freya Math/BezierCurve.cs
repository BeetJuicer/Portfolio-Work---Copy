using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.LowLevelPhysics2D;
using UnityEngine.UIElements;

[RequireComponent(typeof(MeshFilter))]
public class BezierCurve : MonoBehaviour
{
    public List<Vector3> controlAndAnchorPoints = new();
    [SerializeField] private int pointsPerSegment = 1;

    public List<Vector3> BezierPoints { get; private set; } = new();
    public float BezierLength { get; private set; } = 0;

    private List<float> cumulativeDistances = new();
    private List<float> segmentLengths = new();
    int segments;
    int totalPoints;

    private void OnValidate()
    {
        if (controlAndAnchorPoints == null || controlAndAnchorPoints.Count < 3) return;
        GenerateBezier();
    }

    public void AddAnchorPoint()
    {
        if(controlAndAnchorPoints.Count >= 2)
        {
            Vector3 prevPoint = controlAndAnchorPoints.Last();
            Vector3 prevPrevPt = controlAndAnchorPoints[controlAndAnchorPoints.Count - 2];

            Vector3 dir = (prevPoint - prevPrevPt).normalized; 

            controlAndAnchorPoints.Add(prevPoint + dir * 1f); //add control
            controlAndAnchorPoints.Add(prevPoint + dir * 2f); //add control
            controlAndAnchorPoints.Add(prevPoint + dir * 3f); //add anchor point
        }
    }

    [ContextMenu("Generate Bezier")]
    public void GenerateBezier()
    {
        BezierLength = 0;
        cumulativeDistances.Clear();
        BezierPoints.Clear();
        segmentLengths.Clear();

        segments = (controlAndAnchorPoints.Count - 1) / 3;
        totalPoints = pointsPerSegment * segments;

        // per segment — build cumulativeDistances and segmentLengths
        for (int segment = 0; segment < segments; segment++)
        {
            Vector3 a1 = controlAndAnchorPoints[segment * 3];
            Vector3 c1 = controlAndAnchorPoints[segment * 3 + 1];
            Vector3 c2 = controlAndAnchorPoints[segment * 3 + 2];
            Vector3 a2 = controlAndAnchorPoints[segment * 3 + 3];

            float segmentLength = 0f;

            for (int i = 0; i < pointsPerSegment - 1; i++) 
            {
                float t = (float)i / pointsPerSegment;
                float t2 = (float)(i + 1) / pointsPerSegment;

                Vector3 pt1 = GetPointOnCubicBezier(a1, c1, c2, a2, t);
                Vector3 pt2 = GetPointOnCubicBezier(a1, c1, c2, a2, t2);

                float dist = (pt2 - pt1).magnitude;
                cumulativeDistances.Add(BezierLength);
                BezierLength += dist;
                segmentLength += dist;
            }

            cumulativeDistances.Add(BezierLength);
            segmentLengths.Add(segmentLength); // store this segment's length alone, not cumulative
        }

        // get points at equal distances apart
        for (int i = 0; i < totalPoints - 1; i++)
        {
            float percentage = (float)i / totalPoints;
            float desiredDist = percentage * BezierLength;

            float t = GetTOnBezierByDistance(desiredDist);
            BezierPoints.Add(GetPointOnBezier(t));
        }
        BezierPoints.Add(controlAndAnchorPoints.Last());
    }

    // Returns 0 to 1 for any distance on the bezier curve.
    private float GetTOnBezierByDistance(float desiredDistance)
    {
        if (desiredDistance >= BezierLength)
            return 1f;

        float tIncrement = 1f / totalPoints;
        for (int i = 0; i < totalPoints; i++)
        {
            if (desiredDistance <= cumulativeDistances[i + 1])
            {
                float tEquivalent = tIncrement * i;
                float t2Equivalent = tIncrement * (i + 1);

                return math.remap(cumulativeDistances[i], cumulativeDistances[i + 1], tEquivalent, t2Equivalent, desiredDistance);
            }
        }

        Debug.LogError($"Desired distance improper:  ({desiredDistance}). This should not happen if the method is used correctly.");
        return -1f;
    }

    private Vector3 GetPointOnQuadraticBezier(Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        Vector3 p = (1 - t) * (1 - t) * p1 + 2 * (1 - t) * t * p2 + t * t * p3;
        return p;
    }

    private Vector3 GetPointOnCubicBezier(Vector3 a1, Vector3 c1, Vector3 c2, Vector3 a2, float t)
    {
        Vector3 p = (1 - t) * (1 - t) * (1 - t) * a1 + 3 * (1 - t) * (1 - t) * t * c1 + 3 * (1 - t) * t * t * c2 + t * t * t * a2;
        return p;
    }

    private Vector3 GetPointOnBezier(float t)
    {
        float desiredDist = t * BezierLength;
        float cumulative = 0f;

        for (int i = 0; i < segmentLengths.Count; i++)
        {
            float segEnd = cumulative + segmentLengths[i];

            if (desiredDist <= segEnd || i == segmentLengths.Count - 1)
            {
                float tOnThisSegment = Mathf.InverseLerp(cumulative, segEnd, desiredDist);

                Vector3 a1 = controlAndAnchorPoints[i * 3];
                Vector3 c1 = controlAndAnchorPoints[i * 3 + 1];
                Vector3 c2 = controlAndAnchorPoints[i * 3 + 2];
                Vector3 a2 = controlAndAnchorPoints[i * 3 + 3];

                return GetPointOnCubicBezier(a1, c1, c2, a2, tOnThisSegment);
            }

            cumulative += segmentLengths[i];
        }

        return controlAndAnchorPoints[controlAndAnchorPoints.Count - 1];
    }

    private void OnDrawGizmos()
    {
        if (BezierPoints == null || BezierPoints.Count < 2) return;

        //Draw The Bezier Mesh
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(BezierPoints[0], 0.1f);

        Gizmos.color = Color.black;
        Gizmos.DrawLine(BezierPoints[0], BezierPoints[1]);

        int totalPoints = BezierPoints.Count;
        for (int i = 1; i < totalPoints - 1; i++)
        {
            Gizmos.DrawSphere(BezierPoints[i], 0.1f);
            Gizmos.DrawLine(BezierPoints[i], BezierPoints[i + 1]);
        }

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(BezierPoints[BezierPoints.Count - 1], 0.1f);
    }
}