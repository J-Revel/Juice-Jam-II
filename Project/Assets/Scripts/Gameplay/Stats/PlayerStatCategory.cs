using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "NewPlayerStatCategory", menuName = "ScriptableObjects/Player Stat Category")]
public class PlayerStatCategory : ScriptableObject
{
    private void OnEnable()
    {
        UpdateCurve();
    }

    public void UpdateCurve()
    {
        Keyframe[] keyframes = new Keyframe[fixedValues.Length + 2];
        keyframes[0] = new Keyframe(minLevel, minValue);
        keyframes[fixedValues.Length + 1] = new Keyframe(maxLevel, maxValue);

        for(int i=0; i<fixedValues.Length; i++)
        {
            keyframes[i+1] = new Keyframe(fixedValues[i].level, fixedValues[i].value);
        }
        curve = new AnimationCurve(keyframes);
        for(int i=0; i<fixedValues.Length+1; i++)
        {
            curve.SmoothTangents(i, smoothWeight);
            // curve.keys[i+1].inTangent = (curve.keys[i+1].value - curve.keys[i].value) / (curve.keys[i+1].time - curve.keys[i].time);
            // curve.keys[i+1].outWeight = (curve.keys[i+2].value - curve.keys[i+1].value) / (curve.keys[i+2].time - curve.keys[i+1].time);
        }
    }

    public float Evaluate(float level)
    {
        switch(interpolationMode)
        {
            case PlayerStatCategory.InterpolationMode.Linear:
                if(level <= curve.keys[0].time)
                    return curve.keys[0].value;
                for(int i=1; i<curve.keys.Length; i++)
                {
                    if(curve.keys[i].time >= level)
                    {
                        return curve.keys[i-1].value + (curve.keys[i].value - curve.keys[i-1].value) * (level - curve.keys[i-1].time) / (curve.keys[i].time - curve.keys[i-1].time);
                    }
                }
                return curve.keys[curve.keys.Length-1].value;

            case PlayerStatCategory.InterpolationMode.Smooth:
                return curve.Evaluate(level);
        }
        return 0;
    }
    
    [System.Serializable]
    public struct FixedCurvePoint{
        public int level;
        public float value;
    }
    
    public enum InterpolationMode
    {
        Smooth,
        Linear,
    }

    public enum StatGroup {
        Melee, Ranged, Dash, Movement, Health
    }

    public string categoryId;
    public StatGroup statGroup;
    public int minLevel = -100, maxLevel = 100;
    public float minValue, maxValue;
    public float smoothWeight = 1;
    public InterpolationMode interpolationMode = InterpolationMode.Smooth;
    public FixedCurvePoint[] fixedValues;
    public AnimationCurve curve = AnimationCurve.Linear(0, 0, 1, 1);
}


