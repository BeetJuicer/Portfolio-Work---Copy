using UnityEngine;
using UnityEditor;
using System.Drawing.Printing;
namespace AStar
{

[CustomEditor(typeof(PathCreator))]
public class PathEditor : Editor
{
    PathCreator creator;
    Path path;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if(GUILayout.Button("Create New Path"))
        {
            Undo.RecordObject(creator, "Create New Path");
            creator.CreatePath();
            path = creator.path;
        }

        if(GUILayout.Button("Toggle Closed"))
        {
            Undo.RecordObject(creator, "Toggle Closed");
            creator.ToggleClosed();
        }
    }

    private void OnEnable()
    {
        creator = (PathCreator)target;
        Debug.Log("OnEnable ran, creator is: " + creator);
        Debug.Log("OnEnable ran, creator.path is: " + creator.path);
        if (creator.path == null || creator.path.PointCount == 0)
        {
            creator.CreatePath();
        }
        path = creator.path;

        Debug.Log("OnEnable ran, path is: " + path);
    }

    private void OnSceneGUI()
    {
        if (path == null) return;
        Debug.Log(path.PointCount);
        Input();
        Draw();
    }

    private void Input()
    {
        Event guiEvent = Event.current;
        if (guiEvent != null)
        {
            if(guiEvent.type == EventType.MouseDown && guiEvent.button == 0 && guiEvent.shift)
            {
                Undo.RecordObject(creator, "Add Segment");
                Vector2 mousePos = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition).origin;
                path.AddSegment(mousePos);
            }
        }
    }

    private void Draw()
    {
        
        int segmentCount = path.GetSegmentCount();
        Debug.Log("segment count: " + segmentCount);
        for (int i = 0; i < segmentCount; i++)
        {
            Debug.Log("segment: " + i);
            Vector2[] points = path.GetPointsInSegment(i);

            //lines between the anchor and control points
            Handles.color = Color.black;
            Handles.DrawLine(points[1], points[0]);
            Handles.DrawLine(points[2], points[3]);

            Handles.color = Color.green;
            Handles.DrawBezier(points[0], points[3], points[1], points[2], Color.green, null, 2);
        }


        Handles.color = Color.red;
        for (int i = 0; i < path.PointCount; i++)
        {
            Vector2 newPos = Handles.FreeMoveHandle(path.Points[i], .1f, Vector2.zero, Handles.CylinderHandleCap);
            if (path.Points[i] != newPos)
            {
                Undo.RecordObject(creator, "Move Point");


                //move anchor
                path.MovePoint(i, newPos);
            }
        }
    }
}

}