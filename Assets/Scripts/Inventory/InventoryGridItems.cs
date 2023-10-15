using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryGridItems : MonoBehaviour
{
    public List<GridObjectParent> gridObjs = new List<GridObjectParent>();
    public InventoryGrid grid;
    public InventoryGrid.GridCubeData[,] gridData;

    public void OnEnable()
    {
        grid.AnnounceGridEvent += PlaceItems;
    }

    public void PlaceItems(InventoryGrid.GridCubeData[,] newData)
    {
        gridData = newData;
        List<InventoryGrid.GridCubeData> openSquares = FindOpenSquares();

        GridObjectParent obj;

        for (int i = 0; i < gridObjs.Count; i++)
        {
            obj = gridObjs[i];

            foreach (InventoryGrid.GridCubeData openSquare in openSquares)
            {
                if (AreSurroundingSquaresAccessible(openSquare, obj))
                {
                    // Update the grid object's position
                    Vector3 newPosition = new Vector3(openSquare.xPos, openSquare.yPos, 0);
                    obj.transform.position = newPosition;

                    // Update the parent position
                    obj.parentPosition = new Vector2(openSquare.xPos, openSquare.yPos);

                    // Mark the square as not 
                    grid.TargetCubesFlipEquipped(openSquare.xPos, openSquare.yPos, true);

                    // Mark the surrounding squares as not accessible
                    MarkSurroundingSquaresEquipped(openSquare, obj);

                    // Remove the used square from the list
                    openSquares.Remove(openSquare);

                    //Debug.Log("Placed grid object at " + openSquare.xPos + ", " + openSquare.yPos);
                    break;
                }
            }
        }
    }

    public void MarkSurroundingSquaresEquipped(InventoryGrid.GridCubeData centerSquare, GridObjectParent obj)
    {
        //scans the surrounding squares of the target square according to GridObjectParents list of Vector2's gridPositions
        foreach (Vector2 gridPosition in obj.gridPositions)
        {
            int xOffset = (int)gridPosition.x;
            int yOffset = (int)gridPosition.y;
            int x = centerSquare.xPos + xOffset;
            int y = centerSquare.yPos + yOffset;

            if (IsAccessible(x, y))
            {
                // Mark the square as not accessible
                grid.TargetCubesFlipEquipped(x, y, true);
            }
        }
    }

    public bool AreSurroundingSquaresAccessible(InventoryGrid.GridCubeData openSquare, GridObjectParent obj)
    {
        foreach (Vector2 gridPosition in obj.gridPositions)
        {
            int xOffset = (int)gridPosition.x;
            int yOffset = (int)gridPosition.y;
            int x = openSquare.xPos + xOffset;
            int y = openSquare.yPos + yOffset;

            if (!IsAccessible(x, y))
            {
                return false;
            }
        }

        return true;
    }

    public bool IsAccessible(int x, int y)
    {
        // Check if the square is within bounds and accessible
        if (x >= 0 && x < gridData.GetLength(0) && y >= 0 && y < gridData.GetLength(1))
        {
            return gridData[x, y].accessible;
        }

        return false;
    }

    public List<InventoryGrid.GridCubeData> FindOpenSquares()
    {
        List<InventoryGrid.GridCubeData> openSquares = new List<InventoryGrid.GridCubeData>();

        for (int x = 0; x < gridData.GetLength(0); x++)
        {
            for (int y = 0; y < gridData.GetLength(1); y++)
            {
                if (gridData[x, y].accessible)
                {
                    openSquares.Add(gridData[x, y]);
                }
            }
        }

        return openSquares;
    }

    void OnDisable()
    {
        grid.AnnounceGridEvent -= PlaceItems;
    }
}