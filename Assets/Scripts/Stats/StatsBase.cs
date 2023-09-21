using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatsBase : MonoBehaviour
{
    public int maxStat=999;
    public int currentStat;
    
    public event Action<StatsEnum, int> DeclareStatEvent;

    public StatsEnum testEnum;

    public int testAmount;

    public Dictionary<StatsEnum, int> statsDictionary = new Dictionary<StatsEnum, int>();
    public bool initialized = false;

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

        initialized = true;
    }

    public void TestChangeStat()
    {
        ChangeStat(testEnum, testAmount);
    }

    public void ChangeStat(StatsEnum statInput, int amount)
    {
        if (initialized)
        {
            if (statsDictionary.ContainsKey(statInput))
            {
                int newStatInt = statsDictionary[statInput] + amount;
                statsDictionary[statInput] = newStatInt;
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