using UnityEngine;

public class InteractScanner : MonoBehaviour
{
    [Header("Boxcast Settings")]
    [SerializeField] private Vector2 boxSize = new Vector2(2f, 2f);
    [SerializeField] private float distance = 5f;
    [SerializeField] private LayerMask interactableLayers;

    /// <summary>
    /// Finds the interactable closest to the center line of the boxcast.
    /// Returns null if none found.
    /// </summary>
    public IInteractable GetBestInteractable()
    {
        Vector2 origin = transform.position;
        Vector2 direction = transform.right;

        RaycastHit2D[] hits = Physics2D.BoxCastAll(
            origin,
            boxSize,
            0f,
            direction,
            distance,
            interactableLayers
        );

        if (hits.Length == 0)
            return null;

        IInteractable bestInteractable = null;
        float bestScore = float.MaxValue;

        Vector2 boxCenterLineEnd = origin + direction * distance;

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider == null)
                continue;

            IInteractable interactable = hit.collider.GetComponent<IInteractable>();

            if (interactable == null)
                continue;

            // Distance from hit point to center line
            float score = DistanceToLine(
                origin,
                boxCenterLineEnd,
                hit.point
            );

            if (score < bestScore)
            {
                bestScore = score;
                bestInteractable = interactable;
            }
        }

        return bestInteractable;
    }

    /// <summary>
    /// Distance from point to line segment.
    /// </summary>
    private float DistanceToLine(Vector2 a, Vector2 b, Vector2 point)
    {
        Vector2 ab = b - a;
        float t = Vector2.Dot(point - a, ab) / ab.sqrMagnitude;
        t = Mathf.Clamp01(t);

        Vector2 closestPoint = a + ab * t;

        return Vector2.Distance(point, closestPoint);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        Vector3 origin = transform.position;
        Vector3 direction = transform.right;

        Vector3 center = origin + direction * (distance * 0.5f);

        Gizmos.matrix = Matrix4x4.TRS(
            center,
            transform.rotation,
            Vector3.one
        );

        Gizmos.DrawWireCube(
            Vector3.zero,
            new Vector3(boxSize.x, boxSize.y, 0f)
        );
    }
}
