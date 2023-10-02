using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealerAI : MonoBehaviour
{
    public List<AttackData> attacks = new List<AttackData>();

    public TurnController controller;

    public TurnTaker turnTaker;

    public void OnEnable()
    {

    }

    private void OnDisable()
    {

    }

    // Define a structure to hold attack data
    public struct AttackData
    {
        public PartyMemberScriptableObject attacker;
        public PartyMemberScriptableObject target;
        public ActionScriptableObject attack;

        public AttackData(PartyMemberScriptableObject newAttacker, PartyMemberScriptableObject newTarget, ActionScriptableObject newAttack)
        {
            attacker = newAttacker;
            target = newTarget;
            attack = newAttack;
        }
    }

    public void AddAttack(PartyMemberScriptableObject newAttacker, PartyMemberScriptableObject newTarget,
        ActionScriptableObject newAttack)
    {
        AttackData attackData = new AttackData(newAttacker, newTarget, newAttack);
        attacks.Add(attackData);
    }

    public void CalculateAttacks()
    {
        //sort by speed
        attacks.Sort((a, b) => a.attacker.Speed.CompareTo(b.attacker.Speed));

        for (int i = 0; i < attacks.Count; i++)
        {
            ActionScriptableObject.ActionDataStruct data = attacks[i].attack.actionData;

            for (int j = 0; j < data.statsAffectedList.Count; j++)
            {
                attacks[i].target.stats.ChangeStat(data.statsAffectedList[j], data.statsAmounts[j]);
            }
        }

        turnTaker.EndTurn();
    }
}