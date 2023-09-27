using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCFightState : MonoBehaviour
{
    public NPCBrain brain;

    public BattleManagerSingleton battleManager;

    public void OnEnable()
    {
        brain.fighter.lookingToFight = false;
        battleManager = BattleManagerSingleton.BattleManagerSingletonInstance;
        battleManager.FightEndedEvent += Death;

        brain.fighter.StartFight();
    }

    public void Death()
    {
        brain.ChangeState(NPCBrain.NPCStates.Death);
    }

    public void OnDisable()
    {
        battleManager.FightEndedEvent -= Death;
    }
}
