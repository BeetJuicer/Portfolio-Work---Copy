using System.Collections.Generic;
using UnityEngine;

public class ForwardKinematicsSample : MonoBehaviour
{
    class Axis
    {
        public float distToPointB;
        public float angleOffsetToPointB; // Local offset relative to parent's initial rotation
        public float localRotationOffset; // Child's rotation relative to parent (in degrees)
        public float previousRotation;
    }

    [SerializeField] private Transform[] points;
    private List<Axis> axes = new();

    private void Start()
    {
        for (int i = 0; i < points.Length - 1; i++)
        {
            Vector2 thisPoint = points[i].position;
            Vector2 nextPoint = points[i + 1].position;

            float worldAngle = Mathf.Atan2(nextPoint.y - thisPoint.y, nextPoint.x - thisPoint.x);
            float parentInitialRad = points[i].eulerAngles.z * Mathf.Deg2Rad;

            axes.Add(new Axis
            {
                distToPointB = Vector2.Distance(thisPoint, nextPoint),
                angleOffsetToPointB = worldAngle - parentInitialRad, // ✅ Local offset
                localRotationOffset = points[i + 1].rotation.eulerAngles.z
                                      - points[i].rotation.eulerAngles.z, // ✅ Relative rotation
                previousRotation = points[i].rotation.eulerAngles.z
            });
        }
    }

    private void Update()
    {
        for (int i = 0; i < points.Length - 1; i++)
        {
            // Rotation propagation
            if (axes[i].previousRotation != points[i].rotation.eulerAngles.z)
            {
                axes[i].previousRotation = points[i].rotation.eulerAngles.z;

                // ✅ Apply local offset on top of parent's current world rotation
                float pointBNewZ = points[i].rotation.eulerAngles.z + axes[i].localRotationOffset;
                points[i + 1].rotation = Quaternion.Euler(0f, 0f, pointBNewZ);
            }

            // Position
            Vector2 ownPosition = points[i].position;
            float newAngleOffset = axes[i].angleOffsetToPointB + points[i].eulerAngles.z * Mathf.Deg2Rad;
            Vector2 newDirection = new Vector2(Mathf.Cos(newAngleOffset), Mathf.Sin(newAngleOffset));
            points[i + 1].position = ownPosition + newDirection * axes[i].distToPointB;
        }
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < points.Length - 1; i++)
        {
            Gizmos.DrawLine(points[i].position, points[i + 1].position);
        }
    }
}