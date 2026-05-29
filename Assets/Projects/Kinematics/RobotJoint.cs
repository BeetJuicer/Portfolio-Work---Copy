using UnityEngine;

public class RobotJoint : MonoBehaviour
{
    public Vector3 axis;
    public Vector3 startOffset;

    public float minAngle = -360f;
    public float maxAngle = 360f;
    void Awake()
    {
        startOffset = transform.position;
    }
}
