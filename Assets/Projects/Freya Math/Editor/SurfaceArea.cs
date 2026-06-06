using UnityEngine;

public class SurfaceArea : MonoBehaviour
{
    [SerializeField] private MeshFilter _meshFilter;
    private Vector3[] vertices;
    [SerializeField] private Color gColor;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //AreaPlane();
        //AreaCube();
        //AreaCircle();
        AreaMesh();
    }

    void AreaPlane()
    {
        vertices = _meshFilter.sharedMesh.vertices;

        float l = (vertices[0] - vertices[1]).magnitude;
        float w = (vertices[1] - vertices[2]).magnitude;

        float area = l * w;
        print("Area: " + area);
    }

    void AreaCube()
    {
        vertices = _meshFilter.sharedMesh.vertices;
        float l = (vertices[0] - vertices[1]).sqrMagnitude;
        print("Area: " + 6 * l * l);

        //int[] tris = _meshFilter.sharedMesh.triangles;

        //float w = (vertices[1] - vertices[2]).sqrMagnitude;
        //float h = 1;
        //for (int i = 0; i < vertices.Length; i ++)
        //{
        //    if (vertices[i].y != vertices[0].y)
        //    {
        //        h = Mathf.Abs(vertices[i].y - vertices[0].y);
        //    }
        //}


        //float area = (l / l) * (w / w) * h;
        //print("Area: " + area);

    }

    void AreaCircle()
    {
        vertices = _meshFilter.sharedMesh.vertices;
        print("Area: " + 4 * Mathf.PI * vertices[0].sqrMagnitude);
    }
    void AreaCapsule()
    {
        vertices = _meshFilter.sharedMesh.vertices;
        print("Area: " + 4 * Mathf.PI * vertices[0].sqrMagnitude);
    }

    void AreaMesh()
    {
        vertices = _meshFilter.sharedMesh.vertices;
        int[] tris = _meshFilter.sharedMesh.triangles;

        float areaAccum = 0;

        print("length:  " + tris.Length);
        for (int i = 0; i < tris.Length; i+=3)
        {
            Vector3 vert1 = _meshFilter.transform.TransformPoint(vertices[tris[i]]);
            Vector3 vert2 = _meshFilter.transform.TransformPoint(vertices[tris[i + 1]]);
            Vector3 vert3 = _meshFilter.transform.TransformPoint(vertices[tris[i + 2]]);

            areaAccum += Vector3.Cross(vert2 - vert1, vert3 - vert1).magnitude * 0.5f;
        }

        print("Area: " + areaAccum);
    }

    private void OnDrawGizmos()
    {
        vertices = _meshFilter.sharedMesh.vertices;
        int[] tris = _meshFilter.sharedMesh.triangles;

        for (int i = 0; i < tris.Length; i += 3)
        {
            Vector3 vert1 = vertices[tris[i]];
            Vector3 vert2 = vertices[tris[i + 1]];
            Vector3 vert3 = vertices[tris[i + 2]];


            Gizmos.color = Color.red;
            Gizmos.DrawSphere(vert1, 0.01f);
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(vert2, 0.01f);
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(vert3, 0.01f);

            Gizmos.color = gColor;
            Gizmos.DrawLine(vert1, vert2);
            Gizmos.DrawLine(vert1, vert3);
            Gizmos.DrawLine(vert3, vert2);

        }
    }
}
