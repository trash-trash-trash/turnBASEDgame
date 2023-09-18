using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public sbyte maxHP;
    public sbyte currentHP;

    public bool isAlive;

    public event Action<bool> IsAliveEvent;

    public event Action<sbyte> DeclareHPEvent;

    public sbyte testAmount;

    public void TestChangeHealth()
    {
        ChangeHealth(testAmount);
    }

    public void ChangeHealth(sbyte amount)
    {
        int newHP = currentHP + amount;
        currentHP = (sbyte)Mathf.Clamp(newHP, 0, 100);

        if (currentHP <= 0)
            Die();

        if (currentHP > 0)
        {
            isAlive = true;
        }

        else if (amount >= maxHP)
        {
            currentHP = maxHP;
        }
        
        IsAliveEvent?.Invoke(isAlive);
        DeclareHPEvent?.Invoke(currentHP);
    }

    public void Resurrect()
    {
        isAlive = true;
        ChangeHealth(maxHP);
        IsAliveEvent?.Invoke(true);
    }

    public void Die()
    {
        currentHP = 0;
        isAlive = false;
        IsAliveEvent?.Invoke(false);
    }
}
