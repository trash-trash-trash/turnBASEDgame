using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatsBase : MonoBehaviour
{
    public sbyte maxStat = 100;
    public sbyte currentStat;
    
    public event Action<StatsEnum, sbyte> DeclareStatEvent;

    public StatsEnum testEnum;

    public sbyte testAmount;

    public Dictionary<StatsEnum, sbyte> statsDictionary = new Dictionary<StatsEnum, sbyte>();
    public bool initialised = false;

    public void Awake()
    {
        SetDictionary();
    }

    public void SetDictionary()
    {
        statsDictionary.Add(StatsEnum.HP, maxStat);
        statsDictionary.Add(StatsEnum.Accuracy, maxStat);
        statsDictionary.Add(StatsEnum.Mind, maxStat);
        statsDictionary.Add(StatsEnum.PhysDamage, maxStat);
        statsDictionary.Add(StatsEnum.PhyDefense, maxStat);
        statsDictionary.Add(StatsEnum.MagDamage, maxStat);
        statsDictionary.Add(StatsEnum.Speed, maxStat);
        statsDictionary.Add(StatsEnum.Luck, maxStat);

        initialised = true;
    }

    public void TestChangeStat()
    {
        ChangeStat(testEnum, testAmount);
    }

    public void ChangeStat(StatsEnum statInput, sbyte amount)
    {
        if (initialised)
        {
            if (statsDictionary.ContainsKey(statInput))
            {
                int newStat = statsDictionary[statInput] + amount;
                statsDictionary[statInput] = (sbyte)Mathf.Clamp(newStat, 0, maxStat);
                currentStat = statsDictionary[statInput];

                if (currentStat <= 0)
                    currentStat = 0;

                else if (amount >= maxStat)
                {
                    currentStat = maxStat;
                }

                DeclareStatEvent?.Invoke(statInput, currentStat);
            }
        }
    }
}