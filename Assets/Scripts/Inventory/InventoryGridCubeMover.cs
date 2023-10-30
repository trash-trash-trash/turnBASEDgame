using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class InventoryGridCubeMover : MonoBehaviour
{
    private PlayerControls playerControls;

    public InventoryGridCubeRotater cubeRotater;

    public InventoryGrid currentGrid;
    public InventoryGrid playerGrid;
    public InventoryGrid NPCGrid;

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

    public bool doubleTime;

    public float moveSpeedMultiplier;

    void OnEnable()
    {
        playerControls = PlayerControls.PlayerControlsInstance;

        playerControls.MovementEvent += Movement;
    }

    void Update()
    {
        rotating = cubeRotater.rotating;
    }

    void FixedUpdate()
    {
        if (moving)
        {
            float newMoveSpeed;
            if (doubleTime)
            {
                newMoveSpeed = moveSpeed * moveSpeedMultiplier;
            }
            else
            {
                newMoveSpeed = moveSpeed;
            }

            float distanceCovered = (Time.time - startTime) * newMoveSpeed;
            float fractionOfJourney = distanceCovered / journeyLength;

            if (fractionOfJourney >= 1.0f)
            {
                Vector3 finalPosition = new Vector3(targetPosition.x, targetPosition.y, originalPos.z - gridPosZOffset);
                targetTransform.position = finalPosition;
                doubleTime = false;
                moving = false;
            }
            else
            {
                Vector2 newPosition = Vector2.Lerp(initialPosition, targetPosition, fractionOfJourney);
                targetTransform.position = new Vector3(newPosition.x, newPosition.y, originalPos.z - gridPosZOffset);
            }
        }
    }

    public void SetParentCube(GridObjectParent newTargetParent, bool input, InventoryGrid targetGrid)
    {
        if (input)
        {
            currentGrid = targetGrid;
            currentGrid.selected = true;

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
                currentGrid.ChangeTargetCubeType((int)(targetParentObj.parentPosition.x + vec.x),
                    (int)(targetParentObj.parentPosition.y + vec.y), InventoryGrid.GridCubeType.Open);
            }

            foreach (Vector2 vec in targetParentObj.gridPositions)
            {
                currentGrid.ChangeTargetCubeType((int)(targetParentObj.parentPosition.x + vec.x),
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
        if (!movingCubes || moving)
            return;

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
                currentGrid.ChangeTargetCubeType((int)(targetParentObj.parentPosition.x + vec.x),
                    (int)(targetParentObj.parentPosition.y + vec.y), InventoryGrid.GridCubeType.ShadowOff);
            }

            targetTransform.position = originalPos;
            targetParentObj.parentPosition = originalParentVector;

            cubeRotater.ReturnToOriginalRotation();

            foreach (Vector2 vec in targetParentObj.gridPositions)
            {
                currentGrid.ChangeTargetCubeType((int)(targetParentObj.parentPosition.x + vec.x),
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
                currentGrid.ChangeTargetCubeType((int)(targetParentObj.parentPosition.x + vec.x),
                    (int)(targetParentObj.parentPosition.y + vec.y), InventoryGrid.GridCubeType.Occupied);
            }
        }

        movingCubes = false;

        targetParentObj = null;

        targetTransform = null;

        cubeRotater.SetTransform(targetTransform, targetParentObj, false);

        currentGrid.selected = false;
        currentGrid = null;
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

            // Check for out of bounds y
            int gridHeight = currentGrid.gridHeight;

            if (targetPosition.y >= gridHeight)
            {
                foreach (Vector2 vec in targetParentObj.gridPositions)
                {
                    currentGrid.ChangeTargetCubeType((int)(targetParentObj.parentPosition.x + vec.x),
                        (int)(targetParentObj.parentPosition.y + vec.y), InventoryGrid.GridCubeType.ShadowOff);
                }

                int inputsDown = (int)(targetPosition.y - gridHeight);
                targetPosition.y = 0.25f;
                targetParentObj.parentPosition.y = 0;
                inputDirection = new Vector2(inputDirection.x, -inputsDown + 1);
                doubleTime = true;
            }
            else if (targetPosition.y < 0)
            {
                foreach (Vector2 vec in targetParentObj.gridPositions)
                {
                    currentGrid.ChangeTargetCubeType((int)(targetParentObj.parentPosition.x + vec.x),
                        (int)(targetParentObj.parentPosition.y + vec.y), InventoryGrid.GridCubeType.ShadowOff);
                }

                int inputsUp = Mathf.Abs((int)targetPosition.y);
                targetPosition.y = gridHeight - inputsUp + .25f;
                targetParentObj.parentPosition.y = gridHeight - inputsUp;
                inputDirection = new Vector2(inputDirection.x, -inputsUp + .25f);
                doubleTime = true;
            }

            // Check for out of bounds x
            int gridWidth = currentGrid.gridWidth;

            if (targetPosition.x >= gridWidth)
            {
                foreach (Vector2 vec in targetParentObj.gridPositions)
                {
                    currentGrid.ChangeTargetCubeType((int)(targetParentObj.parentPosition.x + vec.x),
                        (int)(targetParentObj.parentPosition.y + vec.y), InventoryGrid.GridCubeType.ShadowOff);
                }
                int inputsRight = (int)(targetPosition.x - gridWidth);
                targetPosition.x = 0.25f -1;
                targetParentObj.parentPosition.x = -1;
                inputDirection = new Vector2(-inputsRight + .25f, inputDirection.y);
                doubleTime = true;
            }
            else if (targetPosition.x < -1)
            {
                foreach (Vector2 vec in targetParentObj.gridPositions)
                {
                    currentGrid.ChangeTargetCubeType((int)(targetParentObj.parentPosition.x + vec.x),
                        (int)(targetParentObj.parentPosition.y + vec.y), InventoryGrid.GridCubeType.ShadowOff);
                }
                int inputsLeft = Mathf.Abs((int)targetPosition.x);
                targetPosition.x = gridWidth - inputsLeft + 0.25f -1;
                targetParentObj.parentPosition.x = gridWidth - inputsLeft+1;
                inputDirection = new Vector2(-inputsLeft + 0.25f, inputDirection.y);
                doubleTime = true;
            }


            startTime = Time.time;
            journeyLength = Vector2.Distance(initialPosition, targetPosition);

            MoveList(inputDirection);

            moving = true;
        }
    }

    public void MoveList(Vector2 input)
    {
        Vector2 newInput = input.normalized;

        foreach (Vector2 vec in targetParentObj.gridPositions)
        {
            currentGrid.ChangeTargetCubeType((int)(targetParentObj.parentPosition.x + vec.x),
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
                currentGrid.ChangeTargetCubeType((int)(targetParentObj.parentPosition.x + vec.x),
                    (int)(targetParentObj.parentPosition.y + vec.y), InventoryGrid.GridCubeType.ShadowOn);
            }
        }
        else
        {
            foreach (Vector2 vec in targetParentObj.gridPositions)
            {
                currentGrid.ChangeTargetCubeType((int)(targetParentObj.parentPosition.x + vec.x),
                    (int)(targetParentObj.parentPosition.y + vec.y), InventoryGrid.GridCubeType.Blocked);
            }
        }
    }

    public void ChangeGrid(InventoryGrid input)
    {
        if (input == playerGrid)
            currentGrid = NPCGrid;

        else
            currentGrid = playerGrid;
    }


    void OnDisable()
    {
        playerControls.MovementEvent -= Movement;
    }
}