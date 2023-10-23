using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.Windows;

[System.Serializable]
public class InventoryGrid : MonoBehaviour
{
    //TODO: CONSOLIDATE & MODULATE

    public bool selected;

    public int gridWidth;
    public int gridHeight;

    public GameObject gridCube;

    public GridCubeData[,] gridData;

    public event Action<GameObject, GridCubeType> AnnounceChangedCubeEvent;

    public event Action<GridCubeData[,]> AnnounceGridEvent;

    public float startXPosition = 0.0f;

    [System.Serializable]
    public struct GridCubeData
    {
        public GridCubeType currentType;
        public bool accessible;
        public int yPos;
        public int xPos;
        public GameObject me;
    }

    // Adjust this for the spacing between cubes
    // make this transform.size of prefab
    public float spacingX = 1.0f;
    public float spacingY = 1.0f;

    public enum GridCubeType
    {
        Open,
        Occupied,
        ShadowOn,
        ShadowOff,
        Blocked
    }

    public void Start()
    {
        CreateGrid();
    }

    public void CreateGrid()
    {
        gridData = new GridCubeData[gridWidth, gridHeight];

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                Vector3 spawnPosition = new Vector3((x + startXPosition) * spacingX, y * spacingY, 1);
                GameObject newCube = Instantiate(gridCube, spawnPosition, Quaternion.identity);

                newCube.transform.SetParent(transform);
                newCube.transform.name = "Grid Cube " + x + ", " + y;

                MeshRenderer newMesh = newCube.GetComponent<MeshRenderer>();

                GridCubeData cubeData = new GridCubeData
                {
                    currentType = GridCubeType.Open,
                    accessible = true,
                    xPos = x,
                    yPos = y,
                    me = newCube
                };

                gridData[x, y] = cubeData;

                bool isOnBoundary = x == 0 || x == gridWidth - 1 || y == 0 || y == gridHeight - 1;

                if (isOnBoundary)
                {
                    ChangeTargetCubeType(x, y, GridCubeType.Occupied);
                }
            }
        }

        AnnounceGridEvent?.Invoke(gridData);
    }


    public void ChangeTargetCubeType(int targetX, int targetY, GridCubeType newType)
    {
        if (targetX >= 0 && targetX < gridData.GetLength(0) && targetY >= 0 && targetY < gridData.GetLength(1))
        {
            gridData[targetX, targetY].currentType = newType;

            GridCubeType type = newType;

            if (newType == GridCubeType.Occupied)
            {
                gridData[targetX, targetY].accessible = false;
                type = GridCubeType.Occupied;
            }

            else if (newType == GridCubeType.Open)
            {
                gridData[targetX, targetY].accessible = true;
                type = GridCubeType.Open;
            }

            else if (newType == GridCubeType.ShadowOn)
            {
                if (!gridData[targetX, targetY].accessible)
                    type = GridCubeType.Blocked;
            }

            else if (newType == GridCubeType.ShadowOff)
            {
                if (gridData[targetX, targetY].accessible)
                    type = GridCubeType.Open;
                else
                    type = GridCubeType.Occupied;
            }

            AnnounceChangedCubeEvent?.Invoke(gridData[targetX, targetY].me, type);
        }
    }
}