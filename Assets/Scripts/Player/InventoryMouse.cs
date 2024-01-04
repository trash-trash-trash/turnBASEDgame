using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryMouse : MonoBehaviour
{
    public InventoryKeyboard kbd;
    
    public IInventoryObject selectedGridObject;
    public IInventoryObject equippedObj;
    private IInventoryObject prevObj;

    public float sphereRadius;

    public PlayerControls playerControls;

    public bool equipped = false;

    public bool scanning = true;

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
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        if (mouseX != 0 || mouseY != 0)
        {
            // Mouse is moving, do something
            if (kbd.currentObj != null)
                kbd.currentObj.inventoryObjects[0].HighlightedByPlayer(false);

            scanning = true;
        }
        
        if(!equipped && scanning)
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
        if (equipped)
            TryUnequip();
        
        else if (selectedGridObject != null)
        {
            Equip();
        }

        else if (kbd.currentObj != null)
        {
            selectedGridObject = kbd.currentObj.inventoryObjects[0];
            Equip();
        }
    }

    void Equip()
    {
        if(!scanning && kbd.currentObj!=null)
            selectedGridObject = selectedGridObject = kbd.currentObj.inventoryObjects[0];
        
        selectedGridObject.EquippedByPlayer(true);
        equippedObj = selectedGridObject;
        equipped = true;
    }

    public void TryUnequip()
    {
        if (equippedObj != null)
        {
            Unequip();
        }

        else if (kbd.currentObj != null && kbd.currentObj.inventoryObjects[0] != null)
        {
            kbd.currentObj.inventoryObjects[0].HighlightedByPlayer(false);
            equippedObj = kbd.currentObj.inventoryObjects[0];
            Unequip();
        }
    }

    void Unequip()
    {
        equippedObj.EquippedByPlayer(false);
        equippedObj = null;
        selectedGridObject = null;
        equipped = false;
    }
}