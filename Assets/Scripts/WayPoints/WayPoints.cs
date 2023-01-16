using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoints : MonoBehaviour
{
    public WayPointComponent[] points;


    private void OnDrawGizmosSelected()
    {
        for (int i = 0; i < points.Length - 1; i++)
        {
            Debug.DrawLine( points[i].transform.position, points[i + 1].transform.position, Color.red);
        }
    }
}
