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

    public bool LookingToFight()
    {
        return lookingToFight;
    }

    public void SetParty()
    {
        battleManager.SetParty(battleManager.playerTwoParty, party);
    }

    public void StartFight()
    {
        party = partyInventory.party;
        battleManager = BattleManagerSingleton.BattleManagerSingletonInstance;
        SetParty();
        battleManager.NPCStartFight(this);
        FightStartedEvent?.Invoke();
    }
}
