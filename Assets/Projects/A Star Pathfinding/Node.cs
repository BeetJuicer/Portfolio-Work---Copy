using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Node : MonoBehaviour
{
    public event Action OnSelected;
    public Node(bool isWall, List<Node> neighbors)
    {
        this.isWall = isWall;
        neighbors.ForEach(n => this.neighbors.Add(n));
    }

    SpriteRenderer r;

    [SerializeField] List<Node> neighbors = new();
    public bool isWall { get; } = false;

    public List<Node> Neighbors { get => neighbors; }

    private void Start()
    {
        r = GetComponent<SpriteRenderer>();
    }

    public void Select()
    {
        OnSelected?.Invoke();
    }

    public void ChangeColor()
    {
        if(r == null)
        {
            r = GetComponent<SpriteRenderer>();
        }

        if (isWall)
        {
            r.color = Color.red;
        }
        else
        {
            r.color = Color.green;
        }
    }

    public void SetAsPathNodeColor()
    {
        r.color = Color.blue;
    }

    private void OnDrawGizmos()
    {
        foreach (Node neighbor in neighbors)
            Gizmos.DrawLine(transform.position, neighbor.transform.position);
    }
}
