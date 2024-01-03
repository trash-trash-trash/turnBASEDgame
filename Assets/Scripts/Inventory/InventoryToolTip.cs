using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryToolTip : MonoBehaviour
{
    public InventoryGridItems gridItems;

    public GameObject tooltipCanvasGO;

    public InventoryItemScriptableObject currentObj;

    public TMP_Text itemName;

    public TMP_Text itemStats;
    
    public TMP_Text itemDescription;
    
    void OnEnable()
    {
        foreach (GridObjectParent obj in gridItems.gridObjs)
        {
            if (!obj.initialised)
            {
                // Wait until initialised
                StartCoroutine(WaitForInitialization(obj));
            }
            else
            {
                SubscribeToHighlightEvent(obj);
            }
        }
    }

    IEnumerator WaitForInitialization(GridObjectParent obj)
    {
        while (!obj.initialised)
        {
            yield return null; // Wait for the next frame
        }

        // Object is now initialised
        SubscribeToHighlightEvent(obj);
    }

    void SubscribeToHighlightEvent(GridObjectParent obj)
    {
        List<IInventoryObject> objList = obj.inventoryObjects;

        foreach (IInventoryObject newObj in objList)
        {
            newObj.AnnouncePlayerHighlightEvent += TurnCanvasOnOff;
        }
    }

    void TurnCanvasOnOff(bool input, IInventoryObject inventoryObject, GridObjectParent parentObj)
    {
        if (input)
        {
            currentObj = parentObj.item;
            ChangeText();
        }

        else
        {
            currentObj = null;
        }
            
        tooltipCanvasGO.SetActive(input);
    }

    void ChangeText()
    {
        itemName.text = "";
        itemStats.text = "";
        itemDescription.text = "";
        
        itemName.text = currentObj.name;

        foreach (InventoryItemScriptableObject.ItemStatsEffected stat in currentObj.statsList)
        {
            // Determine the sign
            string sign = (stat.statVariable >= 0) ? "+" : "-";

            // Append each line to the existing text
            itemStats.text += $"{stat.stat}: {sign}{Mathf.Abs(stat.statVariable)}\n";
        }


        itemDescription.text = $"<i>{currentObj.description}</i>";
    }

    private void OnDisable()
    {
        foreach (GridObjectParent obj in gridItems.gridObjs)
        {
            List<IInventoryObject> objList = obj.inventoryObjects;

            foreach (IInventoryObject newObj in objList)
            {
                newObj.AnnouncePlayerHighlightEvent -= TurnCanvasOnOff;
            }
        }
    }
}