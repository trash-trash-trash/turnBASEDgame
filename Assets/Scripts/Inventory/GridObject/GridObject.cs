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
        PlayerEquippedEvent?.Invoke(input);
        equipped = input;
    }


    public void HighlightedByPlayer(bool input)
    {
        AnnouncePlayerHighlightEvent?.Invoke(input, this);
        playerHighlighted = input;
    }

    public void PickUpPutDown(bool input)
    {
        AnnouncePickUpPutDownEvent?.Invoke(input);
        equipped = input;
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
        AnnounceHighlightEvent?.Invoke(input);
        highlighted = input;
    }

    public bool ReturnHighlighted()
    {
        return highlighted;
    }
}