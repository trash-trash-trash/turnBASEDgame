using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class DialogueSingleton : MonoBehaviour
{
    //IncrementallyDisplayText allows the typewriter effect, displays text one char at a time
    //waits until waitForNextLine is False if there are lines remaining
    //NextLine() detects if there are more lines to display or closes dialogue

    private static DialogueSingleton _instance;

    #region Singleton

    public static DialogueSingleton Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<DialogueSingleton>();

                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject("singleton");
                    _instance = singletonObject.AddComponent<DialogueSingleton>();
                }
            }

            return _instance;
        }
    }


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    #endregion

    public event Action<bool> OpenCloseDialogueEvent;

    public event Action<string> NewDialogueEvent;

    public ITalk talker;

    public List<string> lines;
    public float delayBetweenCharacters;
    public float minDelayBetweenCharacters;
    private float originalDelay;

    public int currentLineIndex = 0;
    private int characterIndex = 0;
    private bool displayComplete = false;
    private bool waitForNextLine = false;

    public event Action NextLineEvent;

    private IEnumerator displayText;

    private IEnumerator nextLine;

    public bool nextLinePressed = false;
    public bool talking = false;

    public void OnNewDialogue(List<string> newLines)
    {
        originalDelay = delayBetweenCharacters;

        lines = newLines;
        currentLineIndex = 0;
        characterIndex = 0;

        displayText = IncrementallyDisplayText();
        StartCoroutine(displayText);
    }

    private IEnumerator IncrementallyDisplayText()
    {
        while (currentLineIndex < lines.Capacity)
        {
            talking = true;
            if (nextLinePressed)
            {
                delayBetweenCharacters = minDelayBetweenCharacters;
            }
            else
            {
                delayBetweenCharacters = originalDelay;
            }
            if (characterIndex < lines[currentLineIndex].Length)
            {
                NewDialogueEvent?.Invoke(lines[currentLineIndex][characterIndex].ToString());
                characterIndex++;
            }
            else
            {
                delayBetweenCharacters = originalDelay;
                talking = false;
                waitForNextLine = true;
                yield return new WaitUntil(() => waitForNextLine == false);
            }

            yield return new WaitForSeconds(delayBetweenCharacters);
        }
    }

    public void NextLine()
    {
        nextLinePressed = true;

        if (!waitForNextLine || talking)
            return;

        characterIndex = 0;
        currentLineIndex++;

        if (currentLineIndex < lines.Capacity)
        {
            NextLineEvent?.Invoke();
            displayComplete = false;
            waitForNextLine = false;
            nextLinePressed = false;
        }
        else
        {
            nextLinePressed = false;
            displayComplete = true;
            OnOpenCloseDialogue(false);
        }
    }

    public void OnOpenCloseDialogue(bool input)
    {
        OpenCloseDialogueEvent?.Invoke(input);

        if (!input && talker != null)
            talker.CloseDialogue();
    }
}