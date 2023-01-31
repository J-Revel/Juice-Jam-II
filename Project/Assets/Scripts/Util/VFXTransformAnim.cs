using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXTransformAnim : MonoBehaviour
{
    private VFXRoot root;
    public float duration = 1;
    private bool finished = false;
    public Vector3 appearRotation;
    public Vector3 appearTranslation;
    public Vector3 appearScale = Vector3.one;
    public Vector3 disappearRotation;
    public Vector3 disappearTranslation;
    public Vector3 disappearScale = Vector3.one;

    private IEnumerator Start()
    {
        root = GetComponentInParent<VFXRoot>();
        root.playingEffectsTokens++;
        root.finishedDelegate += () => {
            finished = true;
        };
        Vector3 defaultPos = transform.localPosition;
        Vector3 defaultRot = transform.localRotation.eulerAngles;
        Vector3 defaultScale = transform.localScale;
        Vector3 appearPos = defaultPos - appearTranslation;
        Vector3 appearRot = defaultRot - appearRotation;
        Vector3 disappearPos = defaultPos + disappearTranslation;
        Vector3 disappearRot = defaultRot + disappearRotation;
        for(float time = 0; time < duration; time += Time.deltaTime)
        {
            float t = time / duration;
            transform.localPosition = Vector3.Lerp(appearPos, defaultPos, t);
            transform.localRotation = Quaternion.Lerp(Quaternion.Euler(appearRot), Quaternion.Euler(defaultRot), t);
            transform.localScale = Vector3.Lerp(appearScale, defaultScale, t);
            yield return null;
            if(finished)
                break;
        }
        while(!finished)
        {
            yield return null;
        }
        for(float time = 0; time < duration; time += Time.deltaTime)
        {
            float t = time / duration;
            transform.localPosition = Vector3.Lerp(defaultPos, disappearPos, t);
            transform.localRotation = Quaternion.Lerp(Quaternion.Euler(defaultRot), Quaternion.Euler(disappearRot), t);
            transform.localScale = Vector3.Lerp(defaultScale, disappearScale, t);
            yield return null;
        }
        root.playingEffectsTokens--;
    }
}
