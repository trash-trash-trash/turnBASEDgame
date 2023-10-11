using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatsBase : MonoBehaviour
{
    public PartyMemberScriptableObject member;

    public string name;

    public int maxStat=999;
    public int currentStat;
    
    public event Action<StatsEnum, int> DeclareStatEvent;

    public StatsEnum testEnum;

    public int testAmount;

    public Dictionary<StatsEnum, int> statsDictionary = new Dictionary<StatsEnum, int>();
    public bool initialized = false;

    public void Start()
    {
        SetDictionary();
    }

    public void SetDictionary()
    {
        name = member.name;

        statsDictionary.Add(StatsEnum.HP, member.HP);
        statsDictionary.Add(StatsEnum.Accuracy, member.Accuracy);
        statsDictionary.Add(StatsEnum.Mind, member.Mind);
        statsDictionary.Add(StatsEnum.PhysDamage, member.PhysDamage);
        statsDictionary.Add(StatsEnum.PhyDefense, member.PhysDefense);
        statsDictionary.Add(StatsEnum.MagDamage, member.MagDamage);
        statsDictionary.Add(StatsEnum.Speed, member.MagDefense);
        statsDictionary.Add(StatsEnum.Luck, member.Luck);

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