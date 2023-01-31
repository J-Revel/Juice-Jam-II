using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothSpline : Spline
{
    public float directionIntensities = 1;
    public float additionalDistance = 150;
    void Start()
    {
        UpdatePath();
    }

    void Update()
    {
        
    }

    public override void UpdatePath()
    {
        Vector3[] points = new Vector3[transform.childCount];
        Vector3[] directions = new Vector3[transform.childCount];
        for(int i=0; i<transform.childCount; i++)
        {
            points[i] = transform.GetChild(i).position;
            directions[i] = transform.GetChild(i).forward;
        }

        path = new Vector3[(points.Length-1) * subdivisions + 1];
        Vector3 lastPos = points[0];
        for(int i=0; i<points.Length - 1; i++)
        {
            Vector3 controlPoint = points[i];
            for(int j = 0; j < subdivisions; j++)
            {
                float f = j / (float)subdivisions;
                Vector3 A = Vector3.Lerp(points[i], points[i] + directions[i] * directionIntensities, f);
                Vector3 B = Vector3.Lerp(points[i+1] - directions[i+1] * directionIntensities, points[i+1], f);
                Vector3 C = Vector3.Lerp(A, B, f);
                path[1 + i * (subdivisions) + j] = C;
                lastPos = C;
            }
        }
        path[0] = points[0] - directions[0] * additionalDistance;
        path[path.Length - 2] = points[points.Length - 1];
        path[path.Length - 1] = points[points.Length - 1] + directions[directions.Length - 1] * additionalDistance;
    }

    
}
