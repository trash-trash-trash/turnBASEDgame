using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuQuit : MainMenuBase
{
    public Button backButton;

    public Button quitButton;

    protected override void OnEnable()
    {
        base.OnEnable();

        buttonsList.Clear();
        buttonsList.Add(backButton);
        buttonsList.Add(quitButton);

        maxSelectInt = 1;
        ChangeSelectInt(0);
    }

    protected override void Confirm()
    {
        base.Confirm();
        if (selectInt == 0)
        {
            Back();
        }

        else
        {
            Quit();
        }
    }


    protected override void Cancel()
    {
        base.Cancel();
        Back();
    }

    public void Back()
    {
        brain.ChangeMenuState(MainMenuStateEnum.MainMenu);
    }

    public void Quit()
    {
#if UNITY_EDITOR
        // This code will execute only when in Unity Editor play mode
        UnityEditor.EditorApplication.isPlaying = false;
#else
    // This code will execute in a standalone build
    Application.Quit();
#endif
    }
}