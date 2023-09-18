using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TurnControllerView : MonoBehaviour
{
    public TMP_Text text;

    public TurnController controller;

    public void OnEnable()
    {
        controller.AnnounceControllerString += ChangeText;
    }

    public void ChangeText(string input)
    {
        text.text = input;
    }

    public void OnDisable()
    {
        controller.AnnounceControllerString -= ChangeText;
    }
}
