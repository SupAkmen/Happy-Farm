using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopListing : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Image itemThumbnail;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI costText;

    ItemData itemData;

    public void Display(ItemData itemData)
    {
        this.itemData = itemData;
        itemThumbnail.sprite = itemData.thumbail;
        nameText.text = itemData.name;
        costText.text = itemData.cost + PlayerStats.CURRENCY;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        UIManager.instance.shopListingManager.OpenConfirmationScreen(itemData);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UIManager.instance.DisplayItemInfo(itemData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.instance.DisplayItemInfo(null);
    }
}
