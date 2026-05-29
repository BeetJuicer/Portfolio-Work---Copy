using System.Collections.Generic;
using UnityEngine;
namespace AStar
{

[System.Serializable]
public class Path
{
    [SerializeField]
    private List<Vector2> points = new();
    public List<Vector2> Points { get { return points; } }
    public int PointCount { get { return points.Count; } }
    bool isClosed = false;

    public Path(Vector2 center, bool isClosed)
    {
        points = new List<Vector2>()
        {
            center + Vector2.left,
            center + Vector2.right,
            center + 0.5f * (Vector2.left + Vector2.up),
            center + 0.5f * (Vector2.right + Vector2.down),
        };
        this.isClosed = isClosed;
    }

    public void AddSegment(Vector2 newAnchorPoint)
    {
        Vector2 lastAnchor = points[points.Count - 1];
        Vector2 lastControl = points[points.Count - 2];
        Vector2 lastControlTwin = lastAnchor + (lastAnchor - lastControl);
        points.Add(lastControlTwin);

        Vector2 newControlPoint = (lastControlTwin + newAnchorPoint) / 2;
        points.Add(newControlPoint);

        points.Add(newAnchorPoint);
    }

    public Vector2[] GetPointsInSegment(int segment)
    {
        int startPoint = segment * 3;
        return new Vector2[] {
            points[startPoint],
            points[startPoint + 1],
            points[startPoint + 2],
            points[LoopIndex(startPoint + 3)]
        };
    }

    public int GetSegmentCount()
    {
        return points.Count / 3;
    }

    public void MovePoint(int i, Vector2 newPos)
    {
        if (isAnchorPoint(i))
        {
            Vector2 deltaMove = newPos - points[i];
            if (isClosed || i + 1 < PointCount)
            {
                points[LoopIndex(i + 1)] += deltaMove;
            }
            if (isClosed || i - 1 >= 0)
            {
                points[LoopIndex(i - 1)] += deltaMove;
            }
        }
        else if (isNotLoneControlPoint(i))
        {
            int anchorIndex = getNeighbourAnchorIndex(i);
            int twinControl = getTwinControlIndex(i, anchorIndex);

            Vector2 dirFromAnchor = (newPos - points[anchorIndex]).normalized;
            float twinDistanceFromAnchor = Vector2.Distance(points[twinControl], points[anchorIndex]);

            points[twinControl] = points[anchorIndex] - dirFromAnchor * twinDistanceFromAnchor;
        }
        points[i] = newPos;
    }

    public void ToggleClosed()
    {
        isClosed = !isClosed;

        if (isClosed)
        {
            Vector2 lastAnchor = points[points.Count - 1];
            Vector2 lastControl = points[points.Count - 2];
            Vector2 lastControlTwin = lastAnchor + (lastAnchor - lastControl);
            points.Add(lastControlTwin);

            Vector2 firstAnchor = points[0];
            Vector2 firstControl = points[1];
            Vector2 firstControlTwin = firstAnchor + (firstAnchor - firstControl);
            points.Add(firstControlTwin);
        }
        else
        {
            points.RemoveAt(points.Count - 1);
            points.RemoveAt(points.Count - 1);
        }
    }

    private bool isAnchorPoint(int i)
    {
        return i % 3 == 0;
    }

    private bool isNotLoneControlPoint(int i)
    {
        return isClosed || (i > 1 && i < PointCount - 2);
    }

    private int getNeighbourAnchorIndex(int i)
    {
        int prev = LoopIndex(i - 1);
        int next = LoopIndex(i + 1);
        return isAnchorPoint(prev) ? prev : next;
    }

    private int getTwinControlIndex(int i, int anchorIndex)
    {
        bool anchorIsBehind = LoopIndex(anchorIndex) == LoopIndex(i - 1);
        if (anchorIsBehind)
        {
            return LoopIndex(i - 2);
        }
        else
        {
            return LoopIndex(i + 2);
        }
    }

    private int LoopIndex(int i)
    {
        return (i + PointCount) % PointCount;
    }
}
}