using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedBox : InteractableObject
{
    bool containsFeed;
    public GameObject displayFeed;
    public int id;

    public override void PickUp()
    {
       if(CanFeed())
        {
            FeedAnimal();
        }
    }

    void FeedAnimal()
    {
        InventoryManager.instance.ConsumeItem(InventoryManager.instance.GetEquippedSlot(InventorySlot.InventoryType.Item)); 
        SetFeedState(true);

        FindObjectOfType<AnimalFeedManager>().FeedAnimal(id);
    }

    public void SetFeedState(bool feed)
    {
        containsFeed = feed;
        displayFeed.SetActive(feed);
    }

    bool CanFeed()
    {
        ItemData handSlotItem = InventoryManager.instance.GetEquippedSlotItem(InventorySlot.InventoryType.Item);

        if(handSlotItem == null || containsFeed) return false;

        if(handSlotItem.name != item.name) return false;

        return true;
    }
}
