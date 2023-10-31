    using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talker : MonoBehaviour, ITalk
{
    //TODO: change to list? support multiple lines / branching lines / etc
    [SerializeField] private List<string> dialogue;

    public event Action OpenDialogueEvent;
    public event Action CloseDialogueEvent;

    public bool canTalk;

    public DialogueSingleton dialogueSingleton;

    void OnEnable()
    {
        dialogueSingleton = DialogueSingleton.DiaglogueSingletonInstance;
    }

    public bool CanTalk()
    {
        return canTalk;
    }

    public void ChangeDialogue(List<string> newDialogue)
    {
        dialogue.Clear();
        dialogue.AddRange(newDialogue);
    }

    public void OpenDialogue()
    {
        if (!canTalk)
            return;
        
        dialogueSingleton.talker = this;
        dialogueSingleton.OnOpenCloseDialogue(true);
        dialogueSingleton.OnNewDialogue(dialogue);  
        OpenDialogueEvent?.Invoke();
    }

    public void CloseDialogue()
    {
        CloseDialogueEvent?.Invoke();
    }
}