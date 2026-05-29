using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class BoidSpawner : MonoBehaviour
{
    [SerializeField] GameObject originalClone;

    [SerializeField] private Vector2 positiveRange; 
    [SerializeField] private Vector2 negativeRange;

    [SerializeField] private int boidCount;
    private void Start()
    {
        SpawnBoids();
    }

    public void SpawnBoids()
    {
        for (int i = 0; i < boidCount; i++)
        {
            float xPos = Random.Range(negativeRange.x, positiveRange.x);
            float yPos = Random.Range(negativeRange.y, positiveRange.y);

            Vector2 pos = new Vector2(xPos, yPos);

            GameObject newboid = Instantiate(originalClone, pos, Quaternion.Euler(0, 0, Random.Range(0, 360f)));
        }
    }

}
