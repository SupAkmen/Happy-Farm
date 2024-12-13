using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShippingBin : InteractableObject
{
    public static int hourToShip = 18;
    public static  List<ItemSlotData> itemToShip = new List<ItemSlotData>();

    public override void PickUp()
    {
        // lay itemdata tu item nguoi choi muon dat xuoong bin
        ItemData handSlotItem = InventoryManager.instance.GetEquippedSlotItem(InventorySlot.InventoryType.Item);

        if ((handSlotItem == null))
        {
            return;
        }

        UIManager.instance.TriggerYesNoPrompt($"Do you want to sell {handSlotItem.name} ?", PlaceItemToShippingBin);
    }

    void PlaceItemToShippingBin()
    {
        // Get the itemdata of whay the player is holding
        ItemSlotData handSlot = InventoryManager.instance.GetEquippedSlot(InventorySlot.InventoryType.Item);

        // Add the item to the itemToShip
        itemToShip.Add(new ItemSlotData(handSlot));

        // rong vi tri
        handSlot.Empty();

        InventoryManager.instance.RenderHand();

        foreach(ItemSlotData item in itemToShip)
        {
            Debug.Log($"{item.itemData.name} x {item.quantity}");
        }
    }

    // hang co the van chuyen du ko co nguoi choi trong canh nen dung static
    public static void ShipItems()
    {
        int moneyToRecieve = TallyItems(itemToShip);
        PlayerStats.Earn(moneyToRecieve);
        // Empty the shipping bin
        itemToShip.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PickUp();
        }
    }

    static int TallyItems(List<ItemSlotData> items)
    {
        int total = 0;
        foreach (ItemSlotData item in items)
        {
            total += item.quantity * item.itemData.cost;
        }
        return total;
    }
}
