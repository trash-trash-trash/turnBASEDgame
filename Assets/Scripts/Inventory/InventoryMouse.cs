using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryMouse : MonoBehaviour
{
    public IInventoryObject selectedGridObject;
    public IInventoryObject equippedObj;
    private IInventoryObject prevObj;

    public float sphereRadius;

    public PlayerControls playerControls;

    public bool equipped = false;

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

            if (prevObj != null && prevObj != newHighlightedObject)
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


    private void TryEquip()
    {
        if (selectedGridObject != null && !equipped)
        {
            selectedGridObject.EquippedByPlayer(true);
            equippedObj = selectedGridObject;
            equipped = true;
        }
    }

    private void TryUnequip()
    {
        if (equippedObj != null && equipped)
        {
            equippedObj.EquippedByPlayer(false);
            equippedObj = null;
            selectedGridObject = null;
            equipped = false;
        }
    }
}