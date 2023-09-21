using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyMember : MonoBehaviour
{
    public StatsBase stats;

    public PartyMemberScriptableObject scriptableObject;

    public void SetStats()
    {
        stats.ChangeStat(StatsEnum.HP, scriptableObject.HP);
        stats.ChangeStat(StatsEnum.Mind, scriptableObject.Mind);
        stats.ChangeStat(StatsEnum.Speed, scriptableObject.Speed);
        stats.ChangeStat(StatsEnum.Accuracy, scriptableObject.Accuracy);
        stats.ChangeStat(StatsEnum.PhysDamage, scriptableObject.PhysDamage);
        stats.ChangeStat(StatsEnum.PhyDefense, scriptableObject.PhysDefense);
        stats.ChangeStat(StatsEnum.MagDamage, scriptableObject.MagDamage);
        stats.ChangeStat(StatsEnum.MagDefense, scriptableObject.MagDefense);
        stats.ChangeStat(StatsEnum.Luck, scriptableObject.Luck);
    }
    
}
