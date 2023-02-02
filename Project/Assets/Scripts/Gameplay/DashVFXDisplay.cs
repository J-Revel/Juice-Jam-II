using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashVFXDisplay : MonoBehaviour
{
    private Renderer renderer;
    private MaterialPropertyBlock propertyBlock;
    public float indexRatio;
    public Color startColor, endColor;
    public float startAnimDuration = 0.3f;
    public float endAnimDuration = 0.3f;
    IEnumerator Start()
    {
        propertyBlock = new MaterialPropertyBlock();
        renderer = GetComponent<Renderer>();
        renderer.GetPropertyBlock(propertyBlock);
        propertyBlock.SetColor("_MainColor", Color.Lerp(startColor, endColor, indexRatio));
        propertyBlock.SetFloat("_AnimRatio", 0);
        float animDuration = Mathf.Lerp(startAnimDuration, endAnimDuration, indexRatio);
        for(float t=0; t<animDuration; t+= Time.deltaTime)
        {
            propertyBlock.SetFloat("_AnimRatio", t / animDuration);
            renderer.SetPropertyBlock(propertyBlock);
            yield return null;
        }
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
