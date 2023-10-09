using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuPressStart : MainMenuBase
{
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
