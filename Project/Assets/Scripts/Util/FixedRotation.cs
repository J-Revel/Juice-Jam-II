using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedRotation : MonoBehaviour
{
    private Quaternion startRotation;
    void Start()
    {
        startRotation = transform.rotation;
    }

    void Update()
    {
        transform.rotation = startRotation;        
    }
}
