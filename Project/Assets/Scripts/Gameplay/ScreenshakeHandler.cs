using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ScreenshakeEffect
{
    public ScreenshakeEffect() {}
    public ScreenshakeEffect(ScreenshakeEffect model) {
        time = model.time;
        duration = model.duration;
        translationIntensity = model.translationIntensity;
        rotationIntensity = model.rotationIntensity;
    }
    [HideInInspector]
    public float time;
    public float duration;
    public float translationIntensity;
    public float rotationIntensity;
}
public class ScreenshakeHandler : MonoBehaviour
{
    public static ScreenshakeHandler instance;
    public List<ScreenshakeEffect> activeEffects = new List<ScreenshakeEffect>();
    public float maxRotation = 5;
    public float maxTranslation = 1;
    public float debugRotationIntensity = 0;
    public float debugTranslationIntensity = 0;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        
    }

    void Update()
    {
        float translationIntensity = debugTranslationIntensity;
        float rotationIntensity = debugRotationIntensity;
        if(TimeManager.gameplayUnscaledDeltaTime > 0)
        {
            for(int i=activeEffects.Count-1; i>=0; i--)
            {
                ScreenshakeEffect effect = activeEffects[i];
                float t = effect.time / effect.duration;
                effect.time += Time.deltaTime;
                translationIntensity += effect.translationIntensity * (1-(1-t)*(1-t));
                rotationIntensity += effect.rotationIntensity * (1-(1-t)*(1-t));
                if(effect.time > effect.duration)
                    activeEffects.RemoveAt(i);
            }
            float angle = Random.Range(-maxRotation, maxRotation) * Mathf.Clamp01(rotationIntensity);
            float dx = Random.Range(-maxTranslation, maxTranslation) * Mathf.Clamp01(translationIntensity);
            float dy = Random.Range(-maxTranslation, maxTranslation) * Mathf.Clamp01(translationIntensity);
            transform.localRotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.localPosition = new Vector3(dx, dy, 0);
        }
    }
}
