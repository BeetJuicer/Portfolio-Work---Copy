using System.Threading;
using UnityEditor;
using UnityEngine;

public class TurretDetection : MonoBehaviour
{
    private enum TurretMode
    {
        Wedge,
        Spherical,
        SphericalSector
    }

    [SerializeField] private float maxRadius;
    [SerializeField] private float minRadius;
    [SerializeField] private float height;
    [SerializeField] private Transform target;
    [SerializeField] private GameObject gun;
    [SerializeField] private TurretMode mode;

    [Range(0, 360)]
    [SerializeField] private float fovDeg = 45f;
    float FovRad => fovDeg * Mathf.Deg2Rad;
    float AngThresh => Mathf.Cos(FovRad / 2);
    [SerializeField] private float rotateSpeed = 2f;

    private Transform currentTarget;

    [SerializeField] private Vector3 centerOffset = new Vector3(0, 0.5f, 0);

    private void Update()
    {
        //currentTarget = CheckInRange();
        if(currentTarget != null)
        {
            PointGunAtTarget(currentTarget.position);
        }
    }

    private bool WedgeContains(Vector3 position)
    {
        Vector3 vecToTargetWorld = (position - transform.position);
        Vector3 vecToTarget = transform.InverseTransformVector(vecToTargetWorld);

        if (vecToTarget.y < 0 || vecToTarget.y > height)
        {
            return false; //outside the height
        }

        Vector3 flatDirToTarget = vecToTarget;
        flatDirToTarget.y = 0;

        float flatDistance = flatDirToTarget.magnitude;

        flatDirToTarget = flatDirToTarget / flatDistance; // normalized vector
        if (flatDirToTarget.z < Mathf.Cos((fovDeg * Mathf.Deg2Rad) / 2))
        {
            return false; // outside the angle
        }

        if(flatDistance > maxRadius || flatDistance <= minRadius)
        {
            return false; // outside the range
        }

        return true;
    }

    private void PointGunAtTarget(Vector3 target)
    {
        Vector3 toTarget = (target - gun.transform.position).normalized;
        float t = Time.deltaTime * rotateSpeed;
        Vector3 face = Vector3.Slerp(gun.transform.forward, toTarget, t);
        
        gun.transform.rotation = Quaternion.LookRotation(face, gun.transform.up);
    }

    private void OnDrawGizmos()
    {
        switch(mode)
        {
            case TurretMode.Wedge:
                WedgeGizmos();
                break;
            case TurretMode.Spherical:
                 SphericalGizmos();
                break;
            case TurretMode.SphericalSector:
                SphericalSectorGizmos();
                break;
        }
    }

    private void WedgeGizmos()
    {
        Handles.matrix = transform.localToWorldMatrix;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = Handles.color = WedgeContains(target.position) ? Color.green : Color.red;

        float halfDeg = fovDeg / 2f;
        float p = Mathf.Cos(halfDeg * Mathf.Deg2Rad);
        float x = Mathf.Sin(halfDeg * Mathf.Deg2Rad);

        Vector3 vLeft = new Vector3(-x, 0, p) * maxRadius;
        Vector3 vRight = new Vector3(x, 0, p) * maxRadius;

        Vector3 top = new Vector3(0, height, 0);

        Gizmos.DrawLine(default, top);

        Gizmos.DrawLine(default, vRight);
        Gizmos.DrawLine(default, vLeft);

        Gizmos.DrawRay(top, vLeft);
        Gizmos.DrawRay(top, vRight);

        Gizmos.DrawLine(top, top + vLeft);
        Gizmos.DrawLine(top, top + vRight);
        Gizmos.DrawLine(vRight, top + vRight);
        Gizmos.DrawLine(vLeft, top + vLeft);

        //max radius
        Handles.DrawWireArc(default, Vector3.up, vLeft, fovDeg, maxRadius);
        Handles.DrawWireArc(top, Vector3.up, vLeft, fovDeg, maxRadius);

        //min radius
        Vector3 minLeft = new Vector3(-x, 0, p) * minRadius;
        Vector3 minRight = new Vector3(x, 0, p) * minRadius;

        Handles.color = Color.blue;
        Handles.DrawWireArc(default, Vector3.up, minLeft, fovDeg, minRadius);
        Handles.DrawWireArc(top, Vector3.up, minLeft, fovDeg, minRadius);
    }
    
    private void SphericalGizmos()
    {
        Gizmos.color = SphereContains(target.position) ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position, maxRadius);
    }

    private bool SphereContains(Vector3 position)
    {
        return (target.position - transform.position).sqrMagnitude <= maxRadius * maxRadius;
    }


    private void SphericalSectorGizmos()
    {
        Gizmos.matrix = Handles.matrix = transform.localToWorldMatrix;
        Gizmos.color = Handles.color = SphericalSectorContains(target.position) ? Color.green : Color.red;

        Vector3 fwd = new(0, 0, 1);
        Vector3 up = new(0, 1, 0);
        Vector3 right = new(1, 0, 0);
        Vector3 origin = centerOffset;

        float halfDeg = fovDeg / 2f;
        float p = Mathf.Cos(halfDeg * Mathf.Deg2Rad);
        float x = Mathf.Sin(halfDeg * Mathf.Deg2Rad);

        Vector3 vLeft = new Vector3(-x, 0, p) * maxRadius;
        Vector3 vRight = new Vector3(x, 0, p) * maxRadius;

        Gizmos.DrawLine(origin , origin  + vLeft);
        Gizmos.DrawLine(origin , origin  + vRight);

        Vector3 vTop = new Vector3(0, x, p) * maxRadius;
        Vector3 vBottom = new Vector3(0, -x, p) * maxRadius;
        Gizmos.DrawLine(origin , origin  + vTop);
        Gizmos.DrawLine(origin , origin  + vBottom);
        Handles.DrawWireArc(origin , fwd, vLeft, 360, maxRadius);
        Handles.DrawWireArc(origin , up, vLeft, fovDeg, maxRadius);
        Handles.DrawWireArc(origin , right, vTop, fovDeg, maxRadius);
    }

    private bool SphericalSectorContains(Vector3 position)
    {
        Vector3 toTarget = (target.position + centerOffset - transform.position);
        float sqrMag = toTarget.sqrMagnitude;

        if (sqrMag > maxRadius * maxRadius)
            return false;

        Vector3 toTargetHorizontal = new Vector3(toTarget.x, 0, toTarget.z);
        Vector3 toTargetVertical = new Vector3(0, toTarget.y, toTarget.z);

        float horizontalAngle = Mathf.Acos(Vector3.Dot(transform.forward, toTargetHorizontal.normalized));
        float verticalAngle = Mathf.Acos(Vector3.Dot(transform.forward, toTargetVertical.normalized));
        float halfRad = FovRad / 2;

        if(horizontalAngle > halfRad || verticalAngle > halfRad)
        {
            return false;
        }
        
        return true;
    }
}
