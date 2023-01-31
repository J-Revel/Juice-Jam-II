using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ProceduralEffectConfig
{
    public float additionalParamValue;
    public Vector3 translation;
    public Vector3 scale;
    public Vector3 rotation;
    public float rotationOffset;
    public AnimationCurve intensityCurve;
}

[CreateAssetMenu(fileName = "New Procedural Effect", menuName = "ScriptableObjects/Procedural Effect")]
public class ProceduralEffect : ScriptableObject
{
    public ProceduralEffectConfig[] effects;
    public float animDuration;
}
