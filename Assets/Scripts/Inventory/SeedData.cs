using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Items/Seed")]
public class SeedData : ItemData
{
    public int dayToGrow;
    public ItemData cropToYield;
    public GameObject seedling;

    public bool regrowable;
    public int dayToRegrow;
}
