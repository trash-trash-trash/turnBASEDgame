using System;
using UnityEngine;

public interface IInventoryObject
{
    public Vector2 ReturnGridVector2();

    public event Action<bool, IInventoryObject> AnnouncePlayerHighlightEvent;
    public event Action<bool> AnnounceHighlightEvent;

    public event Action<bool> PlayerEquippedEvent;
    public event Action<bool> AnnouncePickUpPutDownEvent;

    public void EquippedByPlayer(bool input);

    public void HighlightedByPlayer(bool input);

    public void PickUpPutDown(bool input);

    public void HighLight(bool input);

    public bool ReturnHighlighted();

    public bool ReturnPlayerHighlight();

    public bool ReturnEquipped();

    public Transform GetParentTransform();
}