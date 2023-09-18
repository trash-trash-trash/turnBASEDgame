using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(StatsBase))]
public class StatsEditor : Editor
{

    public override void OnInspectorGUI()
    {
        StatsBase stats = (StatsBase)target;

        // Display a foldout for the statsDictionary
        EditorGUILayout.LabelField("Stats Dictionary");
        EditorGUI.indentLevel++;

        foreach (var kvp in stats.statsDictionary)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(kvp.Key.ToString(), GUILayout.Width(100));
            sbyte newValue = (sbyte)EditorGUILayout.IntField(kvp.Value);
            if (newValue != kvp.Value)
            {
                stats.statsDictionary[kvp.Key] = newValue;
            }

            EditorGUILayout.EndHorizontal();
        }

        EditorGUI.indentLevel--;

        DrawDefaultInspector();

        if (GUILayout.Button("Change Stat Value"))
        {
            stats.TestChangeStat();
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(stats);
            serializedObject.ApplyModifiedProperties();
        }

        serializedObject.Update();
        serializedObject.ApplyModifiedProperties();
    }
}