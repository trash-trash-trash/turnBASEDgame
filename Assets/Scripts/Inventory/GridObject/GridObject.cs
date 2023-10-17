using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject : MonoBehaviour, IInventoryObject
{
    public bool equipped;

    public bool playerHighlighted;

    public bool highlighted;

    public Vector2 gridVector2;

    public Transform parentTransform;

    public Transform GetParentTransform()
    {
        return parentTransform;
    }


    public Vector2 ReturnGridVector2()
    {
        gridVector2 = transform.localPosition;
        return gridVector2;
    }

    public event Action<bool, IInventoryObject> AnnouncePlayerHighlightEvent;
    public event Action<bool> AnnounceHighlightEvent;

    public event Action<bool> PlayerEquippedEvent;
    public event Action<bool> AnnouncePickUpPutDownEvent;

    public void EquippedByPlayer(bool input)
    {      
        equipped = input;
        PlayerEquippedEvent?.Invoke(input);
    }


    public void HighlightedByPlayer(bool input)
    {        
        playerHighlighted = input;
        AnnouncePlayerHighlightEvent?.Invoke(input, this);
    }

    public void PickUpPutDown(bool input)
    {      
        equipped = input;
        AnnouncePickUpPutDownEvent?.Invoke(input);
    }

    public bool ReturnPlayerHighlight()
    {
        return playerHighlighted;
    }

    public bool ReturnEquipped()
    {
        return equipped;
    }
        
    public void HighLight(bool input)
    { 
        highlighted = input;
        AnnounceHighlightEvent?.Invoke(input);
    }

    public bool ReturnHighlighted()
    {
        return highlighted;
    }
}