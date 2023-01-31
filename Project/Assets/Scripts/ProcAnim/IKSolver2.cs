using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKSolver2 : MonoBehaviour
{
    public WalkingLeg target;
    public float[] segmentLengths;
    private Vector3[] points;
    public float[] reverseAngles;
    public bool update;
    public Color[] colors;
    public Transform pointPrefab;
    private Transform[] pointElementInstances;
    public Transform segmentPrefab;
    private Transform[] segmentElementInstances;
    void Start()
    {
        points = new Vector3[segmentLengths.Length + 1];
        Vector3 cursor = Vector3.zero;
        for(int i=0; i<points.Length-1; i++)
        {
            cursor += Vector3.right * segmentLengths[i];
            points[i] = cursor;
        }
        points[points.Length-1] = cursor;
        pointElementInstances = new Transform[points.Length];
        for(int i=0; i<points.Length; i++)
        {
            pointElementInstances[i] = Instantiate(pointPrefab, points[i], Quaternion.identity, transform);
        }
        segmentElementInstances = new Transform[points.Length - 1];
        for(int i=0; i<segmentElementInstances.Length; i++)
        {
            segmentElementInstances[i] = Instantiate(segmentPrefab, points[i], Quaternion.identity, transform);
        }
    }

    void Update()
    {
        Vector3 previousPoint = Vector3.zero;
        if(update)
        {
            Reach();
            SolveConstraints();
            ReachReverse();
            UpdateDisplay();
            // update = false;
        }
        for(int i=0; i<points.Length; i++)
        {
            Debug.DrawLine(points[i], previousPoint, colors[i%colors.Length], 0);
            previousPoint = points[i];
        }
    }

    public void Reach()
    {
        Vector3 targetPos = transform.InverseTransformPoint(target.footPos);
        for(int i=0; i<points.Length; i++)
            points[i] = Vector3.ProjectOnPlane(points[i], Vector3.Cross(targetPos, Vector3.up));
        Vector3 targetPoint = targetPos;
        float targetDistance = 0;
        for(int i=points.Length - 1; i>=0; i--)
        {
            Vector3 targetDirection = (targetPoint - points[i]).normalized;
            points[i] = targetPoint - targetDirection * targetDistance;
            if(i > 0)
                targetDistance = segmentLengths[i-1];
            targetPoint = points[i];
        }
    }
    public void ReachReverse()
    {
        Vector3 targetPoint = Vector3.zero;
        float targetDistance = 0;
        for(int i=0; i<points.Length; i++)
        {
            Vector3 targetDirection = (targetPoint - points[i]).normalized;
            points[i] = targetPoint - targetDirection * targetDistance;
            if(i < segmentLengths.Length)
                targetDistance = segmentLengths[i];
            targetPoint = points[i];
        }
    }

    public void SolveConstraints()
    {
        Vector3 targetPos = transform.InverseTransformPoint(target.footPos);
        Vector3 previousDirection = target.footPos;
        for(int i=0; i<points.Length-1; i++)
        {
            Vector3 nextDirection = points[i+1] - points[i];
            Vector3 cross = Vector3.Cross(nextDirection, previousDirection);
            float dot = Vector3.Dot(cross, Vector3.Cross(targetPos, Vector3.up));
            if(reverseAngles.Length > i)
                dot *= reverseAngles[i];
            if(dot < 0)
            {
                points[i] = points[i] + nextDirection - previousDirection;
                // Debug.DrawLine(points[i], points[i] + nextDirection - previousDirection, colors[i%colors.Length], 0);
            }
            // Debug.DrawLine(points[i], points[i] + cross, colors[i%colors.Length], 0);
            previousDirection = nextDirection;
        }
    }

    public void UpdateDisplay()
    {
        for(int i=0; i<points.Length; i++)
        {
            pointElementInstances[i].localPosition = points[i];
        }
        for(int i=0; i<segmentElementInstances.Length; i++)
        {
            segmentElementInstances[i].localPosition = (points[i] + points[i+1])/2;
            segmentElementInstances[i].localRotation = Quaternion.LookRotation(points[i] - points[i+1]);
            Vector3 parentScale = new Vector3(1/transform.lossyScale.x, 1/transform.lossyScale.y, 1/transform.lossyScale.z);
            segmentElementInstances[i].localScale = Vector3.Scale(new Vector3(segmentPrefab.localScale.x, segmentPrefab.localScale.y, segmentPrefab.localScale.z * (points[i] - points[i+1]).magnitude), parentScale);
        }
    }
}
