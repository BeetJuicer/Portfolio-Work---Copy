using UnityEngine;

public class RoomVisuals : MonoBehaviour
{
    [SerializeField] private GameObject rightWall;
    [SerializeField] private GameObject leftWall;
    [SerializeField] private GameObject topWall;
    [SerializeField] private GameObject bottomWall;

    void Awake()
    {
        SetWalls(false, false, false, false);
    }

    public void SetWallActive(Direction direction, bool active)
    {
        switch (direction)
        {
            case Direction.East:
                rightWall.SetActive(active);
                break;
            case Direction.West:
                leftWall.SetActive(active);
                break;
            case Direction.North:
                topWall.SetActive(active);
                break;
            case Direction.South:
                bottomWall.SetActive(active);
                break;
        }
    }

    public void SetWalls(bool right = false, bool left = false, bool top = false, bool bottom = false)
    {
        rightWall.SetActive(right);
        leftWall.SetActive(left);
        topWall.SetActive(top);
        bottomWall.SetActive(bottom);
    }
}
