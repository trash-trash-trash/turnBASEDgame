using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

public class InventoryGridCubeRotater : MonoBehaviour
{
    public InventoryGridItems items;

    public InventoryGridCubeMover cubeMover;

    public GridObjectParent targetParentObj;

    private PlayerControls playerControls;

    public InventoryGrid grid;

    public Transform targetTransform;

    public bool rotating;

    public bool shiftHeld = false;

    public bool moving;

    public Quaternion originalRotation;

    public List<Vector2> originalVectors;

    private Quaternion targetRotation;

    float rotationAmount;


    public float rotationSpeed;

    void OnEnable()
    {
        playerControls = PlayerControls.PlayerControlsInstance;
        playerControls.RotateEvent += Rotate;
        playerControls.ShiftHeldEvent += Shift;
    }

    void OnDisable()
    {
        playerControls.RotateEvent -= Rotate;
        playerControls.ShiftHeldEvent -= Shift;
    }


    void Update()
    {
        moving = cubeMover.moving;
    }

    public void SetTransform(Transform newTransform, GridObjectParent newParentObj, bool input)
    {
        if (input)
        {
            targetTransform = newTransform;
            targetParentObj = newParentObj;

            originalVectors = targetParentObj.gridPositions;
            originalRotation = newTransform.rotation;
        }

        else
        {
        }
    }

    private void Rotate()
    {
        if (rotating || moving)
            return;

        foreach (Vector2 vec in targetParentObj.gridPositions)
        {
            grid.ChangeTargetCubeType((int)(targetParentObj.parentPosition.x + vec.x),
                (int)(targetParentObj.parentPosition.y + vec.y), InventoryGrid.GridCubeType.ShadowOff);
        }

        targetParentObj.gridPositions = RotateVectors(targetParentObj.gridPositions);

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
    public List<Vector2> RotateVectors(List<Vector2> originalList)
    {
        List<Vector2> rotatedList = new List<Vector2>();


        if (!shiftHeld)
        {
            foreach (Vector2 originalVector in originalList)
            {
                // Rotate the vector as described.
                float rotatedX = originalVector.y;
                float rotatedY = -originalVector.x;
                
                Vector2 rotatedVector = new Vector2(rotatedX, rotatedY);

                rotatedList.Add(rotatedVector);
            }

            rotationAmount = -90f;
        }
        else
        {
            foreach (Vector2 originalVector in originalList)
            {
                float rotatedX = -originalVector.y;
                float rotatedY = originalVector.x;
                
                Vector2 rotatedVector = new Vector2(rotatedX, rotatedY);

                rotatedList.Add(rotatedVector);

                rotationAmount = 90f;
            }
        }

        targetRotation = targetTransform.rotation * Quaternion.Euler(0, 0, rotationAmount);
        rotating = true;

        return rotatedList;
    }

    public void ReturnToOriginalRotation()
    {
        targetTransform.rotation = originalRotation;

        targetParentObj.gridPositions = originalVectors;
    }

    void Shift(bool input)
    {
        if (input)
            shiftHeld = true;
        else
            shiftHeld = false;
    }

    void FixedUpdate()
    {
        if (rotating)
        {
            targetTransform.rotation =
                Quaternion.Lerp(targetTransform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Check if the rotation is close to the target rotation (you can adjust the threshold)
            if (Quaternion.Angle(targetTransform.rotation, targetRotation) < 1.0f)
            {
                targetTransform.rotation = targetRotation;
                rotating = false;
            }
        }
    }
}