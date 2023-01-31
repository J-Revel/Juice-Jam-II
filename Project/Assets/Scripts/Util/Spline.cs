using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spline : MonoBehaviour
{
    public int subdivisions = 5;
    public Vector3[] path;
    void Start()
    {
        UpdatePath();
    }

    void Update()
    {
        
    }

    public virtual void UpdatePath()
    {
        Vector3[] points = new Vector3[transform.childCount];
        for(int i=0; i<transform.childCount; i++)
        {
            points[i] = transform.GetChild(i).position;
        }

        path = new Vector3[(points.Length / 2) * (subdivisions+1)];
        Vector3 lastPos = points[0];
        for(int i=0; i<(points.Length - 1)/2; i+=1)
        {
            for(int j = 0; j <= subdivisions; j++)
            {
                float f = j / (float)subdivisions;
                Vector3 A = Vector3.Lerp(points[i*2], points[i*2+1], f);
                Vector3 B = Vector3.Lerp(points[i*2+1], points[i*2+2], f);
                Vector3 C = Vector3.Lerp(A, B, f);
                path[i * (subdivisions+1) + j] = C;
                lastPos = C;
            }
        }
    }
    
    public Vector3 GetPoint(int index, bool reversed)
    {
        if(reversed)
            return path[path.Length - 2 - index];
        return path[index + 1];
    }

    public int pathLength {
        get {
            return path.Length - 1;
        }
    }
}
