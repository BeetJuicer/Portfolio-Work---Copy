using UnityEngine;

namespace AStar
{

public class PathCreator : MonoBehaviour
{
    [HideInInspector]
    public Path path;

    public bool isClosed = false;
    public void CreatePath()
    {
        print("Creating new path.");
        path = new Path(transform.position, isClosed);
    }

    public void ToggleClosed()
    {
        path.ToggleClosed();
    }
}

}