using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAttackState : MonoBehaviour
{
    public NPCBrain brain;

    public BattleManagerSingleton battleManager;

    public void OnEnable()
    {
        battleManager = BattleManagerSingleton.BattleManagerSingletonInstance;
        battleManager.FightStartedEvent += Fight;
    }

    private void Fight()
    {
        brain.ChangeState(NPCBrain.NPCStates.Fight);
        brain.fighter.lookingToFight = false;
    }

    public void OnDisable()
    {
        battleManager.FightStartedEvent -= Fight;
    }
}
