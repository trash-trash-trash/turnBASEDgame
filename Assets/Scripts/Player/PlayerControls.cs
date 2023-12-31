using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerControls : MonoBehaviour
{
    #region Singleton

    //change to a playermanager singleton for two players?

    private static PlayerControls instance;

    public static PlayerControls PlayerControlsInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerControls>();

                if (instance == null)
                {
                    GameObject singletonObject = new GameObject("PlayerSingleton");
                    instance = singletonObject.AddComponent<PlayerControls>();
                }
            }

            return instance;
        }
    }

    #endregion

    public PlayerInputs inputs;

    public Vector2 movementInputs;

    public event Action<Vector2> MovementEvent;

    public event Action InteractEvent;

    public event Action MenuConfirmEvent;

    public event Action MenuCancelEvent;

    public event Action RotateEvent;

    public event Action<bool> ShiftHeldEvent;

    public event Action<bool> TabHeldEvent;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        inputs = new PlayerInputs();
        inputs.Enable();

        inputs.OverworldMovement.MoveInput.performed += Movement;
        inputs.OverworldMovement.MoveInput.canceled += Movement;
        inputs.OverworldMovement.Interact.performed += Interact;
        inputs.TurnBasedCombat.Confirm.performed += MenuConfirm;
        inputs.TurnBasedCombat.Cancel.performed += MenuCancel;
        inputs.TetrisInventory.Rotate.performed += Rotate;
        inputs.TetrisInventory.Shift.performed += ShiftHeld;
        inputs.TetrisInventory.Shift.canceled += ShiftHeld;
        inputs.TetrisInventory.Tab.performed += TabHeld;
        inputs.TetrisInventory.Tab.canceled += TabHeld;
    }

    private void ShiftHeld(InputAction.CallbackContext context)
    {
        if (context.performed)
            ShiftHeldEvent?.Invoke(true);

        else
            ShiftHeldEvent?.Invoke(false);
    }

    private void Rotate(InputAction.CallbackContext context)
    {
        RotateEvent?.Invoke();
    }

    private void TabHeld(InputAction.CallbackContext context)
    {
        if (context.performed)
            TabHeldEvent?.Invoke(true);

        else
            TabHeldEvent?.Invoke(false);
    }

    private void Movement(InputAction.CallbackContext context)
    {
        //  Debug.Log(context.ReadValue<Vector2>());

        movementInputs = context.ReadValue<Vector2>();

        if (context.canceled)
            movementInputs = Vector2.zero;
        
        MovementEvent?.Invoke(movementInputs);
    }


    private void Interact(InputAction.CallbackContext context)
    {
        InteractEvent?.Invoke();
    }

    private void MenuConfirm(InputAction.CallbackContext context)
    {
        MenuConfirmEvent?.Invoke();
    }

    private void MenuCancel(InputAction.CallbackContext context)
    {
        MenuCancelEvent?.Invoke();
    }

    private void OnDisable()
    {
        inputs.OverworldMovement.MoveInput.performed -= Movement;
        inputs.OverworldMovement.MoveInput.canceled -= Movement;
        inputs.OverworldMovement.Interact.performed -= Interact;
        inputs.OverworldMovement.Interact.performed -= MenuConfirm;
        inputs.TurnBasedCombat.Cancel.performed -= MenuCancel;
        inputs.TetrisInventory.Rotate.performed -= Rotate;
        inputs.TetrisInventory.Rotate.performed -= ShiftHeld;
        inputs.TetrisInventory.Rotate.canceled -= ShiftHeld;
        inputs.TetrisInventory.Tab.performed -= TabHeld;
        inputs.TetrisInventory.Tab.canceled -= TabHeld;
    }
}