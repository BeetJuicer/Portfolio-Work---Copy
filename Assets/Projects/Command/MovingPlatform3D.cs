using UnityEngine;

using UnityEngine;

public class MovingPlatform3D : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float speed = 3f;
    [SerializeField] private float waitTime = 1.5f;

    private int currentWaypointIndex = 0;
    private float waitTimer = 0f;
    private bool isWaiting = false;

    public Vector3 PlatformVelocity { get; private set; }
    private Vector3 previousPosition;

    void Start()
    {
        previousPosition = transform.position;
    }

    void FixedUpdate()
    {
        if (waypoints == null || waypoints.Length == 0) return;

        if (isWaiting)
        {
            PlatformVelocity = Vector3.zero;
            previousPosition = transform.position;

            waitTimer -= Time.fixedDeltaTime;
            if (waitTimer <= 0f)
            {
                isWaiting = false;
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            }
            return;
        }

        Transform target = waypoints[currentWaypointIndex];
        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        float movementStep = speed * Time.fixedDeltaTime;

        if (distanceToTarget <= movementStep)
        {
            transform.position = target.position;
            isWaiting = true;
            waitTimer = waitTime; 
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, movementStep);
        }

        PlatformVelocity = (transform.position - previousPosition) / Time.fixedDeltaTime;
        previousPosition = transform.position;
    }
}