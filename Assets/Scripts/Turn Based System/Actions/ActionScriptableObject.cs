using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewActionScriptableObject", menuName = "ActionScriptableObject")]
public class ActionScriptableObject : ScriptableObject
{
    public ActionsSingleton.Actions actionType;

    public enum PartyTargetType
    {
        SingleAlly,
        MultipleAlly,
        SingleEnemy,
        MultipleEnemy,
        All
    }

    public PartyTargetType actionTargetType;
    public string actionName;
    public string actionDescription;
    public List<StatsEnum> actionStatsAffectedList;
    public List<int> actionStatsAmounts;
    public List<NegativeTraits> actionNegativeTraits;
    public List<float> actionNegativeTraitChance;
    public int actionAmountPartyMembersEffected;

    public ActionDataStruct actionData;

    public void OnEnable()
    {
        actionData = new ActionDataStruct(actionTargetType, actionName, actionDescription, actionStatsAffectedList,
            actionStatsAmounts, actionNegativeTraits, actionNegativeTraitChance, actionAmountPartyMembersEffected);
    }

    public struct ActionDataStruct
    {
        public PartyTargetType targetType;
        public string name;
        public string description;
        public List<StatsEnum> statsAffectedList;
        public List<int> statsAmounts;
        public List<NegativeTraits> negativeTraits;
        public List<float> negativeTraitChance;
        public int amountPartyMembersEffected;

        public ActionDataStruct(PartyTargetType targetType, string name, string description,
            List<StatsEnum> statsAffectedList, List<int> statsAmounts, List<NegativeTraits> negativeTraits,
            List<float> negativeTraitChance, int amountPartyMembersEffected)
        {
            this.targetType = targetType;
            this.name = name;
            this.description = description;
            this.statsAffectedList = statsAffectedList;
            this.statsAmounts = statsAmounts;
            this.negativeTraits = negativeTraits;
            this.negativeTraitChance = negativeTraitChance;
            this.amountPartyMembersEffected = amountPartyMembersEffected;
        }
    }
}