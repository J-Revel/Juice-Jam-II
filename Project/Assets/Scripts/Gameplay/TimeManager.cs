using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PauseEffect
{
    public PauseEffect() {}
    public PauseEffect(float duration) { this.duration = duration; }
    public PauseEffect(PauseEffect model) 
    {
        this.duration = model.duration;
        this.multiplier = model.multiplier;
        this.time = model.time;
        this.finished = model.finished;
    }
    
    public float duration;
    public float multiplier;
    public float time;
    public bool finished;
}

public class TimeManager : MonoBehaviour
{
    public static TimeManager instance;
    private int modalMenuDisplayCount;
    public List<PauseEffect> activePauseEffects = new List<PauseEffect>();

    public static float gameplayUnscaledDeltaTime { get { return instance.modalMenuDisplayCount > 0 ? 0 : Time.unscaledDeltaTime;}}

    void Awake()
    {
        instance = this;
    }

    public static void OnModalAppear()
    {
        instance.modalMenuDisplayCount++;
    }

    public static void OnModalDisappear()
    {
        instance.modalMenuDisplayCount--;
    }

    public static void AddPauseEffect(PauseEffect pauseEffect)
    {
        instance.activePauseEffects.Add(pauseEffect);
    }

    private void Update()
    {
        float timeScale = 1;
        for(int i = activePauseEffects.Count-1; i>=0; i--)
        {
            timeScale *= activePauseEffects[i].multiplier;
            activePauseEffects[i].time += gameplayUnscaledDeltaTime;
            if(activePauseEffects[i].finished || (activePauseEffects[i].duration > 0 && activePauseEffects[i].time >= activePauseEffects[i].duration))
                activePauseEffects.RemoveAt(i);
        }
        if(instance.modalMenuDisplayCount > 0)
            Time.timeScale = 0;
        else
            Time.timeScale = timeScale;
            
    }
}
