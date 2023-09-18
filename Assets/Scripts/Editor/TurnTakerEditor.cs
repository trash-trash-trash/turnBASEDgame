using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TurnTaker))]
public class TurnTakerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        TurnTaker turnTaker = (TurnTaker)target;

        DrawDefaultInspector();

        if (GUILayout.Button("End Turn"))
        {
            turnTaker.EndTurn();
        }

        if (GUI.changed)
        {
            serializedObject.ApplyModifiedProperties();
        }
    }
}
