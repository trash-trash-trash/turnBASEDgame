using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TurnController))]
public class TurnControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        TurnController turnCont = (TurnController)target;

        DrawDefaultInspector();

        if (GUILayout.Button("End Fight"))
        {
            turnCont.EndFight();
        }

        if (GUI.changed)
        {
            serializedObject.ApplyModifiedProperties();
        }
    }
}