using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.Windows;

public class InventoryGrid : MonoBehaviour
{
    public int gridSize;
    public GameObject gridCube;

    public GridCubeData[,] gridData;

    public event Action<GameObject, bool> AnnounceCubeAccessibleEvent;

    public event Action<GameObject, bool> AnnounceCubeEquippedEvent;

    public event Action<GameObject, bool> AnnounceCubeShadowEvent;

    public event Action<GridCubeData[,]> AnnounceGridEvent;

    public int targetX;

    public int targetY;

    public bool flipAccessible;

    public bool flipEquipped;

    public InventoryCustomShapes.InventoryShapeEnum customTestShape;

    public InventoryCustomShapes shapes;

    public struct GridCubeData
    {
        public bool accessible;
        public int yPos;
        public int xPos;
        public bool equipped;
        public bool shadow;
        public GameObject me;
    }

    // Adjust this for the spacing between cubes
    // make this transform.size of prefab
    public float spacing = 1.0f;

    public void Start()
    {
        CreateGrid();
    }

    public void CreateGrid()
    {
        int gridDimension = (int)Mathf.Sqrt(gridSize);

        gridData = new GridCubeData[gridDimension, gridDimension];

        for (int x = 0; x < gridDimension; x++)
        {
            for (int y = 0; y < gridDimension; y++)
            {
                Vector3 spawnPosition = new Vector3(x * spacing, y * spacing, 1);
                GameObject newCube = Instantiate(gridCube, spawnPosition, Quaternion.identity);

                newCube.transform.SetParent(transform);
                newCube.transform.name = "Grid Cube " + x + ", " + y;

                MeshRenderer newMesh = newCube.GetComponent<MeshRenderer>();

                GridCubeData cubeData = new GridCubeData
                {
                    accessible = true,
                    xPos = x,
                    yPos = y,
                    equipped = false,
                    shadow = false,
                    me = newCube
                };

                gridData[x, y] = cubeData;

                bool isOnBoundary = x == 0 || x == gridDimension - 1 || y == 0 || y == gridDimension - 1;

                if (isOnBoundary)
                    TargetCubesFlipAccessible(x, y, false);
            }
        }

        AnnounceGridEvent?.Invoke(gridData);
    }

    public void TestCustomShapeEquip()
    {
        List<Vector2Int> customShapePoints = shapes.shapesDictionary[customTestShape];

        foreach (Vector2Int point in customShapePoints)
        {
            int newtargetX = point.x;
            int newtargetY = point.y;

            if (newtargetX >= 0 && newtargetX < gridData.GetLength(0) && newtargetY >= 0 && newtargetY < gridData.GetLength(1))
            {
                if (!gridData[newtargetX, newtargetY].accessible)
                    return;

                TargetCubesFlipEquipped(newtargetX, newtargetY, true);
            }
        }
    }

    public void TestTargetCubesFlipAccessible()
    {
        TargetCubesFlipAccessible(targetX, targetY, flipAccessible);
    }

    public void TargetCubesFlipAccessible(int targetX, int targetY, bool input)
    {
        if (targetX >= 0 && targetX < gridData.GetLength(0) && targetY >= 0 && targetY < gridData.GetLength(1))
        {
            gridData[targetX, targetY].accessible = input;
            AnnounceCubeAccessibleEvent?.Invoke(gridData[targetX, targetY].me, input);
        }
    }

    public void TestTargetCubesFlipEquipped()
    {
        TargetCubesFlipEquipped(targetX, targetY, flipEquipped);
    }

    public void TargetCubesFlipEquipped(int targetX, int targetY, bool input)
    {
        if (targetX >= 0 && targetX < gridData.GetLength(0) && targetY >= 0 && targetY < gridData.GetLength(1))
        {
            gridData[targetX, targetY].equipped = input;
            gridData[targetX, targetY].accessible = !input;
            AnnounceCubeEquippedEvent?.Invoke(gridData[targetX, targetY].me, input);
        }
    }

    public void TargetCubesFlipShadow(int targetX, int targetY, bool input)
    {
        if (targetX >= 0 && targetX < gridData.GetLength(0) && targetY >= 0 && targetY < gridData.GetLength(1))
        {
            gridData[targetX, targetY].shadow = input;
            AnnounceCubeShadowEvent?.Invoke(gridData[targetX, targetY].me, input);
        }
    }
}