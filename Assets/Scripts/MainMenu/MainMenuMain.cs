using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuMain : MainMenuBase
{
    //load new game / continue game

    public bool loadGame = false;

    public void NewGame()
    {
        if (loadGame)
            brain.ChangeMenuState(MainMenuStateEnum.LoadGame);

        brain.ChangeMenuState(MainMenuStateEnum.NewGame);
    }

    public void Options()
    {
        brain.ChangeMenuState(MainMenuStateEnum.Options);
    }

    public void Quit()
    {
        brain.ChangeMenuState(MainMenuStateEnum.Quit);
    }
}