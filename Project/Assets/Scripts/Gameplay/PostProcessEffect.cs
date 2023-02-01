using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class PostProcessEffect : MonoBehaviour
{
    private LensDistortion lensDistortion;
    private Vignette vignette;
    private float initialLensDistortion;
    public float lensDistortionIntensity = 0.5f; 
    public float duration = 0.5f;
    public Color vignetteColor = Color.red;
    public float vignetteIntensity = 0.7f;
    private float initialVignetteIntensity;
    private Color initialVignetteColor;
    public ScreenshakeEffect screenshake;
    void Start()
    {
        Volume volume = gameObject.GetComponent<Volume>();
        
 
        if(volume.profile.TryGet<LensDistortion>( out lensDistortion ) )
        {
            initialLensDistortion = lensDistortion.intensity.value;
        }
        if(volume.profile.TryGet<Vignette>( out vignette ))
        {
            initialVignetteIntensity = vignette.intensity.value;
            initialVignetteColor = vignette.color.value;
        }
    }

    public void Play()
    {
        StartCoroutine(HitStop(10));
        StartCoroutine(PlayEffect());
    }

    IEnumerator HitStop(int frameCount)
    {
        Time.timeScale = 0;
        for(int i=0; i<frameCount; i++)
            yield return null;
        Time.timeScale = 1;
    }

    IEnumerator PlayEffect()
    {
        ScreenshakeHandler.instance.activeEffects.Add(new ScreenshakeEffect(screenshake));
        for(float time=0; time < duration; time += Time.unscaledDeltaTime)
        {
            float f = 1 - (1-time / duration) * (1-time / duration);
            lensDistortion.intensity.value = Mathf.Lerp(lensDistortionIntensity, initialLensDistortion, f);
            vignette.color.value = Color.Lerp(vignetteColor, initialVignetteColor, f);
            vignette.intensity.value = Mathf.Lerp(vignetteIntensity, initialVignetteIntensity, f);
            yield return null;
        }
        Time.timeScale = 1;
        lensDistortion.intensity.value = initialLensDistortion;
        vignette.color.value = initialVignetteColor;
        vignette.intensity.value = initialVignetteIntensity;
    }
}
