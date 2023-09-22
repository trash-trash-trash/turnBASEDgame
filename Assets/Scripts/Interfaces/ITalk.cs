using System;

public interface ITalk
{
    public event Action OpenDialogueEvent ;

    public event Action CloseDialogueEvent ;

    public void OpenDialogue();

    public void CloseDialogue();    
}