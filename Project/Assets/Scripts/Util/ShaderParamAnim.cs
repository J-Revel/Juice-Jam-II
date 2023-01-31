using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderParamAnim : MonoBehaviour
{
    public float duration;
    public string shaderParamName;
    private MaterialPropertyBlock propertyBlock;
    private new Renderer renderer;
    void Start()
    {
        propertyBlock = new MaterialPropertyBlock();
        renderer.GetPropertyBlock(propertyBlock);
    }

    void Update()
    {
        
    }
}
