using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CropBehaviour : MonoBehaviour
{
    int landId; // the id of land they belong to

    SeedData seedToGrow;

    [Header("Stages of life")]
    public GameObject seed;
    public GameObject wilted;
    private GameObject seedling;
    private GameObject harvestable;

    // the growth point of the crop
    int growth;
    // how many growth points it take before it becomes harvestable
    int maxGrowth;

    // cay co the song 48 neu ko dc tuoi nươc
    int maxHealth = GameTimeStamp.HoursToMinutes(48);
    int health;

    public enum CropState
    {
        Seed, Seedling, Harvestable, Wilted
    }

    public CropState cropState;
    // goi khi player plant a seed 
    public void Plant(int landId, SeedData seedToGrow)
    {
        /*
        this.seedToGrow = seedToGrow;

        // san sinh ra cay non va thu hoach
        seedling = Instantiate(seedToGrow.seedling, transform);

        ItemData cropToYield = seedToGrow.cropToYield;
        harvestable = Instantiate(cropToYield.model, transform);

        // chuyen thoi gian sang phut
        int hourToGrow = GameTimeStamp.DaysToHours(seedToGrow.dayToGrow);
        maxGrowth = GameTimeStamp.HoursToMinutes(hourToGrow);

        // ktra cay trong lai
        if (seedToGrow.regrowable)
        {
            RegrowHarvestableBehaviour regrowHarvestable = harvestable.GetComponent<RegrowHarvestableBehaviour>();

            // khoi tao the harvestable
            regrowHarvestable.SetParent(this);
        }

        SwitchState(CropState.Seed);
        */

        LoadCropData(landId, seedToGrow,CropState.Seed,0,0);

        LandManager.instance.RegisterCrop(landId, seedToGrow, cropState, growth, health);
    }

    public void LoadCropData(int landId, SeedData seedToGrow, CropBehaviour.CropState cropState, int growth, int health)
    {
        this.landId = landId;

        this.seedToGrow = seedToGrow;

        // san sinh ra cay non va thu hoach
        seedling = Instantiate(seedToGrow.seedling, transform);

        ItemData cropToYield = seedToGrow.cropToYield;
        harvestable = Instantiate(cropToYield.model, transform);

        // chuyen thoi gian sang phut
        int hourToGrow = GameTimeStamp.DaysToHours(seedToGrow.dayToGrow);

        maxGrowth = GameTimeStamp.HoursToMinutes(hourToGrow);

        // set the growth and health accordingly
        this.health = health;
        this.growth = growth;

        // ktra cay trong lai
        if (seedToGrow.regrowable)
        {
            RegrowHarvestableBehaviour regrowHarvestable = harvestable.GetComponent<RegrowHarvestableBehaviour>();

            // khoi tao the harvestable
            regrowHarvestable.SetParent(this);
        }

        SwitchState(cropState);

    }
    public void Grow()
    {
        growth++;

        // hoi health khi duoc tuoi nuoc
        if(health < maxHealth)
        {
            health++;
        }    

        if(growth >= maxGrowth/2 && cropState == CropState.Seed)
        {
            SwitchState(CropState.Seedling);
        }

        if(growth >= maxGrowth && cropState == CropState.Seedling)
        {
            SwitchState(CropState.Harvestable);
        }

        //update landmanager on the changes
        LandManager.instance.OnCropStateChange(landId, cropState, growth, health);
    }

    public void Wither()
    {
        health--;

        if(health <=0 && cropState != CropState.Seed)
        {
            SwitchState(CropState.Wilted);
        }

        //update landmanager on the changes
        LandManager.instance.OnCropStateChange(landId, cropState, growth, health);
    }
    void SwitchState(CropState stateToSwitch)
    {
        seed.SetActive(false);
        seedling.SetActive(false);
        harvestable.SetActive(false);
        wilted.SetActive(false);


        switch(stateToSwitch)
        {
            case CropState.Seed:
                seed.SetActive(true);
                break;
            case CropState.Seedling:
                seedling.SetActive(true);

                health = maxHealth;
                break;
            case CropState.Harvestable:
                harvestable.SetActive(true);

                if(!seedToGrow.regrowable)
                {
                    // sau khi thu hoach dc thi huy bo cropbehavior
                    harvestable.transform.parent = null;
                    // pass the Remove function over to the InteractableObject so that it will be called when the palyer pick it up
                    harvestable.GetComponent<InteractableObject>().onInteract.AddListener(RemoveCrop);
                }
             
                break;
            case CropState.Wilted:
                wilted.SetActive(true);
                break;
        }

        cropState = stateToSwitch;
    }

    //destroys and deregister the crop
    public void RemoveCrop()
    {
        LandManager.instance.DeregisterCrop(landId);
        Destroy(gameObject);
    }

    public void Regrow()
    {
        // reset the growth
        // get the regrowth time in hours
        int hourToRegrow = GameTimeStamp.DaysToHours(seedToGrow.dayToGrow);
        growth = maxGrowth - GameTimeStamp.HoursToMinutes(hourToRegrow);

        // switch the state back to seedling
        SwitchState (CropState.Seedling);
    }

}
