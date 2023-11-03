using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MainMenuBase : MonoBehaviour
{
    public MainMenuBrain brain;

    public PlayerControls playerControls;

    public int selectInt;

    public int maxSelectInt;

    protected virtual void OnEnable()
    {
        playerControls = PlayerControls.PlayerControlsInstance;

        playerControls.MenuMovementEvent += MenuMovement;
        playerControls.MenuConfirmEvent += Confirm;
        playerControls.MenuCancelEvent += Cancel;
    }


    protected virtual void OnDisable()
    {
        playerControls.MenuMovementEvent -= MenuMovement;
        playerControls.MenuConfirmEvent -= Confirm;
        playerControls.MenuCancelEvent -= Cancel;
    }
    
    protected virtual void Confirm()
    {
    }

    protected virtual void Cancel()
    {
    }

    private void MenuMovement(Vector2 vector2)
    {
        if (vector2.x > 0 || vector2.y > 0)
        {
            ChangeSelectInt(-1);
        }
        else if (vector2.x < 0 || vector2.y < 0)
        {
            ChangeSelectInt(1);
        }
        Debug.Log(vector2);
    }

    protected virtual void ChangeSelectInt(int amount)
    {
        int newInt = selectInt + amount;

        if (newInt < 0)
        {
            newInt = maxSelectInt;
        }

        else if (newInt > maxSelectInt)
        {
            newInt = 0;
        }

        selectInt = newInt;

        Debug.Log(selectInt);
    }
}
