using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LegMeshPlacer : MonoBehaviour
{
    public float meshLength = 2;
    public Vector3 scaleAxes = new Vector3(0, 0, 1);
    public Vector3 startScale;
    void Start()
    {
        
    }

    void Update()
    {
        transform.position = (transform.parent.position + transform.parent.parent.position) / 2;
        transform.rotation = Quaternion.LookRotation(transform.parent.parent.position - transform.parent.position);
        transform.localScale = Vector3.Scale(startScale, (scaleAxes * (transform.parent.parent.position - transform.parent.position).magnitude / meshLength + (Vector3.one - scaleAxes)));
    }
}
