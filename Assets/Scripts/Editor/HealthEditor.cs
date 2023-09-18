using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Health))]
public class HealthEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Health HP = (Health)target;

        DrawDefaultInspector();

        if (GUILayout.Button("Change Health"))
        {
            HP.TestChangeHealth();
        }

        if (GUILayout.Button("Resurrect"))
        {
            HP.Resurrect();
        }

        if (GUILayout.Button("Kill"))
        {
            HP.Die();
        }

        if (GUI.changed)
        {
            serializedObject.ApplyModifiedProperties();
        }
    }
}