using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats
{
    public static int Money { get; private set; }

    public const string CURRENCY = "G";

    public static void Spend(int cost)
    {
        if(cost > Money)
        {
            Debug.LogError("you don't enough money");
            return;
        }
        Money -= cost;

        UIManager.instance.RenderPlayerStats();
    }

    public static void Earn(int income)
    {
        Money += income;
        UIManager.instance.RenderPlayerStats();
    }

    public static void LoadStats(int money)
    {
        Money = money;

        UIManager.instance.RenderPlayerStats();

        Debug.Log("Load player data");
    }
}
