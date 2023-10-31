using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteractBox : MonoBehaviour, IInteractable
{
    public event Action<DoorSide> DeclareDoorInteractEvent;

    public enum DoorSide
    {
        Side1,
        Side2
    }

    public DoorSide mySide;
    
    public void Interact()
    {
        DeclareDoorInteractEvent?.Invoke(mySide);
    }
}
