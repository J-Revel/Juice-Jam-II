using System.Collections.Generic;
using UnityEngine;
using System;

public enum StatValueType
{
    AbsoluteValue,
    Multiplier,
    Divider,
}

[System.Serializable]
public class StatEvaluatorConfig
{
    public float defaultValue = 1;
    public PlayerStatCategory category;
    public StatValueType type;
}

[System.Serializable]
public class StatEvaluator : StatEvaluatorConfig
{
    private float currentValue;
    private float lastUpdateTime = -1;
    private bool initialized = false;

    public float value
    { 
        get
        {
            if(!initialized || lastUpdateTime < PlayerStatsData.instance.lastUpdateTime)
            {
                initialized = true;
                UpdateValue();
            }
            return currentValue;
        }
    }

    private void UpdateValue()
    {
        lastUpdateTime = PlayerStatsData.instance.lastUpdateTime;
        if(category == null)
        {
            currentValue = defaultValue;
            return;
        }
        float statValue = PlayerStatsData.instance.GetStatValue(category);
        switch(type)
        {
            case StatValueType.AbsoluteValue:
                currentValue = statValue;
                break;
            case StatValueType.Multiplier:
                currentValue = defaultValue * statValue;
                break;
            case StatValueType.Divider:
                currentValue = defaultValue / statValue;
                break;
            default:
                currentValue = defaultValue;
                break;
        }
    }

}


