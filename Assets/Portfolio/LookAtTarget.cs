using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 localForward = Vector3.forward;

    private void Update()
    {
        if (target == null) return;

        Vector3 directionToTarget = target.position - transform.position;

        // Build a rotation where localForward points at target
        Quaternion toTarget = Quaternion.LookRotation(directionToTarget);
        Quaternion fromForward = Quaternion.FromToRotation(Vector3.forward, localForward);
        transform.rotation = toTarget * Quaternion.Inverse(fromForward);
    }
}