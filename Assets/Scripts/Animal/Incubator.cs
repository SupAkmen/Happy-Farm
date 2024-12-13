using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Incubator : InteractableObject
{
    // co chua egg trong long ap ko
    bool containsEgg;
    // thoi gian xh trung
    int timeToIncubate;

    public GameObject displayEgg;

    //the id of the incubator
    public int incubationID;

    public override void PickUp()
    {
        if(CanIncubate())
        {
            StartIncubation();
            Debug.Log("incuba");
        }
    }

    bool CanIncubate()
    {
        // lay tt item tu tay nguoi choi
        ItemData handSlotItem = InventoryManager.instance.GetEquippedSlotItem(InventorySlot.InventoryType.Item);

        // ktra player dang cam thu gi ko hoac long ap da co trung
        if (handSlotItem == null || containsEgg) return false;

        // dam bao item la trung
        if(handSlotItem.name != item.name) return false;    

        return true;

    }

    void StartIncubation()
    {
        InventoryManager.instance.ConsumeItem(InventoryManager.instance.GetEquippedSlot(InventorySlot.InventoryType.Item));
        int hours = GameTimeStamp.DaysToHours(IncubationManager.daysToIncubate);
        SetIncubationState(true,GameTimeStamp.HoursToMinutes(hours));

        // dk vao list
        IncubationManager.eggIncubating.Add(new EggIncubationSaveState(incubationID, timeToIncubate));
    }

    public void SetIncubationState(bool containsEgg,int timeToIncubate)
    {
        this.containsEgg = containsEgg;
        this.timeToIncubate = timeToIncubate;

        displayEgg.SetActive(containsEgg);
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        PickUp();
    //    }
    //}
}
