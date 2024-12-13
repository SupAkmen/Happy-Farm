using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour,ITimeTracker
{
    public static UIManager instance { get; private set; }

    [Header("Screen Management")]
    public GameObject menuScreen;
    public enum Tab
    {
        Inventory, Relationships,Animals
    }
    public Tab selectedTab;

    [Header("Status Bar")]
    public Image toolEquipSlot;
    public TextMeshProUGUI toolQuantitySlot;

    public TextMeshProUGUI timeText;
    public TextMeshProUGUI dateText;

    [Header("Inventory System")]
    public GameObject inventoryPanel;

    // slot ui
    public InventorySlot[] toolSlots;
    public HandInventorySlot toolHandSlot;

    public InventorySlot[] itemSlots;
    public HandInventorySlot itemHandSlot;

    [Header("Item Infor")]
    public GameObject itemInfoBox;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemDesText;

    [Header("Screen Transition")]
    public GameObject fadeIn;
    public GameObject fadeOut;

    [Header("Prompt")]
    public YesNoPrompt yesNoPrompt;
    public NamingPrompt namingPrompt;

    [Header("Player Stats")]
    public TextMeshProUGUI moneyText;

    [Header("Shop")]
    public ShopListingManager shopListingManager;

    [Header("Relationships")]
    public RelationshipListingManager relationshipListingManager;
    public AnimalListingManager animalRelationshipListingManager;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        RenderInventory();
        AssignSlotIndexes();
        RenderPlayerStats();
        DisplayItemInfo(null);

        // add uimanager to the list of object Timemanager will notify when the time update
        TimeManager.instance.RegisterTracker(this);
    }

    #region Prompt
    public void TriggerYesNoPrompt(string message,Action onYesCallBack)
    {
        yesNoPrompt.gameObject.SetActive(true);

        yesNoPrompt.CreatePrompt(message, onYesCallBack);

        Debug.Log("Touch");
    }

    public void TriggerNamingPrompt(string message,Action<string> onConfirmCallback)
    {
        // ktra neu cac prompt dang trong tien trinh
        if(namingPrompt.gameObject.activeSelf)
        {
            namingPrompt.QueuePromptAction(() => TriggerNamingPrompt(message,onConfirmCallback));
            return;
        }
        namingPrompt.gameObject.SetActive(true);

        namingPrompt.CreatePrompt(message, onConfirmCallback);
    }

    #endregion

    #region Tab Management

    public void ToggleMenuPanel()
    {
        menuScreen.SetActive(!menuScreen.activeSelf);
        OpenWindow(selectedTab);
        TabBehaviour.onTabStateChange?.Invoke();
    }

    public void OpenWindow(Tab windowToOpen)
    {
        relationshipListingManager.gameObject.SetActive(false);
        inventoryPanel.SetActive(false);
        animalRelationshipListingManager.gameObject.SetActive(false);

        switch(windowToOpen)
        {
            case Tab.Inventory:
                inventoryPanel.SetActive(true);
                RenderInventory();
                break;
            case Tab.Relationships:
                relationshipListingManager.gameObject.SetActive(true);
                relationshipListingManager.Render(RelationshipStats.relationships);
                break;
            case Tab.Animals:
                animalRelationshipListingManager.gameObject.SetActive(true);
                animalRelationshipListingManager.Render(AnimalStats.animalRelationships);
                break;
        }

        selectedTab = windowToOpen;
    }


    #endregion

    #region FadeIn FadeOut Transition
    public void FadeOutScreen()
    {
        fadeOut.SetActive(true);
    }  
    public void FadeInScreen()
    {
        fadeIn.SetActive(true);
    }
    public void OnFadeInComplete()
    {
        fadeIn.SetActive(false);
    }

    public void ResetFadeDefault()
    {
        fadeOut.SetActive(false);
        fadeIn.SetActive(true);
    }
    #endregion

    #region Inventory
    public void AssignSlotIndexes()
    {
        for(int i = 0; i < toolSlots.Length; i++)
        {
            toolSlots[i].AssignIndex(i);
            itemSlots[i].AssignIndex(i);
        }
    }

    public void RenderInventory()
    {
        // get 
        ItemSlotData[] inventoryToolSlots = InventoryManager.instance.GetInventorySlot(InventorySlot.InventoryType.Tool);
        ItemSlotData[] inventoryItemSlots = InventoryManager.instance.GetInventorySlot(InventorySlot.InventoryType.Item);
        // Hien thi 
        RenderInventoryPanel(inventoryToolSlots,toolSlots);
        RenderInventoryPanel(inventoryItemSlots,itemSlots);

        // render the equiped slot
        toolHandSlot.Display(InventoryManager.instance.GetEquippedSlot(InventorySlot.InventoryType.Tool));
        itemHandSlot.Display(InventoryManager.instance.GetEquippedSlot(InventorySlot.InventoryType.Item));



        // lay tool equip tu inventorymanager
        ItemData equippedTool = InventoryManager.instance.GetEquippedSlotItem(InventorySlot.InventoryType.Tool);

        toolQuantitySlot.text = " ";

        if (equippedTool != null)
        {
            toolEquipSlot.sprite = equippedTool.thumbail;

            toolEquipSlot.gameObject.SetActive(true);

            //Get quantity

            int quantity = InventoryManager.instance.GetEquippedSlot(InventorySlot.InventoryType.Tool).quantity;
            if (quantity > 1)
            {
                toolQuantitySlot.text = quantity.ToString();
            }
            return;
        }

        toolEquipSlot.gameObject.SetActive(false);
    }   
    
    // hien thi len ui
    void RenderInventoryPanel(ItemSlotData[] slots, InventorySlot[] uiSlots)
    {
        for (int i = 0; i < uiSlots.Length; i++)
        {
            uiSlots[i].Display(slots[i]);
        }
    }

    public void ToggleInventoryPanel()
    {
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        RenderInventory();
    }

    // hien thi thong tin 
    public void DisplayItemInfo(ItemData data)
    {
        if(data == null)
        {
            itemNameText.text = "";
            itemDesText.text = "";
            itemInfoBox.SetActive(false);
            return;
        }

        itemInfoBox.SetActive(true);
        itemNameText.text = data.name;
        itemDesText.text = data.description;
    }
    #endregion

    #region Time
    // callback to handle the UI for time
    public void ClockUpdate(GameTimeStamp timestamp)
    {
        // Time
        int hours = timestamp.hour;
        int minutes = timestamp.minute;

        string prefix = "AM";

        if(hours > 12)
        {
            prefix = "PM";
            hours -= 12;
        }
        timeText.text = prefix + " " + hours + ":" + minutes.ToString("00");

        // Date
        int day =timestamp.day;
        string season = timestamp.season.ToString();
        string dayOfWeek = timestamp.GetDayOfWeek().ToString();

        dateText.text = season + " " +day + " (" + dayOfWeek + ")";
    }
    #endregion

    public void RenderPlayerStats()
    {
        moneyText.text = PlayerStats.Money + PlayerStats.CURRENCY;
    }

    public void OpenShop(List<ItemData> shopItems)
    {
        shopListingManager.gameObject.SetActive(true);
        shopListingManager.Render(shopItems);
    }

    public void ToggleRelationshipPanel()
    {
        GameObject panel = relationshipListingManager.gameObject;
        panel.SetActive(!panel.activeSelf);

        if(panel.activeSelf)
        {
            relationshipListingManager.Render(RelationshipStats.relationships);
        }
    }
}
