using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryGridObject : MonoBehaviour
{
    //TODO : change to grid controlling target cubes later
    //will have to rotate a parent obj

    public bool isEquipped = false;

    public Transform targetTransform;
    public float rotationSpeed;
    private Quaternion targetRotation;
    private bool isRotating = false;

    public bool shiftHeld = false;

    public PlayerControls playerControls;

    public void Start()
    {
        playerControls = PlayerControls.PlayerControlsInstance;
        playerControls.inputs.TetrisInventory.Rotate.performed += Rotate;
        playerControls.inputs.TetrisInventory.Shift.performed += Shift;
        playerControls.inputs.TetrisInventory.Shift.canceled += Shift;
    }

    public void Rotate(InputAction.CallbackContext callbackContext)
    {
        if (!isEquipped)
            return;
        // Determine the desired rotation amount based on the input bool
        float rotationAmount = shiftHeld ? 90.0f : -90.0f;

        // Calculate the target rotation quaternion
        targetRotation = targetTransform.rotation * Quaternion.Euler(0, 0, rotationAmount);

        // Set the flag to start rotating
        isRotating = true;
    }

    private void Shift(InputAction.CallbackContext context)
    {
        if (!isEquipped)
            return;

        if (context.performed)
            shiftHeld = true;
        else
            shiftHeld = false;
    }

    private void Update()
    {
        if (isRotating)
        {
            // Rotate the transform towards the target rotation using Lerp
            targetTransform.rotation =
                Quaternion.Lerp(targetTransform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Check if the rotation is close to the target rotation (you can adjust the threshold)
            if (Quaternion.Angle(targetTransform.rotation, targetRotation) < 1.0f)
            {
                isRotating = false;
            }
        }
    }
}