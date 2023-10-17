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

    public List<Vector2> rotatedVectors;

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
            Unequip(prevTransform);
        }
    }

    void Equip(Transform newTargetTransform)
    {
        Transform previousTransform = targetTransform;

        // Update targetTransform to the new item
        targetTransform = newTargetTransform;

        // Update prevTransform with the previous item
        prevTransform = previousTransform;

        // Other logic...

        isEquipped = true;
        
        targetTransform.position = new Vector3(targetTransform.position.x - gridPosOffset,
            targetTransform.position.y - gridPosOffset,
            targetTransform.position.z - 2);

        targetParentObj = newTargetTransform.GetComponent<GridObjectParent>();

        originalVectors = targetParentObj.gridPositions;
        originalParentVector = targetParentObj.parentPosition;
        
        initialPosition = newTargetTransform.position;

        foreach (Vector2 vec in targetParentObj.gridPositions)
        {
            grid.ChangeTargetCubeType((int)(targetParentObj.parentPosition.x + vec.x),
                (int)(targetParentObj.parentPosition.y + vec.y), InventoryGrid.GridCubeType.Open);

            grid.ChangeTargetCubeType((int)(targetParentObj.parentPosition.x + vec.x),
                (int)(targetParentObj.parentPosition.y + vec.y), InventoryGrid.GridCubeType.ShadowOn);
        }
    }

    void Unequip(Transform newTargetTransform)
    {
        if (newTargetTransform == null)
            return;

        bool canPlace = true;

        foreach (Vector2 vec in targetParentObj.gridPositions)
        {
            if (!gridItems.IsAccessible((int)(targetParentObj.parentPosition.x + vec.x),
                    (int)(targetParentObj.parentPosition.y + vec.y)))
            {
                canPlace = false;
                break;
            }
        }

        if (canPlace)
            PlaceItem();
        else
            ReturnItem();
    }

    private void PlaceItem()
    {
        foreach (Vector2 vec in targetParentObj.gridPositions)
        {
            grid.ChangeTargetCubeType((int)(targetParentObj.parentPosition.x + vec.x),
                (int)(targetParentObj.parentPosition.y + vec.y), InventoryGrid.GridCubeType.Occupied);

            grid.ChangeTargetCubeType((int)(targetParentObj.parentPosition.x + vec.x),
                (int)(targetParentObj.parentPosition.y + vec.y), InventoryGrid.GridCubeType.ShadowOff);
        }

        isEquipped = false;
        targetTransform = null;
        originalVectors.Clear();

        prevTransform.position = new Vector3(targetTransform.position.x + gridPosOffset,
            targetTransform.position.y + gridPosOffset,
            targetTransform.position.z + 2);
    }

    private void ReturnItem()
    {
        targetParentObj.transform.position = new Vector3(originalParentVector.x + gridPosOffset,
            originalParentVector.y + gridPosOffset, targetTransform.position.z+2);

        targetParentObj.gridPositions = originalVectors;

        targetParentObj.parentPosition = originalParentVector;

        foreach (Vector2 vec in targetParentObj.gridPositions)
        {
            grid.ChangeTargetCubeType((int)(targetParentObj.parentPosition.x + vec.x),
                (int)(targetParentObj.parentPosition.y + vec.y), InventoryGrid.GridCubeType.Occupied);

            grid.ChangeTargetCubeType((int)(targetParentObj.parentPosition.x + vec.x),
                (int)(targetParentObj.parentPosition.y + vec.y), InventoryGrid.GridCubeType.ShadowOff);
        }

        originalVectors.Clear();

        isEquipped = false;
        targetTransform = null;
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


        rotatedVectors = new List<Vector2>();

        foreach (Vector2 ve in rotatedVectors)
        {
            UpdateRotatedGridPositions(ve, true);
        }

        rotatedVectors = RotateVectors(rotatedVectors, rotationAmount);

        foreach (Vector2 ve in rotatedVectors)
        {
            UpdateRotatedGridPositions(ve, false);
        }


        // Update the grid positions with the rotated vectors
        targetParentObj.gridPositions.Clear();
        targetParentObj.gridPositions = rotatedVectors;
        rotatedVectors.Clear();
        targetRotation = targetTransform.rotation * Quaternion.Euler(0, 0, rotationAmount);
        isRotating = true;
    }

    private List<Vector2> RotateVectors(List<Vector2> vectors, float rotationAmount)
    {
        List<Vector2> newRotatedVectors = new List<Vector2>();
        newRotatedVectors.Clear();

        foreach (Vector2 vector in vectors)
        {
            float newX, newY;

            if (rotationAmount > 0)
            {
                // Rotate clockwise
                newX = -vector.y;
                newY = vector.x;
            }
            else
            {
                // Rotate counterclockwise
                newX = vector.y;
                newY = -vector.x;
            }

            newRotatedVectors.Add(new Vector2(newX, newY));
        }

        return newRotatedVectors;
    }

    private void Shift(InputAction.CallbackContext context)
    {
        if (context.performed)
            shiftHeld = true;
        else
            shiftHeld = false;
    }

    private void UpdateRotatedGridPositions(Vector2 input, bool firstStage)
    {
        if (firstStage)
            grid.ChangeTargetCubeType((int)(targetParentObj.parentPosition.x + input.x),
                (int)(targetParentObj.parentPosition.y + input.y), InventoryGrid.GridCubeType.ShadowOff);

        else
            grid.ChangeTargetCubeType((int)(targetParentObj.parentPosition.x + input.x),
                (int)(targetParentObj.parentPosition.y + input.y), InventoryGrid.GridCubeType.ShadowOn);
    }

    private void UpdateGridPositions(Vector2 input)
    {
        foreach (Vector2 vec in targetParentObj.gridPositions)
        {
            grid.ChangeTargetCubeType((int)(targetParentObj.parentPosition.x + vec.x),
                (int)(targetParentObj.parentPosition.y + vec.y), InventoryGrid.GridCubeType.ShadowOff);
        }

        targetParentObj.parentPosition += input;

        foreach (Vector2 vec in targetParentObj.gridPositions)
        {
            grid.ChangeTargetCubeType((int)(targetParentObj.parentPosition.x + vec.x),
                (int)(targetParentObj.parentPosition.y + vec.y), InventoryGrid.GridCubeType.ShadowOn);
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