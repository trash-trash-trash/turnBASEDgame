using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class GridObjectParent : MonoBehaviour
{
    public InventoryGridObjectController controller;

    public List<IInventoryObject> inventoryObjects = new List<IInventoryObject>();
    public List<Vector2> inventoryVectors = new List<Vector2>();

    public bool highlighted;

    public bool equipped;

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
        }
    }

    void OnDisable()
    {
        foreach (IInventoryObject obj in inventoryObjects)
        {
            obj.AnnouncePlayerHighlightEvent -= HighlightChildren;
            obj.PlayerEquippedEvent -= EquipChildren;
        }
    }

    private void HighlightChildren(bool input, IInventoryObject iObj)
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
    }

    private void EquipChildren(bool input)
    {
        if (input)
        {
            foreach (IInventoryObject obj in inventoryObjects)
            {
                obj.PickUpPutDown(true);
            }

            controller.SetTargetTransform(transform);
        }

        else
        {
            foreach (IInventoryObject obj in inventoryObjects)
            {
                obj.PickUpPutDown(false);
            }

            controller.SetTargetTransform(null);
        }
    }
}