using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(PlayerStatCategory))]
public class PlayerStatCategoryEditor : Editor
{

    PlayerStatCategory data;
    static bool showTileEditor = false;

    public void OnEnable()
    {
        data = (PlayerStatCategory)target;
    }

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();
        base.OnInspectorGUI();
        for (int i = data.minLevel; i <= data.maxLevel; i++)
        {
            float value = data.Evaluate(i);
            EditorGUILayout.LabelField("Level " + i + " : " + value);
        }
        if(EditorGUI.EndChangeCheck())
        {
            data.UpdateCurve();
        }
    }
}