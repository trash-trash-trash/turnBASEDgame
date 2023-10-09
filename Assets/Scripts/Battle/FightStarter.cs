using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightStarter : MonoBehaviour, IStartFights
{
    public List<PartyMemberScriptableObject> party;

    public BattleManagerSingleton battleManager;

    public bool lookingToFight;

    public event Action FightStartedEvent;

    public PartyInventory partyInventory;

    public void OnEnable()
    {
        battleManager = BattleManagerSingleton.BattleManagerSingletonInstance;

        party = partyInventory.party;

        lookingToFight = true;
    }

    public bool LookingToFight()
    {
        return lookingToFight;
    }


    public void StartFight()
    {
        battleManager.SetParty(battleManager.playerTwoParty, party);
        battleManager.NPCStartFight(this);
        FightStartedEvent?.Invoke();
    }
}
