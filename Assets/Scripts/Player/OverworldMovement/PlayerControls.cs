using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    public PlayerInputs inputs;

    public Vector2 movementInputs;

    public event Action<Vector2> MovementEvent;

    public event Action InteractEvent;

    private void Awake()
    {
        inputs = new PlayerInputs();
        inputs.Enable();

        inputs.OverworldMovement.MoveInput.performed += Movement;
        inputs.OverworldMovement.MoveInput.canceled += Movement;

        inputs.OverworldMovement.Interact.performed += Interact;
    }

    private void Movement(InputAction.CallbackContext context)
    {
        movementInputs = context.ReadValue<Vector2>().normalized;

        if(context.canceled)
            movementInputs = Vector2.zero;

        MovementEvent?.Invoke(movementInputs);
    }

    private void Interact(InputAction.CallbackContext context)
    {
        InteractEvent?.Invoke();
        Debug.Log("Interact Input");
    }

    private void OnDisable()
    {
        inputs.OverworldMovement.MoveInput.performed -= Movement;
        inputs.OverworldMovement.MoveInput.canceled -= Movement;
        inputs.OverworldMovement.Interact.performed -= Interact;
    }
}
