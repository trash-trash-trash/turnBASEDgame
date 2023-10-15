using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class InventoryGridObjectController : MonoBehaviour
{
    //TODO : change to grid controlling target cubes later
    //will have to rotate a parent obj

    public Transform targetTransform;
    private GridObjectParent targetParentObj;
    private Transform prevTransform;

    public bool isEquipped = false;

    public float rotationSpeed;
    private Quaternion targetRotation;
    private bool isRotating = false;

    public bool shiftHeld = false;

    public PlayerControls playerControls;

    public Vector2Int moveDistance;
    public float moveSpeed;

    private Vector2 initialPosition;
    public Vector2 targetPosition;
    private float startTime;
    private float journeyLength;

    public bool moving = false;

    public InventoryGrid grid;
    public InventoryGridItems gridItems;

    public float gridPosOffset;

    private List<Vector2> originalVectors = new List<Vector2>();
    private Vector2 originalParentVector;
    private Vector2 parentPositionVector;


    public void Start()
    {
        playerControls = PlayerControls.PlayerControlsInstance;

        playerControls.MovementEvent += Movement;
        playerControls.inputs.TetrisInventory.Rotate.performed += Rotate;
        playerControls.inputs.TetrisInventory.Shift.performed += Shift;
        playerControls.inputs.TetrisInventory.Shift.canceled += Shift;
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
                targetTransform.rotation = targetRotation;
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


    public void SetTargetTransform(Transform newTargetTransform, bool newEquipped)
    {
        if (newEquipped)
        {
            Equip(newTargetTransform);
        }
        else
        {
            Unequip(newTargetTransform);
        }
    }

    void Equip(Transform newTargetTransform)
    {
        targetTransform = newTargetTransform;
        prevTransform = newTargetTransform;

        targetTransform.position = new Vector3(targetTransform.position.x - gridPosOffset,
            targetTransform.position.y - gridPosOffset,
            targetTransform.position.z - 1);

        targetParentObj = newTargetTransform.GetComponent<GridObjectParent>();

        isEquipped = true;
        initialPosition = newTargetTransform.position;

        foreach (Vector2 vec in targetParentObj.gridPositions)
        {
            grid.TargetCubesFlipShadow((int)(targetParentObj.parentPosition.x + vec.x),
                (int)(targetParentObj.parentPosition.y + vec.y), true);

            originalVectors.Add(vec);
        }
    }

    void Unequip(Transform newTargetTransform)
    {
        if (newTargetTransform == null)
        {
            if (prevTransform != null)
            {
                prevTransform.position = new Vector3(prevTransform.position.x + gridPosOffset,
                    prevTransform.position.y + gridPosOffset,
                    prevTransform.position.z + 1);
            }

            isEquipped = false;
            targetTransform = null;

            targetParentObj.gridPositions.Clear();

            targetParentObj.gridPositions = originalVectors;

            targetParentObj.parentPosition = originalParentVector;

            originalVectors.Clear();
        }
    }

    public void Movement(Vector2 input)
    {
        if (!isEquipped || moving || isRotating)
            return;

        if (!moving)
        {
            initialPosition = new Vector2(targetTransform.position.x, targetTransform.position.y);

            targetPosition = initialPosition + input;
            startTime = Time.time;
            journeyLength = moveDistance.magnitude;

            UpdateGridPositions(input);

            moving = true;
        }
    }

    public void Rotate(InputAction.CallbackContext callbackContext)
    {
        if (!isEquipped || isRotating || moving)
            return;

        float x;
        float rotationAmount = shiftHeld ? x = 90.0f : x = -90.0f;

        targetRotation = targetTransform.rotation * Quaternion.Euler(0, 0, rotationAmount);

        isRotating = true;
    }

    private void Shift(InputAction.CallbackContext context)
    {
        if (context.performed)
            shiftHeld = true;
        else
            shiftHeld = false;
    }

    private void UpdateGridPositions(Vector2 input)
    {
        
    }


    private void OnDisable()
    {
        playerControls.MovementEvent -= Movement;
        playerControls.inputs.TetrisInventory.Rotate.performed -= Rotate;
        playerControls.inputs.TetrisInventory.Shift.performed -= Shift;
        playerControls.inputs.TetrisInventory.Shift.canceled -= Shift;
    }
}