using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCStats : StatsBase
{
    public int HP;

    public void OnEnable()
    {
   
    }

    public void AnnounceDeath(StatsEnum statsEnum, int i)
    {
        if (statsEnum != StatsEnum.HP)
            return;
        if (i <= 0)
        {

        }
    }
}
