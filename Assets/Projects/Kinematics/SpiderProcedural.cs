using System.Collections.Generic;
using UnityEngine;

public class SpiderProcedural : MonoBehaviour
{
    [System.Serializable]
    public class Leg
    {
        public Transform tracker;
        public FastIKFabric fabrik;
        [HideInInspector] public Vector3 stepStartPos;
    }

    [System.Serializable]
    public class LegPair
    {
        public Leg front;
        public Leg back;
        [HideInInspector] public bool isStepping;
    }

    [Header("Movement")]
    [SerializeField] private float speed = 2f;
    [SerializeField] private float rayLength = 0.3f;
    [SerializeField] private Vector3 topRayOffset = new(0f, 0.3f, 0f);

    [Header("IK / Animation")]
    [SerializeField] private List<LegPair> legPairs = new();
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private float distanceMoveThreshold = 1f;
    [SerializeField] private float rayCastStartOffset = 5f;
    [SerializeField] private float stepHeight;

    [Header("Body")]
    [SerializeField] private Transform body;
    [SerializeField] private float bodyHeightOffset;

    // Stepping state
    private float stepT;
    private int currentPair;

    // Body rotation state
    private Quaternion bodyStartRotation;
    private Vector3 smoothedNormal = Vector3.up;

    // Gizmos
    private Vector3 gizmosDir;
    private Vector3 debugNormal;

    bool wallPlacement;

    // -------------------------------------------------------------------------
    // Unity Lifecycle
    // -------------------------------------------------------------------------

    private void Start()
    {
        float offset = 0.5f * distanceMoveThreshold;
        legPairs[0].front.fabrik.Target.position += Vector3.forward * offset;
        legPairs[0].back.fabrik.Target.position += Vector3.forward * offset;

        bodyStartRotation = body.rotation;
    }

    private void Update()
    {
        if (wallPlacement)
            return;

        HandleMovement();
        UpdateTrackers();
        TryInitiateSteps();
    }

    private void LateUpdate()
    {
        foreach (var pair in legPairs)
        {
            pair.front.fabrik.ResolveIK();
            pair.back.fabrik.ResolveIK();
        }

        PositionBody();
        RotateBody();
    }

    // -------------------------------------------------------------------------
    // Movement
    // -------------------------------------------------------------------------

    private void HandleMovement()
    {
        Vector3 dir = new(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        gizmosDir = dir;

        if (dir == Vector3.zero) return;

        bool bottomHit = Physics.Raycast(transform.position, dir, out RaycastHit bottomInfo, rayLength, whatIsGround);
        bool topHit = Physics.Raycast(transform.position + topRayOffset, dir, out _, rayLength, whatIsGround);

        //if (!topHit && !bottomHit)

        {
            transform.Translate(dir * speed * Time.deltaTime);
            return;
        }
    }

    // -------------------------------------------------------------------------
    // Leg Stepping
    // -------------------------------------------------------------------------

    private void UpdateTrackers()
    {
        foreach (var pair in legPairs)
        {
            RaycastTracker(pair.front);
            RaycastTracker(pair.back);
        }
    }

    private void RaycastTracker(Leg leg)
    {
        Vector3 origin = leg.tracker.position + Vector3.up * rayCastStartOffset;
        if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, 1000f, whatIsGround))
            leg.tracker.position = hit.point;
    }

    private void TryInitiateSteps()
    {
        LegPair pair = legPairs[currentPair];
        Leg front = pair.front;
        Leg back = pair.back;

        if (pair.isStepping)
        {
            stepT = Mathf.Clamp01(stepT + 5f * Time.deltaTime);
            float height = Mathf.Sin(stepT * Mathf.PI) * stepHeight;

            front.fabrik.Target.position = Vector3.Lerp(front.stepStartPos, front.tracker.position + Vector3.up * height, stepT);
            back.fabrik.Target.position = Vector3.Lerp(back.stepStartPos, back.tracker.position + Vector3.up * height, stepT);
        }
        else if (LegNeedsStep(front) || LegNeedsStep(back))
        {
            front.stepStartPos = front.fabrik.Target.position;
            back.stepStartPos = back.fabrik.Target.position;
            stepT = 0f;
            pair.isStepping = true;
        }

        if (stepT >= 1f)
        {
            pair.isStepping = false;
            currentPair = (currentPair + 1) % legPairs.Count;
        }
    }

    private bool LegNeedsStep(Leg leg)
    {
        return Vector3.Distance(leg.fabrik.Target.position, leg.tracker.position) >= distanceMoveThreshold;
    }

    // -------------------------------------------------------------------------
    // Body Positioning & Rotation
    // -------------------------------------------------------------------------

    private void PositionBody()
    {
        Vector3 avg = Vector3.zero;
        foreach (var pair in legPairs)
        {
            avg += pair.front.fabrik.Target.position;
            avg += pair.back.fabrik.Target.position;
        }
        avg /= legPairs.Count * 2;

        body.position = new Vector3(transform.position.x, avg.y + bodyHeightOffset, transform.position.z);
    }

    private void RotateBody()
    {
        if (Physics.Raycast(body.position, -body.up, out RaycastHit hit))
        {
            debugNormal = hit.normal;
            smoothedNormal = Vector3.Slerp(smoothedNormal, hit.normal, Time.deltaTime * 3f);
        }

        Quaternion surfaceRotation = Quaternion.FromToRotation(Vector3.up, smoothedNormal);
        body.rotation = surfaceRotation * bodyStartRotation;
    }

    // -------------------------------------------------------------------------
    // Gizmos
    // -------------------------------------------------------------------------

    private void OnDrawGizmos()
    {
        if (legPairs == null || legPairs.Count < 2) return;

        DrawLegGizmo(legPairs[0].front, Color.green);
        DrawLegGizmo(legPairs[0].back, Color.green);
        DrawLegGizmo(legPairs[1].front, Color.cyan);
        DrawLegGizmo(legPairs[1].back, Color.cyan);

        Gizmos.color = Color.green;
        Gizmos.DrawRay(body.position, debugNormal * 2f);
        Gizmos.DrawLine(transform.position, transform.position + gizmosDir * rayLength);
        Gizmos.DrawLine(transform.position + topRayOffset, transform.position + topRayOffset + gizmosDir * rayLength);
    }

    private void DrawLegGizmo(Leg leg, Color color)
    {
        if (leg?.tracker == null) return;
        Vector3 origin = leg.tracker.position + Vector3.up * rayCastStartOffset;
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(origin, 0.05f);
        Gizmos.color = color;
        Gizmos.DrawLine(origin, origin + Vector3.down * 10f);
    }
}