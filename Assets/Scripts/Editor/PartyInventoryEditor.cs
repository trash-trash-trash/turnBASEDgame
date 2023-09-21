using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PartyInventory))]
public class PartyInventoryEditor : Editor
{
    public override void OnInspectorGUI()
    {
        PartyInventory partyInven = (PartyInventory)target;

        DrawDefaultInspector();

        if (GUILayout.Button("New Party Leader"))
        {
            partyInven.EquipMainMember();
        }

        if (GUI.changed)
        {
            serializedObject.ApplyModifiedProperties();
        }
    }
}