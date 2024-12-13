using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler
{
    ItemData itemToDisplay;
    int quantity;

    public Image itemDisplayImage;
    public TextMeshProUGUI quantityText;

    int slotIndex;
    public enum InventoryType
    {
        Tool,Item
    }

    public InventoryType inventoryType;

    public void Display(ItemSlotData itemSlot)
    {
        // set the variables accordingly
        itemToDisplay = itemSlot.itemData;
        quantity = itemSlot.quantity;

        quantityText.text = "";


        if (itemToDisplay != null)
        {
            itemDisplayImage.sprite = itemToDisplay.thumbail;

            if(quantity > 1)
            {
                quantityText.text = quantity.ToString();
            }
      
            itemDisplayImage.gameObject.SetActive(true);
            return;
        }
         
        itemDisplayImage.gameObject.SetActive(false);
    }


    // set slotIndex
    public void AssignIndex(int slotIndex)
    {
        this.slotIndex = slotIndex;
    }

    // khi di chuot vao se hien thi thong tin
    public void OnPointerEnter(PointerEventData eventData)
    {
       UIManager.instance.DisplayItemInfo(itemToDisplay);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.instance.DisplayItemInfo(null);
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        // chuyen item tu inventory to hand
        InventoryManager.instance.InventoryToHand(slotIndex,inventoryType);
    }
}
