using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ProceduralAnimConfig
{
    public Vector3 translation;
    public float translationOffset;
    public Vector3 scale;
    public float scaleOffset;
    public Vector3 rotation;
    public float rotationOffset;
    public float offsetRatio;
    public float additionalParamValue;
}

[CreateAssetMenu(fileName = "New Procedural Animation", menuName = "ScriptableObjects/Procedural Animation")]
public class ProceduralAnimation : ScriptableObject
{
    public ProceduralAnimConfig[] anims;
    public float animDuration;
}
