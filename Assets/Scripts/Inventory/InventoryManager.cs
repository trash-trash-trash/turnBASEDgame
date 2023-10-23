using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject playerInventory;

    public GameObject NPCInventory;

    public void OpenClosePlayerInventory(bool input)
    {
        if(input)
            playerInventory.SetActive(true);
        else
        {
            playerInventory.SetActive(false);
        }
    }

    public void OpenCloseNPCInventory(bool input)
    {
        if (input)
        {
            playerInventory.SetActive(true);
            NPCInventory.SetActive(true);
        }
        else
        {
            playerInventory.SetActive(false);
            NPCInventory.SetActive(false);
        }
    }
}
