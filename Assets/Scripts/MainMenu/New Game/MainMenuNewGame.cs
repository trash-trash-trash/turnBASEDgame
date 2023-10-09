using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuNewGame : MainMenuBase
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
}
