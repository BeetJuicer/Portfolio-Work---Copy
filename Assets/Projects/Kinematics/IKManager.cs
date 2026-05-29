using UnityEngine;
using System.Collections.Generic;

public class IKManager : MonoBehaviour
{
    public List<RobotJoint> joints = new();
    [SerializeField] int SamplingDistance = 2;
    [SerializeField] float learningRate = 2.5f;
    [SerializeField] float DistanceThreshold = 1f;

    public Transform target; // the thing the arm reaches toward
    private float[] angles;


    void Start()
    {
        angles = new float[joints.Count];
        for (int i = 0; i < joints.Count; ++i)
        {
            angles[i] = joints[i].transform.rotation.eulerAngles.z;
            joints[i].axis = new Vector3(0, 0, 1);
        }
    }

    void Update()
    {
        InverseKinematics(target.position, angles);
        for (int i = 0; i < joints.Count; i++)
        {
            joints[i].transform.rotation = Quaternion.Euler(0f, 0f, angles[i]);
        }
    }

    public Vector3 ForwardKinematics(float[] angles)
    {
        Vector3 prevPoint = joints[0].transform.position;
        Quaternion rotation = Quaternion.identity;
        for (int i = 1; i < joints.Count; i++)
        {
            // Rotates around a new axis
            rotation *= Quaternion.AngleAxis(angles[i - 1], joints[i - 1].axis);
            Vector3 nextPoint = prevPoint + rotation * joints[i].startOffset;

            prevPoint = nextPoint;
        }
        return prevPoint;
    }

    public float DistanceFromTarget(Vector3 target, float[] angles)
    {
        Vector3 point = ForwardKinematics(angles);
        return Vector3.Distance(point, target);
    }

    public float PartialGradient(Vector3 target, float[] angles, int jointIndex)
    {
        float angle = angles[jointIndex];
        float f_x = DistanceFromTarget(target, angles);

        angles[jointIndex] += SamplingDistance;
        float f_x_plus_d = DistanceFromTarget(target, angles);

        float gradient = (f_x_plus_d - f_x) / SamplingDistance;
        angles[jointIndex] = angle;

        return gradient;
    }

    public void InverseKinematics(Vector3 target, float[] angles)
    {
        float dist = DistanceFromTarget(target, angles);
        print(dist);

        if (DistanceFromTarget(target, angles) < DistanceThreshold)
            return;

        for (int i = 0; i < joints.Count; i++)
        {
            float gradient = PartialGradient(target, angles, i);
            angles[i] -= learningRate * gradient;

            angles[i] = Mathf.Clamp(angles[i], joints[i].minAngle, joints[i].maxAngle);

            if (DistanceFromTarget(target, angles) < DistanceThreshold)
                return;
        }
    }
}