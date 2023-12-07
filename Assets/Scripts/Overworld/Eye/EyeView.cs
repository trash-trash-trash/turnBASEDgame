using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeView : MonoBehaviour
{
    public EyeBrain brain;

    public SpriteRenderer spr;

    public Sprite idlePupil;

    public Sprite angryPupil;

    public void OnEnable()
    {
        brain.AnnouncePlayerDetected += ChangePupil;
    }

    public void ChangePupil(bool input)
    {
        if (!input)
            spr.sprite = idlePupil;
        else
            spr.sprite = angryPupil;
    }

    public void OnDisable()
    {
        brain.AnnouncePlayerDetected -= ChangePupil;
    }
}
