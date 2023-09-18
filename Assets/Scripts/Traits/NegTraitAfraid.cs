using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NegTraitAfraid : TraitsBase, IStatus
{
    public sbyte accuracyDecreaseAmount;

    public sbyte speedIncreaseAmount;

    public void SetStatus()
    {
        AddTargetStat(StatsEnum.Accuracy);
        AddTargetStat(StatsEnum.Speed);
        
        ChangeEffectStats(StatsEnum.Accuracy, accuracyDecreaseAmount);
        ChangeEffectStats(StatsEnum.Speed, speedIncreaseAmount);

        if(evolved)
            Terrified();
    }

    public void SetOwner(StatsBase inputStats)
    {
        stats = inputStats;
    }

    public void Terrified()
    {
        float randomValue = Random.Range(0f, 1f);
        
        if (randomValue <= 0.25f)
        {
            turnTaker.EndTurn();
        }
    }
}
