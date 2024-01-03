using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class GridObjectParent : MonoBehaviour
{
    public InventoryItemScriptableObject item;

    // public InventoryGridObjectController controller;
   public InventoryGridCubeMover mover;

   public InventoryGrid currentGrid;

    public List<IInventoryObject> inventoryObjects = new List<IInventoryObject>();
    public List<Vector2> inventoryVectors = new List<Vector2>();

    public bool highlighted;

    public bool equipped;

    public Vector2 parentPosition;

    public List<Vector2> gridPositions = new List<Vector2>();

    public bool initialised = false;

    void OnEnable()
    {
        inventoryVectors.Clear();

        IInventoryObject[] inventoryObjectComponents = GetComponentsInChildren<IInventoryObject>();

        foreach (IInventoryObject obj in inventoryObjectComponents)
        {
            inventoryObjects.Add(obj);

            inventoryVectors.Add(obj.ReturnGridVector2());

            obj.AnnouncePlayerHighlightEvent += HighlightChildren;
            obj.PlayerEquippedEvent += EquipChildren;

            gridPositions.Add(obj.ReturnGridVector2());
        }

        initialised = true;
    }

    void OnDisable()
    {
        foreach (IInventoryObject obj in inventoryObjects)
        {
            obj.AnnouncePlayerHighlightEvent -= HighlightChildren;
            obj.PlayerEquippedEvent -= EquipChildren;
        }
    }

    private void HighlightChildren(bool input, IInventoryObject iObj, GridObjectParent parentObj)
    {
        if (input)
        {
            foreach (IInventoryObject obj in inventoryObjects)
            {
                obj.HighLight(true);
            }
        }

        else
        {
            foreach (IInventoryObject obj in inventoryObjects)
            {
                obj.HighLight(false);
            }
        }

        highlighted = input;
    }

    public void EquipChildren(bool input)
    {
        if (input)
        {
            foreach (IInventoryObject obj in inventoryObjects)
            {
                obj.PickUpPutDown(true);
            }

            mover.SetParentCube(this, true, currentGrid);
       //     controller.SetTargetTransform(transform, true);
        }

        else
        {
            foreach (IInventoryObject obj in inventoryObjects)
            {
                obj.PickUpPutDown(false);
            }

            mover.SetParentCube(this,false, null);
            //   controller.SetTargetTransform(null, false);
        }

        equipped = input;
    }
}