using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class InventoryGridView : MonoBehaviour
{
    //change getting each individual cube mesh to reading inventory array and making from there

    public Material openCube;

    public Material occupiedCube;

    public Material shadowCube;

    public Material blockedCube;

    public InventoryGrid inventoryGrid;

    private Dictionary<InventoryGrid.GridCubeType, Material> materialDict;

    public void OnEnable()
    {
        if (materialDict == null)
        {
            materialDict = new Dictionary<InventoryGrid.GridCubeType, Material>();

            materialDict.Add(InventoryGrid.GridCubeType.Open, openCube);
            materialDict.Add(InventoryGrid.GridCubeType.Occupied, occupiedCube);
            materialDict.Add(InventoryGrid.GridCubeType.ShadowOn, shadowCube);
            materialDict.Add(InventoryGrid.GridCubeType.Blocked, blockedCube);
        }


        inventoryGrid.AnnounceChangedCubeEvent += FlipCubeType;
    }

    private void FlipCubeType(GameObject gO, InventoryGrid.GridCubeType cubeType)
    {
        if (materialDict.TryGetValue(cubeType, out Material value))
        {
            MeshRenderer rend = gO.GetComponent<MeshRenderer>();

            rend.material = value;
        }
    }


    public void OnDisable()
    {
        inventoryGrid.AnnounceChangedCubeEvent -= FlipCubeType;
    }
}