using UnityEngine;
using UnityEngine.Splines;
using Unity.Mathematics;

[RequireComponent(typeof(EdgeCollider2D))]
public class SplineColliderGenerator : MonoBehaviour
{
    [SerializeField] private SplineContainer splineContainer;
    [SerializeField] private int resolution = 60;      // points sampled along spline
    [SerializeField] private float width = 2f;         // how thick the wind tunnel is

    private void Start()
    {
        GenerateCollider();
    }

    private void GenerateCollider()
    {
        //splineContainer.Spline.
    }

#if UNITY_EDITOR
    private void OnValidate() => UnityEditor.EditorApplication.delayCall += () =>
    {
        if (this != null) GenerateCollider();
    };
#endif
}