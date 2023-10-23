using System.Collections.Generic;
using UnityEngine;

public class OldWomanBrain : MonoBehaviour
{
    public Talker talker;

    public bool givenApple;

    public List<string> string01 = new List<string>();
    public List<string> string02 = new List<string>();
    public List<string> string03 = new List<string>();

    private List<List<string>> stringLists = new List<List<string>>();

    private void Awake()
    {
        stringLists.Add(string01);
        stringLists.Add(string02);
        stringLists.Add(string03);
    }

    void OnEnable()
    {
        talker.CloseDialogueEvent += GiveApple;
        talker.CloseDialogueEvent += ChangeDialogue;

    }

    private void GiveApple()
    {
        // open inventory - > player drags apple into inventory
        givenApple = true;
        ChangeDialogue();
    }

    private void ChangeDialogue()
    {
        if (!givenApple)
            return;

        List<string> randomStringList = GetRandomStringList();

        if (randomStringList != null && randomStringList.Count > 0)
        {
            talker.ChangeDialogue(randomStringList);
        }
    }

    private List<string> GetRandomStringList()
    {
            int randomListIndex = Random.Range(0, stringLists.Count);
            return stringLists[randomListIndex];
        }

    void OnDisable()
    {
        talker.CloseDialogueEvent -= ChangeDialogue;
        talker.CloseDialogueEvent -= GiveApple;
    }
}