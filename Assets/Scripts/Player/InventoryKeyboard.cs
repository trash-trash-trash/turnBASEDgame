using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryKeyboard : MonoBehaviour
{
    //pressing Tab while in Inventory cycles thru selecting the equipped Item
    //holding Shift and pressing Tab cycles in reverse

    public GridObjectParent currentObj;

    public GridObjectParent prevObj;

    public InventoryGridItems items;

    public PlayerControls playerControls;

    public InventoryMouse mouse;

    private bool firstTime = true;

    public int selectInt;

    public bool shiftHeld = false;

    public void OnEnable()
    {
        playerControls = PlayerControls.PlayerControlsInstance;

        playerControls.TabHeldEvent += CycleThroughItems;
        playerControls.ShiftHeldEvent += SetShift;
    }

    private void CycleThroughItems(bool input)
    {
        if (mouse.equipped)
            return;

        if (mouse.selectedGridObject != null)
        {
            mouse.scanning = false;
            mouse.selectedGridObject.HighlightedByPlayer(false);
        }

        if (input)
        {
            if (firstTime)
            {
                selectInt = 0;
                currentObj = items.gridObjs[selectInt];
                prevObj = currentObj;
                currentObj.inventoryObjects[0].HighlightedByPlayer(true);
                firstTime = false;
            }
            else
            {
                if (!shiftHeld)
                {
                    ChangeSelectInt(1);
                }
                else
                {
                    ChangeSelectInt(-1);
                }

                prevObj.inventoryObjects[0].HighlightedByPlayer(false);
                currentObj = items.gridObjs[selectInt];
                currentObj.inventoryObjects[0].HighlightedByPlayer(true);
                prevObj = currentObj;
            }
        }
    }

    void ChangeSelectInt(int input)
    {
        selectInt += input;

        if (selectInt < 0)
            selectInt = items.gridObjs.Count - 1;

        else if (selectInt > items.gridObjs.Count - 1)
            selectInt = 0;
    }

    private void SetShift(bool input)
    {
        if (input)
            shiftHeld = true;
        else
            shiftHeld = false;
    }

    void OnDisable()
    {
        playerControls.TabHeldEvent -= CycleThroughItems;
        playerControls.ShiftHeldEvent -= SetShift;
    }
}