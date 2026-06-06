using UnityEngine;

public class InteractScanner : MonoBehaviour
{
    [Header("Boxcast Settings")]
    [SerializeField] private Vector3 boxSize = new Vector3(2f, 2f, 1f);
    [SerializeField] private float distance = 5f;
    [SerializeField] private LayerMask interactableLayers;

    //temporary
    [SerializeField] private GameObject interactUI;

    private IInteractable current;

    public IInteractable GetBestInteractable()
    {
        Vector3 origin = transform.position;
        Vector3 direction = transform.forward;

        RaycastHit[] hits = Physics.BoxCastAll(
            origin,
            boxSize * 0.5f,         // Physics3D takes half-extents, not full size
            direction,
            transform.rotation,
            distance,
            interactableLayers
        );

        if (hits.Length == 0)
        {
            return null;
        }

        IInteractable bestInteractable = null;
        float bestScore = float.MaxValue;
        Vector3 boxCenterLineEnd = origin + direction * distance;

        foreach (RaycastHit hit in hits)
        {
            print("HIT: " + hit.collider.name);

            if (hit.collider == null)
                continue;

            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable == null)
                continue;

            Vector3 objectCenter = hit.collider.bounds.center;
            float score = DistanceToLine(origin, boxCenterLineEnd, objectCenter);

            if (score < bestScore)
            {
                bestScore = score;
                bestInteractable = interactable;
            }
        }

        if(bestInteractable != current)
        {
            current?.OffHighlight();
        }

        return bestInteractable;
    }

    private void Update()
    {
        if (GameManager.Instance.DialogueIsPlaying)
        {
            interactUI.SetActive(false);
            return;
        }

        IInteractable interactable = GetBestInteractable();
        if(interactable != null)
        {
            interactable.OnHighlight();
            current = interactable;
        }
        else
        {
            current?.OffHighlight();
            current = null;
        }

        if(current != null && Input.GetKeyDown(KeyCode.E))
        {
            current.OnInteract();
            current.OffHighlight();
        }

        interactUI.SetActive(current != null);
    }

    private float DistanceToLine(Vector3 a, Vector3 b, Vector3 point)
    {
        Vector3 ab = b - a;
        float t = Vector3.Dot(point - a, ab) / ab.sqrMagnitude;
        t = Mathf.Clamp01(t);
        Vector3 closestPoint = a + ab * t;
        return Vector3.Distance(point, closestPoint);
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 origin = transform.position;
        Vector3 direction = transform.forward;
        Vector3 endCenter = origin + direction * distance;

        float halfW = boxSize.x * 0.5f;
        float halfH = boxSize.y * 0.5f;

        Vector3 up = transform.up;
        Vector3 right = transform.right;

        Vector3 tl = -right * halfW + up * halfH;
        Vector3 tr = right * halfW + up * halfH;
        Vector3 bl = -right * halfW - up * halfH;
        Vector3 br = right * halfW - up * halfH;

        Gizmos.color = Color.green;
        DrawBox(origin, tl, tr, bl, br);

        Gizmos.color = Color.yellow;
        DrawBox(endCenter, tl, tr, bl, br);

        Gizmos.color = new Color(0f, 1f, 0f, 0.4f);
        Gizmos.DrawLine(origin + tl, endCenter + tl);
        Gizmos.DrawLine(origin + tr, endCenter + tr);
        Gizmos.DrawLine(origin + bl, endCenter + bl);
        Gizmos.DrawLine(origin + br, endCenter + br);
    }

    private void DrawBox(Vector3 center, Vector3 tl, Vector3 tr, Vector3 bl, Vector3 br)
    {
        Gizmos.DrawLine(center + tl, center + tr);
        Gizmos.DrawLine(center + bl, center + br);
        Gizmos.DrawLine(center + tl, center + bl);
        Gizmos.DrawLine(center + tr, center + br);
    }
}