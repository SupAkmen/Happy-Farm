using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : InteractableObject
{
    public List<ItemData> shopItem;

    [Header("Dialogues")]
    public List<DialogueLine> dialogueOnShopOpen;

    public static void Purchase(ItemData item,int quantity)
    {
        int totalCost = item.cost * quantity;

        if(PlayerStats.Money >= totalCost)
        {
            PlayerStats.Spend(totalCost);
            // creat an itemslotdata for the purchased item
            ItemSlotData purchasedItem = new ItemSlotData(item, quantity);
            // send it to the inventory
            InventoryManager.instance.ShopToInventory(purchasedItem);
        }
    }

    public override void PickUp()
    {
        DialogueManager.instance.StartDialogues(dialogueOnShopOpen, OpenShop);
    }

    void OpenShop()
    {
        UIManager.instance.OpenShop(shopItem);
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        PickUp();
    //    }
    //}
}
