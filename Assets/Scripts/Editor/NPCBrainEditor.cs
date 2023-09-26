using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NPCBrain))]
public class NPCBrainEditor : Editor
{
    public override void OnInspectorGUI()
    {
        NPCBrain brain = (NPCBrain)target;

        DrawDefaultInspector();
        
        if (GUILayout.Button("Change State"))
        {
            brain.ChangeTestState();
        }

        if (GUI.changed)
        {
            serializedObject.ApplyModifiedProperties();
        }
    }
}