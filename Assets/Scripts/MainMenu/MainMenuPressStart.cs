using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuPressStart : MainMenuBase
{
    public void PressStart()
    {
        brain.ChangeMenuState(MainMenuStateEnum.MainMenu);
    }
}
