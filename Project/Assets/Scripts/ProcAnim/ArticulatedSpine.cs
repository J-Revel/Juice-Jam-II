using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArticulatedSpine : MonoBehaviour
{
    public Transform[] bodyParts;
    private Vector3[] bodyPartOffsets;
    private Vector3[] previousPositions;
    void Start()
    {
        previousPositions = new Vector3[bodyParts.Length];
        bodyPartOffsets = new Vector3[bodyParts.Length];
        for(int i=0; i<bodyPartOffsets.Length; i++)
        {
            bodyPartOffsets[i] = bodyParts[i].localPosition;
            previousPositions[i] = bodyParts[i].position;
        }
    }

    void Update()
    {
        for(int i=1; i<2; i++)
        {
            Vector3 newPosition = bodyParts[i-1].transform.TransformPoint(bodyPartOffsets[i]);
            Vector3 parentDirection = newPosition - bodyParts[i].transform.position;
            if(parentDirection != Vector3.zero)
            {
                Debug.DrawLine(newPosition, bodyParts[i].transform.position, Color.red, 2);
                bodyParts[i].transform.rotation = Quaternion.LookRotation(parentDirection, Vector3.up);
                bodyParts[i].transform.position = newPosition;
            }
        }
    }
}
