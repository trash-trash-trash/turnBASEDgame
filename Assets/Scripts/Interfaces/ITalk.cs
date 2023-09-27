using System;
using System.Collections.Generic;

public interface ITalk
{
    public event Action OpenDialogueEvent ;

    public event Action CloseDialogueEvent ;

    public void ChangeDialogue(List<string> newDialogue);

    public void OpenDialogue();

    public void CloseDialogue();

    public bool CanTalk();
}