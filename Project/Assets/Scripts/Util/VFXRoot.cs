using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXRoot : MonoBehaviour
{
    public System.Action finishedDelegate;
    public int playingEffectsTokens;
    public bool playing = true;

    public void Update()
    {
        if(!playing && playingEffectsTokens <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void StopPlaying()
    {
        playing = false;
        finishedDelegate?.Invoke();
    }
}
