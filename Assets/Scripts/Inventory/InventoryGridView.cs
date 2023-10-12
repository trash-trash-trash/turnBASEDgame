using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryGridView : MonoBehaviour
{
    //change getting each individual cube mesh to reading inventory array and making from there

    public Material openCube;

    public Material inaccessibleCube;

    public Material equippedCube;

    public InventoryGrid inventoryGrid;

    public void OnEnable()
    {
        inventoryGrid.AnnounceCubeAccessibleEvent += FlipCubeAccessible;
        inventoryGrid.AnnounceCubeEquippedEvent += FlipCubeEquipped;
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


    public void OnDisable()
    {
        inventoryGrid.AnnounceCubeAccessibleEvent -= FlipCubeAccessible;
        inventoryGrid.AnnounceCubeEquippedEvent -= FlipCubeEquipped;
    }
}