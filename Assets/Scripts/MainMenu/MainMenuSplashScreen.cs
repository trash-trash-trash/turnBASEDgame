using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuSplashScreen : MainMenuBase
{
    //splash screen fades in over fadeTime, pauses for the duration of hangTime, fades back out
    //plays Lloyds_Sting thru SFX Singleton

    public float fadeTime;

    public float hangTime;

    public Image image;

    public AudioSource audioSource;

    public SFXSingleton SFX;

    protected override void OnEnable()
    {
        base.OnEnable();

        SFX = SFXSingleton.Instance;
        SFX.PlayClip("Lloyds_Sting", audioSource);

        StartCoroutine(FadeInOut());
    }

    IEnumerator FadeInOut()
    {
        // Fade In
        float elapsedTime = 0f;
        Color startColor = image.color;
        Color targetColorIn = new Color(startColor.r, startColor.g, startColor.b, 1f);

        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            image.color = Color.Lerp(startColor, targetColorIn, elapsedTime / fadeTime);
            yield return null;
        }

        image.color = targetColorIn;
        
        yield return new WaitForSeconds(hangTime);

        elapsedTime = 0f;
        Color targetColorOut = new Color(startColor.r, startColor.g, startColor.b, 0f);

        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            image.color = Color.Lerp(targetColorIn, targetColorOut, elapsedTime / fadeTime);
            yield return null;
        }

        image.color = targetColorOut;

        brain.ChangeMenuState(MainMenuStateEnum.PressStart);
    }
}