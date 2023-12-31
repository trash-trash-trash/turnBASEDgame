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

    #region Singleton

    private static DialogueSingleton instance;

    public static DialogueSingleton DiaglogueSingletonInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<DialogueSingleton>();

                if (instance == null)
                {
                    GameObject singletonObject = new GameObject("singleton");
                    instance = singletonObject.AddComponent<DialogueSingleton>();
                }
            }

            return instance;
        }
    }


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    #endregion

    public event Action<bool> OpenCloseDialogueEvent;

    public event Action<string> NewDialogueEvent;

    public ITalk talker;

    public List<string> lines;
    public float currentDelay;
    public float delayBetweenCharacters;
    public float minDelayBetweenCharacters;
    private float originalDelay;

    public int currentLineIndex = 0;
    public int characterIndex = 0;
    private bool waitForNextLine = false;

    public event Action NextLineEvent;

    private IEnumerator displayText;

    private IEnumerator nextLine;

    public bool nextLinePressed = false;
    public bool talking = false;

    void Start()
    {
        originalDelay = delayBetweenCharacters;
        currentDelay = delayBetweenCharacters;
    }

    public void OnNewDialogue(List<string> newLines)
    {
        characterIndex = 0;
        lines = newLines;
        currentLineIndex = 0;

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
                currentDelay = minDelayBetweenCharacters;
            }
            else
            {
                currentDelay = originalDelay;
            }

            if (currentLineIndex >= 0 && currentLineIndex < lines.Count)
            {
                if (characterIndex < lines[currentLineIndex].Length)
                {
                    NewDialogueEvent?.Invoke(lines[currentLineIndex][characterIndex].ToString());
                    characterIndex++;
                }
                else
                {
                    currentDelay = originalDelay;
                    talking = false;
                    nextLinePressed = false;
                    waitForNextLine = true;
                }
            }

            yield return new WaitForSeconds(currentDelay);
        }

        yield return new WaitUntil(() => waitForNextLine == false);
    }

    public void NextLine()
    {
        nextLinePressed = true;

        if (!waitForNextLine || talking)
            return;

        characterIndex = 0;
        currentLineIndex++;

        if (currentLineIndex < lines.Count)
        {
            waitForNextLine = false;
            NextLineEvent?.Invoke();
            nextLinePressed = false;
        }
        else
        {
            nextLinePressed = false;
            OnOpenCloseDialogue(false);
        }
    }

    public void OnOpenCloseDialogue(bool input)
    {
        OpenCloseDialogueEvent?.Invoke(input);

        if (!input && talker != null)
        {
            talker.CloseDialogue();
        }
    }
}