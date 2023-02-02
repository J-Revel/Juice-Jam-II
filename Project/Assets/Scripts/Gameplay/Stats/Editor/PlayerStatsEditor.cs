using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(PlayerStatsData))]
public class PlayerStatsDataEditor : Editor
{

    PlayerStatsData data;
    static bool showTileEditor = false;

    public void OnEnable()
    {
        data = (PlayerStatsData)target;
    }

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();
        base.OnInspectorGUI();
        if(EditorGUI.EndChangeCheck())
        {
            data.UpdateStats();
        }
            // EditorGUILayout.BeginHorizontal();
            
            // EditorGUILayout.EndHorizontal();
        foreach(PlayerStatValue stat in data.stats)
        {
            EditorGUILayout.LabelField(stat.category.name + " : " + stat.level);
        }
    }
}