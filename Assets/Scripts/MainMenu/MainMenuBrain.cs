using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class MainMenuBrain : MonoBehaviour
{
    public GameObjectStateManager stateManager;

    public GameObject pressStart;

    public GameObject newGame;

    public GameObject loadGame;

    public GameObject mainMenu;

    public GameObject options;

    public GameObject quit;

    public Dictionary<MainMenuStateEnum, GameObject> menuStatesDictionary =
        new Dictionary<MainMenuStateEnum, GameObject>();

    private void Start()
    {
        menuStatesDictionary.Add(MainMenuStateEnum.PressStart, pressStart);

        menuStatesDictionary.Add(MainMenuStateEnum.MainMenu, mainMenu);

        menuStatesDictionary.Add(MainMenuStateEnum.NewGame, newGame);

        menuStatesDictionary.Add(MainMenuStateEnum.LoadGame, loadGame);

        menuStatesDictionary.Add(MainMenuStateEnum.Options, options);

        menuStatesDictionary.Add(MainMenuStateEnum.Quit, quit);

        ChangeMenuState(MainMenuStateEnum.PressStart);
    }

    public void ChangeMenuState(MainMenuStateEnum state)
    {
        if (menuStatesDictionary.ContainsKey(state))
        {
            GameObject menuStateObject = menuStatesDictionary[state];
            stateManager.ChangeState(menuStateObject);
        }
        else
        {
            Debug.LogWarning("No key");
        }
    }
}
