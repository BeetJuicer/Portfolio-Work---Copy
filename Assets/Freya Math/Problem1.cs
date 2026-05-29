using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Problem1 : MonoBehaviour
{
    [SerializeField] Transform A;
    [SerializeField] Transform B;

    [SerializeField] float radius;
    // Update is called once per frame
    void Update()
    {
        if(IsWithinRadius(A.position, B.position, radius))
        {
            B.GetComponent<SpriteRenderer>().color = Color.red;
        }
        else
        {
            B.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(A.position, radius);
        Gizmos.DrawLine(default, A.position);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(default, B.position);
    }

    private bool IsWithinRadius(Vector3 a, Vector3 b, float radius)
    {
        // Distance
            // get the vector from a to b
            // get the magnitude of that vector

        // if distance < radius => true
        Vector3 aToB = b - a;
        float dist = Mathf.Sqrt(aToB.x * aToB.x + aToB.y * aToB.y + aToB.z * aToB.z);

        return dist <= radius;
    }
}
