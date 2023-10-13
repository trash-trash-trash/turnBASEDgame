using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryGridObjectController : MonoBehaviour
{
    //TODO : change to grid controlling target cubes later
    //will have to rotate a parent obj

    public Transform targetTransform;
    private Transform prevTransform;

    public bool isEquipped = false;

    public float rotationAngle = 0;
    public float rotationSpeed;
    private Quaternion targetRotation;
    private bool isRotating = false;

    public bool shiftHeld = false;

    public PlayerControls playerControls;

    public Vector2Int moveDistance;
    public float moveSpeed;

    private Vector2 initialPosition;
    private Vector2 targetPosition;
    private float startTime;
    private float journeyLength;

    public bool moving = false;


    public void Start()
    {
        playerControls = PlayerControls.PlayerControlsInstance;

        playerControls.MovementEvent += Movement;
        playerControls.inputs.TetrisInventory.Rotate.performed += Rotate;
        playerControls.inputs.TetrisInventory.Shift.performed += Shift;
        playerControls.inputs.TetrisInventory.Shift.canceled += Shift;
    }

    public void SetTargetTransform(Transform newTargetTransform)
    {
        if (newTargetTransform == null)
        {
            if (prevTransform != null)
            {
                prevTransform.position = new Vector3(prevTransform.position.x, prevTransform.position.y,
                    prevTransform.position.z - 1);
            }
            isEquipped = false;
            targetTransform = null;
        }
        else
        {
            if (newTargetTransform != prevTransform)
            {
                targetTransform = newTargetTransform;
                prevTransform = newTargetTransform;

                targetTransform.position = new Vector3(targetTransform.position.x, targetTransform.position.y,
                    targetTransform.position.z - 1);

                isEquipped = true;
                initialPosition = newTargetTransform.position;
            }
        }
    }

    public void Movement(Vector2 input)
    {
        if (!moving)
        {
            initialPosition = new Vector2(targetTransform.position.x, targetTransform.position.y);
            targetPosition = initialPosition + input;
            startTime = Time.time;
            journeyLength = moveDistance.magnitude;

            moving = true;
        }
    }

    public void Rotate(InputAction.CallbackContext callbackContext)
    {
        if (!isEquipped || isRotating)
            return;

        float x;
        float rotationAmount = shiftHeld ? x = 90.0f : x = -90.0f;

        rotationAngle += x;

        targetRotation = targetTransform.rotation * Quaternion.Euler(0, 0, rotationAmount);

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

    private void FixedUpdate()
    {
        if (isRotating)
        {
            // Rotate the transform towards the target rotation using Lerp
            targetTransform.rotation =
                Quaternion.Lerp(targetTransform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Check if the rotation is close to the target rotation (you can adjust the threshold)
            if (Quaternion.Angle(targetTransform.rotation, targetRotation) < 1.0f)
            {
                targetTransform.rotation = Quaternion.Euler(0, 0, rotationAngle);
                isRotating = false;
            }
        }

        if (moving)
        {
            float distanceCovered = (Time.time - startTime) * moveSpeed;
            float fractionOfJourney = distanceCovered / journeyLength;

            Vector2 newPosition = Vector2.Lerp(initialPosition, targetPosition, fractionOfJourney);

            targetTransform.position = new Vector3(newPosition.x, newPosition.y, targetTransform.position.z);

            if (fractionOfJourney >= 1.0f)
            {
                moving = false;
            }
        }
    }

    private void OnDisable()
    {
        playerControls.MovementEvent -= Movement;
        playerControls.inputs.TetrisInventory.Rotate.performed -= Rotate;
        playerControls.inputs.TetrisInventory.Shift.performed -= Shift;
        playerControls.inputs.TetrisInventory.Shift.canceled -= Shift;
    }
}