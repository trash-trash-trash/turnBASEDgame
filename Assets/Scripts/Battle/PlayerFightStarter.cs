using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFightStarter : MonoBehaviour, IStartFights
{
    public PartyInventory partyInventory;

    public BattleManagerSingleton battleManager;

    public IStartFights startFightsComponent;

    public bool lookingToFight = true;

    public void OnEnable()
    {
        battleManager = BattleManagerSingleton.BattleManagerSingletonInstance;
        battleManager.NPCStartedFightEvent += NPCStartedFight;
    }

    private void OnTriggerEnter(Collider other)
    {
        IStartFights startFightsComponent = other.gameObject.GetComponent<IStartFights>();

        if (startFightsComponent != null)
        {
            if (!startFightsComponent.LookingToFight())
                return;

            StartFight();
        }
    }
    public void SetParty()
    {
        battleManager.SetParty(battleManager.playerOneParty, partyInventory.party);
    }

    public bool LookingToFight()
    {
        return lookingToFight;
    }

    private void NPCStartedFight(IStartFights fighter)
    {
        startFightsComponent = fighter;
        StartFight();
    }

    public void StartFight()
    {
        if (startFightsComponent != null)
        {
            SetParty();
            startFightsComponent.SetParty();
            battleManager.StartFight();
        }
    }
}

