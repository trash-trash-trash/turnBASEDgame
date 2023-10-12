using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryCustomShapes : MonoBehaviour
{
    public enum InventoryShapeEnum
    {
        tBlock0,
        tBlock90,
        tBlock180,
        tBlock270
    }

    public Dictionary<InventoryShapeEnum, List<Vector2Int>> shapesDictionary =
        new Dictionary<InventoryShapeEnum, List<Vector2Int>>();

    private List<Vector2Int> TBlock0 = new List<Vector2Int>
    {
        new Vector2Int(0, 0),
        new Vector2Int(0, -1),
        new Vector2Int(-1, 0),
        new Vector2Int(1, 0)
    };


    private List<Vector2Int> TBlock90 = new List<Vector2Int>
    {
        new Vector2Int(0, 0),
        new Vector2Int(0, -1),
        new Vector2Int(0, 1),
        new Vector2Int(-1, 0)
    };



    private List<Vector2Int> TBlock180 = new List<Vector2Int>
    {
        new Vector2Int(0, 0),
        new Vector2Int(-1, 0),
        new Vector2Int(0, 1),
        new Vector2Int(0, 1)
    };



    private List<Vector2Int> TBlock270 = new List<Vector2Int>
    {
        new Vector2Int(0, 0),
        new Vector2Int(0, 1),
        new Vector2Int(0, -1),
        new Vector2Int(-1, 0)
    };

    public void OnEnable()
    {
        shapesDictionary.Add(InventoryShapeEnum.tBlock0, TBlock0);
        shapesDictionary.Add(InventoryShapeEnum.tBlock90, TBlock90);
        shapesDictionary.Add(InventoryShapeEnum.tBlock180, TBlock180);
        shapesDictionary.Add(InventoryShapeEnum.tBlock270, TBlock270);
    }

}
