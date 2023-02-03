using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public Vector3 axis = Vector3.up;
    public float rotationSpeed = 45;
    void Start()
    {
        
    }

    void Update()
    {
        transform.localRotation *= Quaternion.AngleAxis(Time.deltaTime * rotationSpeed, axis);
    }
}
