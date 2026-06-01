using UnityEngine;

public class LineIntersectionTest : MonoBehaviour
{

    [SerializeField] private Transform p1Test;
    [SerializeField] private Transform p2Test;
    [SerializeField] private Transform p3Test;
    [SerializeField] private Transform p4Test;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        bool intersects = LinesIntersect((p1Test.position, p2Test.position), (p3Test.position, p4Test.position));

        float sphereRadius = 0.01f;

        Gizmos.color = intersects ? Color.red : Color.white;
        Gizmos.DrawLine(p1Test.position, p2Test.position);
        Gizmos.DrawLine(p3Test.position, p4Test.position);

        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(p1Test.position, sphereRadius);
        Gizmos.DrawSphere(p2Test.position, sphereRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(p3Test.position, sphereRadius);
        Gizmos.DrawSphere(p4Test.position, sphereRadius);
    }

    private bool LinesIntersect((Vector3, Vector3) line1, (Vector3, Vector3) line2)
    {
        float wedge1 = WedgeProduct(line1.Item1 - line1.Item2, line2.Item1 - line1.Item1);
        float wedge2 = WedgeProduct(line1.Item1 - line1.Item2, line2.Item2 - line1.Item1);
        float wedge3 = WedgeProduct(line2.Item2 - line2.Item1, line1.Item1 - line2.Item1);
        float wedge4 = WedgeProduct(line2.Item2 - line2.Item1, line1.Item2 - line2.Item1);

        Debug.Log($"wedge1: {wedge1}, wedge2: {wedge2}, wedge3: {wedge3}, wedge4: {wedge4}");

        bool pair1Intersects = (wedge1 == 0 || wedge2 == 0) ? false : Mathf.Sign(wedge1) != Mathf.Sign(wedge2);
        bool pair2Intersects = (wedge3 == 0 || wedge4 == 0) ? false : Mathf.Sign(wedge3) != Mathf.Sign(wedge4);

        return pair1Intersects && pair2Intersects;
    }
    private float WedgeProduct(Vector3 d1, Vector3 d2)
    {
        print("d1: " + d1 + ", d2: " + d2);

        return (d1.x * d2.y) - (d1.y * d2.x);
    }

}
