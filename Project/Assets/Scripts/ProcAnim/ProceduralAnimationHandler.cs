using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ProceduralAnimData
{
    public ProceduralAnimation config;
    public float time;
    public float intensity;
    public float animSpeed = 1;
}

[System.Serializable]
public class ProceduralEffectData
{
    public ProceduralEffect config;
    public float time;
    public float animSpeed = 1;
}

public class ProceduralAnimationHandler : MonoBehaviour
{
    public List<ProceduralAnimData> loopAnims = new List<ProceduralAnimData>();
    public List<ProceduralEffectData> effects = new List<ProceduralEffectData>();
    public Renderer[] meshRenderers;
    private MaterialPropertyBlock[] propertyBlocks;
    public string additionalShaderParam;
    private Vector3 startPosition;
    private Vector3 startScale;
    private Quaternion startRotation;
    public Vector2[] positions;
    private float startAdditionalValue;

    void Start()
    {
        startPosition = transform.localPosition;
        startScale = transform.localScale;
        startRotation = transform.localRotation;
        if(meshRenderers.Length == 0)
        {
            meshRenderers = new Renderer[1];
            meshRenderers[0] = GetComponent<Renderer>();
        }
        propertyBlocks = new MaterialPropertyBlock[meshRenderers.Length];
        for(int i=0; i<meshRenderers.Length; i++)
        {
            propertyBlocks[i] = new MaterialPropertyBlock();
            meshRenderers[i].GetPropertyBlock(propertyBlocks[i]);
        }
        startAdditionalValue = meshRenderers[0].material.GetFloat(additionalShaderParam);
    }

    public ProceduralAnimData AddAnim(ProceduralAnimation anim, float intensity, float speed = 1)
    {
        ProceduralAnimData result = new ProceduralAnimData();
        result.config = anim;
        result.intensity = intensity;
        result.animSpeed = speed;
        loopAnims.Add(result);
        return result;
    }

    public void RemoveAnim(ProceduralAnimData anim)
    {
        loopAnims.Remove(anim);
    }

    public ProceduralEffectData AddEffect(ProceduralEffect effect, float speed = 1)
    {
        for(int i=effects.Count-1; i>=0; i--)
        {
            if(effects[i].config == effect)
            {
                effects.RemoveAt(i);
            }
        }
        ProceduralEffectData result = new ProceduralEffectData();
        result.config = effect;
        result.animSpeed = speed;
        effects.Add(result);
        return result;
    }

    void Update()
    {
        double[] xPositions = new double[16]; // TODO : WHY 16 ?
        for(int i=0; i<xPositions.Length; i++)
        {
            for(int j=1; j<positions.Length; j++)
            {
                float timeRatio = i / (float)xPositions.Length;
                if(positions[j].x > timeRatio)
                {
                    xPositions[i] = positions[j-1].y + (positions[j].y - positions[j-1].y) * (timeRatio - positions[j-1].x) / (positions[j].x - positions[j-1].x);
                    break;
                }
            }
        }
        bool hasTranslation = false;
        bool hasRotation = false;
        bool hasScale = false;
        Vector3 translation = Vector3.zero;
        Vector3 rotation = Vector3.zero;
        Vector3 scale = Vector3.one;
        float additionalValue = startAdditionalValue;
        float colorWeightSum = 1;
        foreach(ProceduralAnimData anim in loopAnims)
        {
            foreach (ProceduralAnimConfig animConfig in anim.config.anims)
            {
                anim.time += Time.deltaTime * anim.animSpeed;
                float f = Mathf.Sin((anim.time / anim.config.animDuration + animConfig.offsetRatio) * 2 * Mathf.PI);
                translation += (animConfig.translationOffset + f) * animConfig.translation * anim.intensity;
                if(animConfig.translation != Vector3.zero)
                    hasTranslation = true;
                rotation += (animConfig.rotationOffset + f) * animConfig.rotation * anim.intensity;
                if(animConfig.rotation != Vector3.zero)
                    hasRotation = true;
                scale = Vector3.Scale(scale, Vector3.one + (animConfig.scaleOffset + f) * animConfig.scale * anim.intensity);
                if(animConfig.scale != Vector3.zero)
                    hasScale = true;
                additionalValue += animConfig.additionalParamValue * f * anim.intensity;
                colorWeightSum += f * anim.intensity;
            }
        }
        foreach(ProceduralEffectData effect in effects)
        {
            foreach (ProceduralEffectConfig effectConfig in effect.config.effects)
            {
                effect.time += Time.deltaTime * effect.animSpeed;
                float f = effectConfig.intensityCurve.Evaluate(effect.time / effect.config.animDuration);
                translation += f * effectConfig.translation;
                if(effectConfig.translation != Vector3.zero)
                    hasTranslation = true;
                rotation += f * effectConfig.rotation;
                if(effectConfig.rotation != Vector3.zero)
                    hasRotation = true;
                scale = Vector3.Scale(scale, Vector3.one + f * effectConfig.scale);
                if(effectConfig.scale != Vector3.zero)
                    hasScale = true;
                additionalValue += effectConfig.additionalParamValue * f;
                colorWeightSum += f;
            }
        }
        for(int i=effects.Count-1; i>=0; i--)
        {
            if(effects[i].time >= effects[i].config.animDuration)
                effects.RemoveAt(i);
        }
        if(hasTranslation)
            transform.localPosition = startPosition + translation;
        if(hasRotation)
            transform.localRotation = startRotation * Quaternion.Euler(rotation.x, rotation.y, rotation.z);
        if(hasScale)
            transform.localScale = Vector3.Scale(startScale, scale);

        for(int i=0; i<meshRenderers.Length; i++)
        {
            meshRenderers[i].GetPropertyBlock(propertyBlocks[i]);
            propertyBlocks[i].SetFloat(additionalShaderParam, additionalValue);
            meshRenderers[i].SetPropertyBlock(propertyBlocks[i]);            
        }
    }
}
