using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemSlotSavedData
{
    public string itemId;
    public int quantity;

    // Convert ItemSlotData into a serializable format
    public ItemSlotSavedData(ItemSlotData data)
    {
        if(data.IsEmpty())
        {
            itemId = null;
            quantity = 0;
            return;
        }

        itemId = data.itemData.name;
        quantity = data.quantity;
    }
}
