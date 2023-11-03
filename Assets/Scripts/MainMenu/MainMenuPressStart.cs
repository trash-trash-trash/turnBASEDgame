using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenuPressStart : MainMenuBase
{
    public TMP_Text buttonText;
    public TMP_Text sunderLogo;
    public float fadeTime;

    protected override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(FadeInOut(buttonText));
        StartCoroutine(FadeInOut(sunderLogo));
    }

    IEnumerator FadeInOut(TMP_Text textElement)
    {
        // Fade In
        float elapsedTime = 0f;
        Color startColor = textElement.color;
        Color targetColorIn = new Color(startColor.r, startColor.g, startColor.b, 1f);

        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            textElement.color = Color.Lerp(startColor, targetColorIn, elapsedTime / fadeTime);
            yield return null;
        }

        textElement.color = targetColorIn;
    }

    protected override void Confirm()
    {
        base.Confirm();
        PressStart();
    }

    public void PressStart()
    {
        brain.ChangeMenuState(MainMenuStateEnum.MainMenu);
    }
}