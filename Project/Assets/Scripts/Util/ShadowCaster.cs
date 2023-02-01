using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class ShadowCaster : MonoBehaviour
{
    void Start()
    { 
        Renderer renderer = GetComponent<Renderer>();
        renderer.receiveShadows = true;
        renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.TwoSided;
    }
}
