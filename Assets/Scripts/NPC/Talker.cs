using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talker : MonoBehaviour, ITalk
{
    //TODO: change to list? support multiple lines / branching lines / etc
    public string dialogue;

    public void OpenDialogue()
    {
        DialogueSingleton.Instance.OnOpenCloseDialogue(true);
        DialogueSingleton.Instance.OnNewDialogue(dialogue);
    }

    public void CloseDialogue()
    {

    }
}