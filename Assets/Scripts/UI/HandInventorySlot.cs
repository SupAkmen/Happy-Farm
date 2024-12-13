using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HandInventorySlot : InventorySlot
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        // chuyen item tu hand to inventory
        InventoryManager.instance.HandToInventory(inventoryType);
    }
}
