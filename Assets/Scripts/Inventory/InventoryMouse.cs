using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryMouse : MonoBehaviour
{
    public IInventoryObject selectedGridObject;
    private IInventoryObject prevObj;

    public float sphereRadius;

    public PlayerControls playerControls;

    public void OnEnable()
    {
        playerControls = PlayerControls.PlayerControlsInstance;

        playerControls.MenuConfirmEvent += TryEquip;
        playerControls.MenuCancelEvent += TryUnequip;
    }

    void OnDisable()
    {
        playerControls.MenuConfirmEvent -= TryEquip;
        playerControls.MenuCancelEvent -= TryUnequip;
    }

    private void Update()
    {
        ShootSphereCast();
    }

    void ShootSphereCast()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.SphereCast(ray, sphereRadius, out hit, Mathf.Infinity))
        {
            IInventoryObject newHighlightedObject = hit.transform.GetComponentInChildren<IInventoryObject>();

            if (newHighlightedObject != prevObj)
            {
                if (prevObj != null)
                {
                    prevObj.HighlightedByPlayer(false);
                }

                selectedGridObject = newHighlightedObject;

                if (selectedGridObject != null)
                {
                    selectedGridObject.HighlightedByPlayer(true);
                }

                prevObj = newHighlightedObject;
            }
        }
        else if (prevObj != null)
        {
            prevObj.HighlightedByPlayer(false);
            prevObj = null;
        }
    }


    private void TryEquip()
    {
        if (selectedGridObject != null)
        {
            selectedGridObject.EquippedByPlayer(true);
        }
    }

    private void TryUnequip()
    {
        if (selectedGridObject != null)
        {
            selectedGridObject.EquippedByPlayer(false);
        }
    }
}
