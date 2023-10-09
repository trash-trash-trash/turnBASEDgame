using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuMain : MainMenuBase
{
    //load new game / continue game

    public GameObject textPrefab;

    public Dictionary<int, MainMenuStateEnum> statesDict;

    public Dictionary<int, Button> buttonsDict = new Dictionary<int, Button>();

    public Canvas canvasObj;

    public bool initialized = false;

    protected override void OnEnable()
    {
        base.OnEnable();

        if (!initialized)
        {
            statesDict = new Dictionary<int, MainMenuStateEnum>
            {
                { 0, MainMenuStateEnum.NewGame },
                { 1, MainMenuStateEnum.LoadGame },
                { 2, MainMenuStateEnum.Options },
                { 3, MainMenuStateEnum.Quit }
            };

            for (int i = 0; i < statesDict.Count; i++)
            {
                GameObject textBox = Instantiate(textPrefab, transform);
                textBox.name = statesDict[i].ToString();

                TMP_Text textComponent = textBox.GetComponentInChildren<TMP_Text>();
                textComponent.text = statesDict[i].ToString();

                Button newButton = textBox.GetComponentInChildren<Button>();

                textBox.transform.SetParent(canvasObj.transform, false);

                switch (i)
                {
                    case 0:
                        buttonsDict.Add(0, newButton);
                        newButton.onClick.AddListener(NewGame);
                        break;
                    case 1:
                        buttonsDict.Add(1, newButton);
                        newButton.onClick.AddListener(LoadGame);
                        break;
                    case 2:
                        buttonsDict.Add(2, newButton);
                        newButton.onClick.AddListener(Options);
                        break;
                    case 3:
                        buttonsDict.Add(3, newButton);
                        newButton.onClick.AddListener(Quit);
                        break;
                }
            }

            TMP_Text text = textPrefab.GetComponentInChildren<TMP_Text>();
            text.text = string.Empty;
            initialized = true;
        }


        maxSelectInt = 3;
        selectInt = 0;
        ChangeSelectInt(0);
    }

    protected override void Confirm()
    {
        base.Confirm();
        ChangeState();
    }

    protected override void Cancel()
    {
        base.Cancel();
        brain.ChangeMenuState(MainMenuStateEnum.PressStart);
    }

    protected override void ChangeSelectInt(int x)
    {
        base.ChangeSelectInt(x);
        Selectable selectable = buttonsDict[x].GetComponentInChildren<Selectable>();
        selectable.Select();

        foreach (Button butt in buttonsDict.Values)
        {
            Selectable newSelectable = butt.GetComponentInChildren<Selectable>();
            newSelectable.OnDeselect(null);
        }
    }

    public void NewGame()
    {
        selectInt = 0;
        ChangeState();
    }

    public void LoadGame()
    {
        selectInt = 1;
        ChangeState();
    }

    public void Options()
    {
        selectInt = 2;
        ChangeState();
    }

    public void Quit()
    {
        selectInt = 3;
        ChangeState();
    }

    public void ChangeState()
    {
        if (statesDict.ContainsKey(selectInt))
        {
            brain.ChangeMenuState(statesDict[selectInt]);
        }
    }
}