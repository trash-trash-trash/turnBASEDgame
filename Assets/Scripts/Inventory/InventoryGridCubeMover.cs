using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class InventoryGridCubeMover : MonoBehaviour
{
    private PlayerControls playerControls;

    public InventoryGridCubeRotater cubeRotater;

    public InventoryGrid grid;

    public InventoryGridItems items;

    public bool rotating = false;

    public bool moving = false;

    public bool movingCubes;

    public GridObjectParent targetParentObj;

    public Transform targetTransform;

    public Vector2 initialPosition;
    public Vector2 originalParentVector;
    public Vector2 targetPosition;
    public Vector3 originalPos;
    public List<Vector2> originalVectors;
    private float startTime;
    private float journeyLength;

    public float gridPosOffset;

    public float gridPosZOffset;

    public float moveSpeed;

    public Vector2Int moveDistance;

    void OnEnable()
    {
        playerControls = PlayerControls.PlayerControlsInstance;

        playerControls.MovementEvent += Movement;
    }

    void Update()
    {
        rotating = cubeRotater.rotating;
    }

    public void SetParentCube(GridObjectParent newTargetParent, bool input)
    {
        if (input)
        {
            targetParentObj = newTargetParent;

            initialPosition = targetParentObj.parentPosition;

            originalParentVector = targetParentObj.parentPosition;

            targetTransform = targetParentObj.transform;

            originalPos = targetTransform.position;

            targetTransform.position = new Vector3(targetTransform.position.x - gridPosOffset,
                targetTransform.position.y - gridPosOffset,
                targetTransform.position.z - gridPosZOffset);

            originalVectors = targetParentObj.gridPositions;

            foreach (Vector2 vec in targetParentObj.gridPositions)
            {
                grid.ChangeTargetCubeType((int)(targetParentObj.parentPosition.x + vec.x),
                    (int)(targetParentObj.parentPosition.y + vec.y), InventoryGrid.GridCubeType.Open);
            }

            foreach (Vector2 vec in targetParentObj.gridPositions)
            {
                grid.ChangeTargetCubeType((int)(targetParentObj.parentPosition.x + vec.x),
                    (int)(targetParentObj.parentPosition.y + vec.y), InventoryGrid.GridCubeType.ShadowOn);
            }

            movingCubes = true;

            cubeRotater.SetTransform(targetTransform, targetParentObj, true);
            
        }
        else
        {
            Unequip();
        }
    }

    void Unequip()
    {
        bool blocked = false;

        foreach (Vector2 vec in targetParentObj.gridPositions)
        {
            if (!items.IsAccessible((int)(targetParentObj.parentPosition.x + vec.x),
                    (int)(targetParentObj.parentPosition.y + vec.y)))
            {
                blocked = true;
                break;
            }
        }

        if (blocked)
        {
            foreach (Vector2 vec in targetParentObj.gridPositions)
            {
                grid.ChangeTargetCubeType((int)(targetParentObj.parentPosition.x + vec.x),
                    (int)(targetParentObj.parentPosition.y + vec.y), InventoryGrid.GridCubeType.ShadowOff);
            }

            targetTransform.position = originalPos;
            targetParentObj.parentPosition = originalParentVector;

            cubeRotater.ReturnToOriginalRotation();

            foreach (Vector2 vec in targetParentObj.gridPositions)
            {
                grid.ChangeTargetCubeType((int)(targetParentObj.parentPosition.x + vec.x),
                    (int)(targetParentObj.parentPosition.y + vec.y), InventoryGrid.GridCubeType.Occupied);
            }
        }
        else
        {
            targetTransform.position = new Vector3(targetTransform.position.x + gridPosOffset,
                targetTransform.position.y + gridPosOffset,
                targetTransform.position.z + gridPosZOffset);

            foreach (Vector2 vec in targetParentObj.gridPositions)
            {
                grid.ChangeTargetCubeType((int)(targetParentObj.parentPosition.x + vec.x),
                    (int)(targetParentObj.parentPosition.y + vec.y), InventoryGrid.GridCubeType.Occupied);
            }
        }

        movingCubes = false;

        targetParentObj = null;

        targetTransform = null;

        cubeRotater.SetTransform(targetTransform, targetParentObj, false);
    }

    private void Movement(Vector2 inputVector)
    {
        if (movingCubes && !moving && !rotating)
        {
            if (inputVector == Vector2.zero)
                return;

            initialPosition = new Vector2(targetTransform.position.x, targetTransform.position.y);

            Vector2 inputDirection = new Vector2(Mathf.Round(inputVector.x), Mathf.Round(inputVector.y));
            targetPosition = initialPosition + inputDirection;

            startTime = Time.time;
            journeyLength = Vector2.Distance(initialPosition, targetPosition);

            moving = true;

            MoveList(inputVector);
        }
    }

    public void MoveList(Vector2 input)
    {
        Vector2 newInput = input.normalized;
        newInput.x = (newInput.x > 0) ? 1 : (newInput.x < 0) ? -1 : 0;
        newInput.y = (newInput.y > 0) ? 1 : (newInput.y < 0) ? -1 : 0;

        foreach (Vector2 vec in targetParentObj.gridPositions)
        {
            grid.ChangeTargetCubeType((int)(targetParentObj.parentPosition.x + vec.x),
                (int)(targetParentObj.parentPosition.y + vec.y), InventoryGrid.GridCubeType.ShadowOff);
        }

        targetParentObj.parentPosition += newInput;

        bool blocked = false;

        foreach (Vector2 vec in targetParentObj.gridPositions)
        {
            if (!items.IsAccessible((int)(targetParentObj.parentPosition.x + vec.x),
                    (int)(targetParentObj.parentPosition.y + vec.y)))
            {
                blocked = true;
                break;
            }
        }

        if (!blocked)
        {
            foreach (Vector2 vec in targetParentObj.gridPositions)
            {
                grid.ChangeTargetCubeType((int)(targetParentObj.parentPosition.x + vec.x),
                    (int)(targetParentObj.parentPosition.y + vec.y), InventoryGrid.GridCubeType.ShadowOn);
            }
        }

        else
        {
            foreach (Vector2 vec in targetParentObj.gridPositions)
            {
                grid.ChangeTargetCubeType((int)(targetParentObj.parentPosition.x + vec.x),
                    (int)(targetParentObj.parentPosition.y + vec.y), InventoryGrid.GridCubeType.Blocked);
            }
        }
    }

    void FixedUpdate()
    {
        if (moving)
        {
            float distanceCovered = (Time.time - startTime) * moveSpeed;
            float fractionOfJourney = distanceCovered / journeyLength;

            if (fractionOfJourney >= 1.0f)
            {
                Vector3 finalPosition = new Vector3(targetPosition.x, targetPosition.y, originalPos.z - gridPosZOffset);
                targetTransform.position = finalPosition;
                moving = false;
            }
            else
            {
                Vector2 newPosition = Vector2.Lerp(initialPosition, targetPosition, fractionOfJourney);
                targetTransform.position = new Vector3(newPosition.x, newPosition.y, originalPos.z - gridPosZOffset);
            }
        }
    }

    void OnDisable()
    {
        playerControls.MovementEvent -= Movement;
    }
}