using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuBase : MonoBehaviour
{
    public List<Button> buttonsList;

    public MainMenuBrain brain;

    public PlayerControls playerControls;

    public int selectInt;

    public int maxSelectInt;

    protected virtual void OnEnable()
    {
        playerControls = PlayerControls.PlayerControlsInstance;

        playerControls.MovementEvent += MenuMovement;
        playerControls.MenuConfirmEvent += Confirm;
        playerControls.MenuCancelEvent += Cancel;
    }


    protected virtual void OnDisable()
    {
        playerControls.MovementEvent -= MenuMovement;
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
        if (vector2 == Vector2.zero)
            return;

        int amount = 0;

        // Horizontal movement (left/right)
        if (vector2.x > 0)
        {
            amount = 1;
        }
        else if (vector2.x < 0)
        {
            amount = -1;
        }

        // Vertical movement (up/down)
        else if (vector2.y < 0)
        {
            amount = -1;
        }
        else if (vector2.y > 0)
        {
            amount = 1;
        }

        ChangeSelectInt(amount);
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

        if (buttonsList.Count > 0)
        {
            // Deselect the currently selected button
            if (selectInt >= 0 && selectInt < buttonsList.Count)
            {
                Selectable currentlySelectedButton = buttonsList[selectInt].GetComponent<Selectable>();
                if (currentlySelectedButton != null)
                {
                    currentlySelectedButton.OnDeselect(null);
                }
            }

            selectInt = newInt;

            // Select the new button
            if (selectInt >= 0 && selectInt < buttonsList.Count)
            {
                buttonsList[selectInt].Select();
            }

            Debug.Log(selectInt);
        }
    }
}