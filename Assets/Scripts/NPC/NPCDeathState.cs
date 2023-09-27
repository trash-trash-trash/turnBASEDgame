using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDeathState : MonoBehaviour
{
    public NPCBrain brain;

    public string deathDescription;

    public List<string> deathDialogue = new List<string>();

    public void OnEnable()
    {
        brain.isAlive = false;

        if (brain.party.Count == 1)
            deathDescription = "The corpse of a " + brain.party[0].name;

        else
            deathDescription = "A pile of " + brain.party[0].name + " corpses.";

        if (!deathDialogue.Contains(deathDescription))
        {
            deathDialogue.Add(deathDescription);
        }

        brain.talker.ChangeDialogue(deathDialogue);
    }
}