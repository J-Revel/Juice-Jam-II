using UnityEngine;
using UnityEditor;

// The icon has to be stored in Assets/Gizmos

[CustomEditor( typeof( Spline ) )]
public class DrawLineEditor : Editor
{
    void OnSceneGUI()
    {
        Spline spline = target as Spline;
        spline.UpdatePath();
        for(int i=0; i<spline.transform.childCount; i++)
        {
            spline.transform.GetChild(i).position = Handles.PositionHandle(spline.transform.GetChild(i).position, Quaternion.identity);
        }
        for(int i=0; i<spline.path.Length-1; i++)
        {
            Handles.DrawLine(spline.path[i], spline.path[i+1]);
        }
    }
}

[CustomEditor( typeof( SmoothSpline ) )]
public class SmoothSplineEditor : Editor
{
    void OnSceneGUI()
    {
        SmoothSpline spline = target as SmoothSpline;
        spline.UpdatePath();
        for(int i=0; i<spline.transform.childCount; i++)
        {
            spline.transform.GetChild(i).position = Handles.PositionHandle(spline.transform.GetChild(i).position, spline.transform.GetChild(i).rotation);
            spline.transform.GetChild(i).rotation = Handles.RotationHandle(spline.transform.GetChild(i).rotation, spline.transform.GetChild(i).position);
        }
        for(int i=0; i<spline.path.Length-1; i++)
        {
            Handles.DrawLine(spline.path[i], spline.path[i+1]);
        }
        for(int i=0; i<spline.transform.childCount; i++)
        {
            Handles.color = Color.red;
            Handles.DrawLine(spline.transform.GetChild(i).position, spline.transform.GetChild(i).position + spline.transform.GetChild(i).forward * spline.directionIntensities);
        }
    }
}