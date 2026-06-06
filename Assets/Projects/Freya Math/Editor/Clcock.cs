using System;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

public class Clcock : MonoBehaviour
{
    [SerializeField] private bool snapToTicks = true;
    [SerializeField] private bool use24hFormat = false;

    Vector3 secondLoc;
    
    
    private void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        Handles.matrix = transform.localToWorldMatrix;

        Handles.DrawWireDisc(Vector3.zero, Vector3.forward, 1f);
        DrawTicks();

        float secondRad = (360 - (DateTime.Now.Second * 6)) * Mathf.Deg2Rad;
        float minuteRad = (360 - (DateTime.Now.Minute * 6)) * Mathf.Deg2Rad;
        float hoursRad = (360 - (DateTime.Now.Hour * 30)) * Mathf.Deg2Rad;

        Vector3 minuteLoc = AngToVec(minuteRad);
        Vector3 hourLoc = AngToVec(hoursRad);

        Vector3 targetSecondLoc = AngToVec(secondRad);
        if(snapToTicks)
            secondLoc = Vector3.Slerp(secondLoc, targetSecondLoc, Time.deltaTime * 5f);
        else
            secondLoc = targetSecondLoc;

        Gizmos.color = Color.white;
        Gizmos.DrawLine(Vector3.zero, secondLoc);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(Vector3.zero, minuteLoc);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(Vector3.zero, hourLoc);

    }

    private void DrawTicks()
    {
        Gizmos.color = Color.black;
        int tickCount = use24hFormat ? 120 : 60;
        int angleBetweenTicks = use24hFormat ? 3 : 6;
        Vector3 start;
        Vector3 end;

        for (int i = 0; i < tickCount; i++)
        {
            Vector3 posOnCircle = AngToVec((i * angleBetweenTicks) * Mathf.Deg2Rad);
            if(i % 5 == 0)
            {
                start = posOnCircle * 0.85f;
                end = posOnCircle * 1.15f;
            }
            else
            {
                start = posOnCircle * 0.95f;
                end = posOnCircle * 1.05f;
            }

            Gizmos.DrawLine(start, end);
        }
    }

    private Vector3 AngToVec(float angleRad)
    {
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad), 0);
    }

    private void CheeseWedge()
    {
        float angleDeg = 45; //test
        float halfDegRad = (angleDeg / 2) * Mathf.Deg2Rad;
        Handles.DrawWireDisc(Vector3.zero, Vector3.forward, 1f);

        float x1 = Mathf.Cos(halfDegRad);
        float y1 = Mathf.Sin(halfDegRad);

        float x2 = Mathf.Cos(-halfDegRad);
        float y2 = Mathf.Sin(-halfDegRad);

        Handles.DrawLine(Vector3.zero, new Vector3(x1, y1, 0));
        Handles.DrawLine(Vector3.zero, new Vector3(x2, y2, 0));
    }

}
