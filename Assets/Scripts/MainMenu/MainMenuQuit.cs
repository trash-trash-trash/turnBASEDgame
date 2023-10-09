using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuQuit : MainMenuBase
{
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