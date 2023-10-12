using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[CustomEditor(typeof(InventoryGrid))]
public class InventoryGridEditor : Editor
{
    public override void OnInspectorGUI()
    {
        InventoryGrid IG = (InventoryGrid)target;

        DrawDefaultInspector();

        if (GUILayout.Button("Change Cube Accessible"))
        {
            IG.TestTargetCubesFlipAccessible();
        }

        if (GUILayout.Button("Change Cube Equipped"))
        {
            IG.TestTargetCubesFlipEquipped();
        }

        if (GUILayout.Button("Change Custom Shape Equipped"))
        {
            IG.TestCustomShapeEquip();
        }
    }
}