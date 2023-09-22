using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talker : MonoBehaviour, ITalk
{
    //TODO: change to list? support multiple lines / branching lines / etc
    public string dialogue;

    public event Action OpenDialogueEvent;
    public event Action CloseDialogueEvent;

    public void OpenDialogue()
    {
        DialogueSingleton.Instance.talker = this;
        DialogueSingleton.Instance.OnOpenCloseDialogue(true);
        DialogueSingleton.Instance.OnNewDialogue(dialogue);
        OpenDialogueEvent?.Invoke();
    }

    public void CloseDialogue()
    {
        CloseDialogueEvent?.Invoke();
    }
}