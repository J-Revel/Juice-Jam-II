using System.Collections.Generic;
using UnityEngine;
using System;

public enum CharacterVariableType
{
    Poise,
    Stun,
}

[System.Serializable]
public struct CharacterVariableConfig
{
    public float min, max;
    public float startValue;
    public float changeOverTime;
}

[System.Serializable]
public class CharacterVariable
{
    public CharacterVariableType variable;
    public CharacterVariableConfig config;
    
    [HideInInspector]
    public float value;
}

public class CharacterVariableHandler : MonoBehaviour
{
    public List<CharacterVariable> variables = new List<CharacterVariable>();

    private void Start()
    {
        foreach(CharacterVariable variable in variables)
        {
            variable.value = variable.config.startValue;
        }
    }

    private void Update()
    {
        foreach(CharacterVariable variable in variables)
        {
            variable.value = Mathf.Clamp(variable.value + Time.deltaTime * variable.config.changeOverTime, variable.config.min, variable.config.max);
        }
    }

    public void SetVariable(CharacterVariableType variableType, float value)
    {
        foreach(CharacterVariable variable in variables)
        {
            if(variable.variable == variableType)
                variable.value = value;
        }
    }

    public void AddToVariable(CharacterVariableType variableType, float value)
    {
        foreach(CharacterVariable variable in variables)
        {
            if(variable.variable == variableType)
                variable.value = Mathf.Clamp(variable.value + value, variable.config.min, variable.config.max);
        }
    }

    public float GetValue(CharacterVariableType variableType, float defaultValue = 0)
    {
        foreach(CharacterVariable variable in variables)
        {
            if(variable.variable == variableType)
                return variable.value;
        }
        return defaultValue;
    }
}
