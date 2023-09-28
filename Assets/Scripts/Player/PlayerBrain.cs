using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class PlayerBrain : MonoBehaviour
{
    public GameObjectStateManager stateManager;

    public BattleManagerSingleton battleManager;

    public enum PlayerStates
    {
        Overworld,
        Combat
    }

    public PlayerStates currentState;

    public GameObject overworldState;

    public GameObject combatState;

    public Dictionary<PlayerStates, GameObject> statesDictionary = new Dictionary<PlayerStates, GameObject>();

    private void OnEnable()
    {
        battleManager = BattleManagerSingleton.BattleManagerSingletonInstance;

        statesDictionary.Add(PlayerStates.Overworld, overworldState);
        statesDictionary.Add(PlayerStates.Combat, combatState);

        battleManager.FightStartedEvent += FightStarted;
        battleManager.NPCStartedFightEvent += NPCFightStarted;
        battleManager.FightEndedEvent += FightEnded;

        ChangeState(currentState);
    }

    private void OnDisable()
    {
        battleManager.FightStartedEvent -= FightStarted;
        battleManager.NPCStartedFightEvent -= NPCFightStarted;
        battleManager.FightEndedEvent -= FightEnded;
    }


    private void FightStarted()
    {
        ChangeState(PlayerStates.Combat);
    }

    private void NPCFightStarted(IStartFights startFights)
    {
        FightStarted();
    }
   private void FightEnded()
    {
        ChangeState(PlayerStates.Overworld);
    }

    public void ChangeState(PlayerStates newState)
    {
        if (statesDictionary.TryGetValue(newState, out GameObject stateObject))
        {
            stateManager.ChangeState(stateObject);
        }
        else
        {
            //think state goes here
            Debug.LogError("State not found: " + newState);
        }
    }
}
