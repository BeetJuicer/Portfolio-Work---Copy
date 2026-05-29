using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Problem2 : MonoBehaviour
{
    [SerializeField] Transform A;
    [SerializeField] float maxRayDistance = 10;
    List<RaycastHit> hits = new();
    Vector3 rayStart;
    Vector3 rayDir;
    //TODO: Adjust angle in editor.
    // Update is called once per frame

    float gizmosDot;
    void Update()
    {
        hits.Clear();
        
        rayDir = A.right;
        rayStart = A.position;

        while (Physics.Raycast(rayStart, rayDir, out RaycastHit lastHit, maxRayDistance)) {
            hits.Add(lastHit);
            
            float dot = Vector3.Dot(lastHit.normal, rayDir);
            gizmosDot = dot;
            
            rayDir = rayDir - (lastHit.normal * (2 * dot));
            rayStart = lastHit.point;
        }
    }

    private void OnDrawGizmos()
    {
        Vector2 firstRayEnd = hits.Count > 0 ? hits[0].point : A.position + (Vector3)A.right * maxRayDistance;
        Gizmos.DrawLine(A.position, firstRayEnd);

        if (hits.Count <= 0)
            return;

        for (int i = 0; i < hits.Count - 1; i++)
        {
            Gizmos.DrawLine(hits[i].point, hits[i + 1].point);
        }
        //last ray with no hit
        Gizmos.DrawLine (rayStart, rayStart + rayDir * maxRayDistance);
    }
}
