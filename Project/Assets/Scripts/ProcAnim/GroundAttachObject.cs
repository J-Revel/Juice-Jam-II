using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundAttachObject : MonoBehaviour
{
    
    private void Update()
    {
        transform.position = LegTargetHeight.instance.RaycastPoint(transform.position);
    }
}
