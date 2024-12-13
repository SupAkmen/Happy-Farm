using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventorySaveState
{
    public ItemSlotSavedData[] toolSlots;
    public ItemSlotSavedData[] itemSlots;

    public ItemSlotSavedData equippedItemSlot;
    public ItemSlotSavedData equippedToolSlot;

    public InventorySaveState(ItemSlotData[] toolSlots,
        ItemSlotData[] itemSlots, 
        ItemSlotData equippedItemSlot, 
        ItemSlotData equippedToolSlot)
    {
        this.toolSlots = ItemSlotData.SerializeArray(toolSlots);
        this.itemSlots = ItemSlotData.SerializeArray(itemSlots);
        this.equippedItemSlot = ItemSlotData.SerializeData(equippedItemSlot);
        this.equippedToolSlot = ItemSlotData.SerializeData(equippedToolSlot);
    }

    public static InventorySaveState Export()
    {
        ItemSlotData[] toolSlots = InventoryManager.instance.GetInventorySlot(InventorySlot.InventoryType.Tool);
        ItemSlotData[] itemSlots = InventoryManager.instance.GetInventorySlot(InventorySlot.InventoryType.Item);

        ItemSlotData equippedToolSlot = InventoryManager.instance.GetEquippedSlot(InventorySlot.InventoryType.Tool);
        ItemSlotData equippedItemSlot = InventoryManager.instance.GetEquippedSlot(InventorySlot.InventoryType.Item);

        return new InventorySaveState(toolSlots, itemSlots,equippedItemSlot ,equippedToolSlot);
    }


    public void LoadData()
    {
        ItemSlotData[] toolSlots = ItemSlotData.DeserializeArray(this.toolSlots);
        ItemSlotData[] itemSlots = ItemSlotData.DeserializeArray(this.itemSlots);
        ItemSlotData equippedToolSlot = ItemSlotData.DeserializeData(this.equippedToolSlot);
        ItemSlotData equippedItemSlot = ItemSlotData.DeserializeData(this.equippedItemSlot);

        InventoryManager.instance.LoadInventory(toolSlots,equippedToolSlot,itemSlots,equippedItemSlot);
    }

}
