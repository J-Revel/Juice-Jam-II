using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXDuration : MonoBehaviour
{
    private VFXRoot root;
    public float duration = 1;
    private bool finished = false;

    private IEnumerator Start()
    {
        root = GetComponentInParent<VFXRoot>();
        root.playingEffectsTokens++;
        root.finishedDelegate += () => {
            finished = true;
        };
        while(!finished)
        {
            yield return null;
        }
        yield return new WaitForSeconds(duration);
        root.playingEffectsTokens--;
    }
}
