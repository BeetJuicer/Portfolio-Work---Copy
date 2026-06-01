using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BezierCurve))]
public class BezierCurveEditor : Editor
{
    private void OnSceneGUI()
    {
        BezierCurve bezierCurve = (BezierCurve)target;

        Event e = Event.current;

        Handles.BeginGUI();
        if (GUI.Button(new Rect(10, 10, 150, 30), "+ Add Anchor Point"))
        {
            Undo.RecordObject(bezierCurve, "Add Anchor Point");
            bezierCurve.AddAnchorPoint();
            bezierCurve.GenerateBezier();
            if(bezierCurve.TryGetComponent<BezierMesh>(out BezierMesh bezierMesh))
            {
                bezierMesh.GenerateMesh();
            }
        }
        Handles.EndGUI();

        for (int i = 0; i < bezierCurve.controlAndAnchorPoints.Count; i++)
        {
            float size = HandleUtility.GetHandleSize(bezierCurve.controlAndAnchorPoints[i]) * 0.1f;

            EditorGUI.BeginChangeCheck();
            
            Handles.color = i % 3 == 0? Color.red : Color.green;
            Vector3 newPos = Handles.FreeMoveHandle(
                bezierCurve.controlAndAnchorPoints[i],
                size,
                Vector3.zero,
                Handles.SphereHandleCap
            );

            Handles.color = Color.white;
            if (i < bezierCurve.controlAndAnchorPoints.Count - 1)
            {
                Handles.DrawLine(bezierCurve.controlAndAnchorPoints[i], bezierCurve.controlAndAnchorPoints[i + 1]);
            }

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(bezierCurve, "Move Control/Anchor Point");
                bezierCurve.controlAndAnchorPoints[i] = newPos;
                bezierCurve.GenerateBezier();
                if (bezierCurve.TryGetComponent<BezierMesh>(out BezierMesh bezierMesh))
                {
                    bezierMesh.GenerateMesh();
                }
            }
        }
    }
}
    