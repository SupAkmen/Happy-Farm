using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CropSaveState 
{
    // cay duoc trong tren land nao
    public int landId;

    public string seedToGrow;
    public CropBehaviour.CropState cropState;
    public int growth;
    public int health ;

    public CropSaveState(int landId, string seedToGrow, CropBehaviour.CropState cropState, int growth, int health)
    {
        this.landId = landId;
        this.seedToGrow = seedToGrow;
        this.cropState = cropState;
        this.growth = growth;
        this.health = health;
    }

    public void Grow()
    {
        growth++;

        SeedData seedInfo = (SeedData) InventoryManager.instance.GetItemFromString(seedToGrow);

        int maxHealth = GameTimeStamp.HoursToMinutes(48);
        int maxGrowth = GameTimeStamp.HoursToMinutes(GameTimeStamp.DaysToHours(seedInfo.dayToGrow));


        // hoi health khi duoc tuoi nuoc
        if (health < maxHealth)
        {
            health++;
        }

        if (growth >= maxGrowth / 2 && cropState == CropBehaviour.CropState.Seed)
        {
            cropState = CropBehaviour.CropState.Seedling;
        }

        if (growth >= maxGrowth && cropState == CropBehaviour.CropState.Seedling)
        {
            cropState = CropBehaviour.CropState.Harvestable;
        }

    }


    public void Wither()
    {
        health--;

        if (health <= 0 && cropState != CropBehaviour.CropState.Seed)
        {
            cropState = CropBehaviour.CropState.Wilted;
        }
    }

}
