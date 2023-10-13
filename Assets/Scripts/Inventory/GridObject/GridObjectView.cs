using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObjectView : MonoBehaviour
{
    public GridObject gridObj;

    public Material neutral;

    public Material highlighted;

    public Material equipped;

    public MeshRenderer meshRend;

    public Material currentMaterial;

    public void OnEnable()
    {
        gridObj = GetComponentInParent<GridObject>();

        meshRend = GetComponent<MeshRenderer>();

        gridObj.AnnouncePickUpPutDownEvent += PickUpPutDown;
        gridObj.AnnounceHighlightEvent += Highlight;
    }

    private void Highlight(bool input)
    {
        if (gridObj.equipped)
            return;

        if (input)
            meshRend.material = highlighted;

        else
            meshRend.material = neutral;
    }

    private void PickUpPutDown(bool input)
    {
        if(input)
            meshRend.material = equipped;

        else
            meshRend.material = neutral;
    }

    private void OnDisable()
    {
        gridObj.AnnouncePickUpPutDownEvent -= PickUpPutDown;
        gridObj.AnnounceHighlightEvent -= Highlight;
    }
}
