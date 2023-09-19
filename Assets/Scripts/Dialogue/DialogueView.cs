using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;

public class DialogueView : MonoBehaviour
{
    public GameObject dialogueCanvas;

    public TMP_Text dialogueText;

    public DialogueSingleton singleton;

    void Start()
    {
        singleton = DialogueSingleton.Instance;

        singleton.OpenCloseDialogueEvent += FlipCanvasOnOff;
        singleton.NewDialogueEvent += ChangeText;
    }

    private void OnDisable()
    {
        singleton.OpenCloseDialogueEvent -= FlipCanvasOnOff;
        singleton.NewDialogueEvent -= ChangeText;
    }

    private void FlipCanvasOnOff(bool input)
    {
        dialogueCanvas.SetActive(input);
    }

    private void ChangeText(string input)
    {
        dialogueText.text = input;
    }
}
