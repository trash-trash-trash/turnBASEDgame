using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class InventoryGridView : MonoBehaviour
{
    //change getting each individual cube mesh to reading inventory array and making from there

    public Material openCube;

    public Material inaccessibleCube;

    public Material equippedCube;

    public Material shadowCube;

    public InventoryGrid inventoryGrid;

    public void OnEnable()
    {
        inventoryGrid.AnnounceCubeAccessibleEvent += FlipCubeAccessible;
        inventoryGrid.AnnounceCubeEquippedEvent += FlipCubeEquipped;
        inventoryGrid.AnnounceCubeShadowEvent += FlipCubeShadow;
    }

    private void FlipCubeAccessible(GameObject newObj, bool input)
    {
        MeshRenderer newMesh = newObj.GetComponent<MeshRenderer>();

        if (input)
            newMesh.material = openCube;

        else
            newMesh.material = inaccessibleCube;
    }

    private void FlipCubeEquipped(GameObject newObj, bool input)
    {
        MeshRenderer newMesh = newObj.GetComponent<MeshRenderer>();

        if (input)
            newMesh.material = equippedCube;

        else
            newMesh.material = openCube;
    }

    private void FlipCubeShadow(GameObject newObj, bool input)
    {
        MeshRenderer newMesh = newObj.GetComponent<MeshRenderer>();

        Material prevMaterial = newMesh.material;

        if (input)
            newMesh.material = shadowCube;

        else
            newMesh.material = prevMaterial;
    }


    public void OnDisable()
    {
        inventoryGrid.AnnounceCubeAccessibleEvent -= FlipCubeAccessible;
        inventoryGrid.AnnounceCubeEquippedEvent -= FlipCubeEquipped;
        inventoryGrid.AnnounceCubeShadowEvent -= FlipCubeShadow;
    }
}