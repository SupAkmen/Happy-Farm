using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

[System.Serializable]
public class ItemSlotData 
{
    public ItemData itemData;
    public int quantity;

    public ItemSlotData(ItemData itemData, int quantity)
    {
        this.itemData = itemData;
        this.quantity = quantity;
        ValidateQuantity();
    }

    public ItemSlotData(ItemData itemData)
    {
        this.itemData = itemData;
        quantity = 1;
        ValidateQuantity();
    }

    //clones the ItemSlotData
    public ItemSlotData(ItemSlotData slotToClone)
    {
        itemData = slotToClone.itemData;
        quantity = slotToClone.quantity;
    }

    public void AddQuantity()
    {
        AddQuantity(1);
    }

    public void AddQuantity(int amountToAdd)
    {
        quantity += amountToAdd;
    }

    public void Remove()
    {
        quantity -= 1;
        ValidateQuantity();
    }

    private void ValidateQuantity()
    {
        if (quantity <= 0 || itemData == null)
        {
            Empty();
        }
    }

    public void Empty()
    {
        itemData = null;
        quantity = 0;
    }

    // check if the stack is considered 'empty'

    public bool IsEmpty()
    {
        return itemData == null;
    }

    // ktra xem co the xep chong hay khong
    public bool Statckable(ItemSlotData slotToCompare)
    {
        return slotToCompare.itemData == itemData;
    }

    // convert itemslotdata into itemslotsaveddata
    public static ItemSlotSavedData SerializeData(ItemSlotData itemSlot)
    {
        return new ItemSlotSavedData(itemSlot);
    }
    // convert itemslosavedtdata into itemslotdata
    public static ItemSlotData DeserializeData(ItemSlotSavedData itemSavedSlot)
    {
        //convert string back into ItemData
        ItemData item = InventoryManager.instance.GetItemFromString(itemSavedSlot.itemId);
        return new ItemSlotData(item,itemSavedSlot.quantity);
    }

    // convert an entire ItemSlotData array into an itemslotsaveddata
    public static ItemSlotSavedData[] SerializeArray(ItemSlotData[] array)
    {
        return Array.ConvertAll(array, new Converter<ItemSlotData, ItemSlotSavedData>(SerializeData));
    }
    // convert an entire ItemSlotData array into an itemslotsaveddata
    public static ItemSlotData[] DeserializeArray(ItemSlotSavedData[] array)
    {
        return Array.ConvertAll(array, new Converter<ItemSlotSavedData,ItemSlotData>(DeserializeData));
    }
}
