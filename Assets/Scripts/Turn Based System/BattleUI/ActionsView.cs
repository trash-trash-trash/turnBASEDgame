using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PartyController;

public class ActionsView : MonoBehaviour
{
    public Material defaultMat;

    public Material highlightedMat;

    public List<SpriteRenderer> spr = new List<SpriteRenderer>();

    public PartyController controller;

    public int selectInt;

    public bool actionOn = false;

    public void OnEnable()
    {
        controller.SelectIntEvent += ChangeActionHighlight;
        controller.ActionSelectEvent += ActionsOn;
    }

    private void ActionsOn(bool input)
    {
        actionOn = input;

        if (input)
        {
            selectInt = 0;

            spr[selectInt].material = highlightedMat;
        }
        else
        {
            foreach (SpriteRenderer newSpr in spr)
            {
                newSpr.material = defaultMat;
            }
        }
    }

    private void ChangeActionHighlight(int x)
    {
        if (actionOn)
        {
            selectInt = x;

            spr[selectInt].material = highlightedMat;

            foreach (SpriteRenderer newSpr in spr)
            {
                if (newSpr != spr[selectInt])
                {
                    newSpr.material = defaultMat;
                }
            }
        }
    }

    public void OnDisable()
    {
        controller.SelectIntEvent -= ChangeActionHighlight;
        controller.ActionSelectEvent -= ActionsOn;
    }
}
