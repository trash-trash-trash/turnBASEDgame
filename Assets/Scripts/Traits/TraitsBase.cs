using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraitsBase : MonoBehaviour
{
    public StatsBase stats;

    public ITakeTurn turnTaker;

    public List<StatsEnum> TargetStatsList = new List<StatsEnum>();

    public bool evolved;

    public void ChangeEvolved(bool input)
    {
        evolved = input;
    }

    public void AddTargetStat(StatsEnum input)
    {
        if(!TargetStatsList.Contains(input))
            TargetStatsList.Add(input);
    }

    public void RemoveTargetStat(StatsEnum input)
    {
        if(TargetStatsList.Contains(input))
            TargetStatsList.Remove(input);
    }

    public void ChangeEffectStats(StatsEnum targetInput, sbyte amount)
    {
        if(TargetStatsList.Contains(targetInput))
            stats.ChangeStat(targetInput, amount);
    }
}
