using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class NodeManager : MonoBehaviour
{
    [SerializeField] Node start;
    [SerializeField] Node goal;
    [SerializeField] GameObject car;

    private Queue<Node> frontier = new();
    private Dictionary<Node, Node> explored = new();

    private Node carPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    { 
        List<Node> all = FindObjectsByType<Node>(sortMode: FindObjectsSortMode.None).ToList();
        foreach (Node node in all)
        {
            node.OnSelected += () =>
            {
                StopCoroutine(FollowPath(new List<Node>()));
                goal = node;
                start = carPosition;
                frontier.Clear();
                explored.Clear();
                Explore();
                List<Node> path = ReconstructPath(goal);
                StartCoroutine(FollowPath(path));
            };
        }

        Explore();
        List<Node> path = ReconstructPath(goal);
        StartCoroutine(FollowPath(path));
    }

    IEnumerator FollowPath(List<Node> path)
    {
        print(path.Count);
        foreach (Node node in path)
        {
            print("going to " + node.name);
            Vector3 target = new Vector3(node.transform.position.x, node.transform.position.y, 0);
            carPosition = node;
            while (Vector3.Distance(car.transform.position, target) > 0.1f)
            {
                car.transform.position = Vector3.MoveTowards(
                car.transform.position,
                    target,
                    10f * Time.deltaTime
                );
                yield return null; // wait one frame
            }
        }
    }

    private void Explore()
    {
        frontier.Enqueue(start);
        while(frontier.Count > 0)
        {
            Node current = frontier.Dequeue();

            if (current == goal)
            {
                Debug.Log("Goal Found");
                break;
            }

            foreach (Node next in current.Neighbors)
            {
                if(!explored.ContainsKey(next) && !next.isWall)
                {
                    explored.Add(next, current);
                    frontier.Enqueue(next);
                    next.ChangeColor();
                }
            }
        }
    }

    private List<Node> ReconstructPath(Node current)
    {
        List<Node> path = new();
        while (current != start)
        {
            path.Add(current);
            current.SetAsPathNodeColor();
            current = explored[current];
        }
        path.Add(start);
        path.Reverse();
        return path;
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
