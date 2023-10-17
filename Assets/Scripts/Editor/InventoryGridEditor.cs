using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(InventoryGrid))]
public class InventoryGridEditor : Editor
{
    private bool showGridData = false;

    public override void OnInspectorGUI()
    {
        InventoryGrid IG = (InventoryGrid)target;

        DrawDefaultInspector();

        showGridData = EditorGUILayout.Foldout(showGridData, "CubeData Struct Array");

        if (showGridData)
        {
            if (IG.gridData != null)
            {
                for (int i = 0; i < IG.gridData.GetLength(0); i++)
                {
                    EditorGUILayout.LabelField($"Row {i}");
                    for (int j = 0; j < IG.gridData.GetLength(1); j++)
                    {
                        EditorGUI.BeginChangeCheck();

                        Vector2 vector2 = EditorGUILayout.Vector2Field("Position", new Vector2(IG.gridData[i, j].xPos, IG.gridData[i, j].yPos));
                        IG.gridData[i, j].accessible = EditorGUILayout.Toggle("Accessible", IG.gridData[i, j].accessible);

                        if (EditorGUI.EndChangeCheck())
                        {
                            // Handle changes
                        }
                    }
                }
            }
        }
    }
}