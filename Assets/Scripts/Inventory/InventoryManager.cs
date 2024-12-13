using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using System.Linq;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance { get; private set; }

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

    List<ItemData> _itemIndex;

    public ItemData GetItemFromString(string name)
    {
        if (_itemIndex == null)
        {
            _itemIndex = Resources.LoadAll<ItemData>("").ToList();
        }
        return _itemIndex.Find(i => i.name == name);
    }

    [Header("Tool")]
    [SerializeField] private ItemSlotData[] toolSlots = new ItemSlotData[8];
    [SerializeField] private ItemSlotData equippedToolSlot = null;
    [Header("Item")]
    [SerializeField] private ItemSlotData[] itemSlots = new ItemSlotData[8];
    [SerializeField] private ItemSlotData equippedItemSlot = null;

    public Transform handPoint;

    // load the inventory from a save
    public void LoadInventory(ItemSlotData[] toolSlots, ItemSlotData equippedToolSlot, ItemSlotData[] itemSlots, ItemSlotData equippedItemSlot)
    {
        Debug.Log("load inventory");
        this.toolSlots = toolSlots;
        this.equippedToolSlot = equippedToolSlot;
        this.itemSlots = itemSlots;
        this.equippedItemSlot= equippedItemSlot;

        UIManager.instance.RenderInventory();
        RenderHand();

    }

    // Equipping

    // lay item tu inventory to hand
    public void InventoryToHand(int slotIndex, InventorySlot.InventoryType inventoryType)
    {
        // the slot to equip(tool default)
        ItemSlotData handToEquip = equippedToolSlot;
        ItemSlotData[] inventoryToAlter = toolSlots; // tham chieu den mang toolSlots

        if(inventoryType == InventorySlot.InventoryType.Item)
        {
            handToEquip = equippedItemSlot;
            inventoryToAlter = itemSlots;
        }
        // check if stackable 
        if(handToEquip.Statckable(inventoryToAlter[slotIndex]))
        {
            ItemSlotData slotToAlter = inventoryToAlter[slotIndex];
            // add to the hand slot
            handToEquip.AddQuantity(slotToAlter.quantity);
            // empty the inventory slot
            slotToAlter.Empty();

        }
        else
        {
            //cache the inventory ItemSlotData
            ItemSlotData slotToEquip = new ItemSlotData(inventoryToAlter[slotIndex]);
            //change the inventory slot to the hands
            inventoryToAlter[slotIndex] = new ItemSlotData(handToEquip);

            if (slotToEquip.IsEmpty())
            {
                handToEquip.Empty();
            }
            else
            {
                EquipHandSlot(slotToEquip);
            }
            
        }
        // update changes in the scene
        if (inventoryType == InventorySlot.InventoryType.Item)
        {
            RenderHand();
        }
        // cap nhat thay doi ui
        UIManager.instance.RenderInventory();
    }

    // lay item tu hand to inventory 
    public void HandToInventory(InventorySlot.InventoryType inventoryType)
    {
        // the slot to move from(tool default)
        ItemSlotData handSlot = equippedToolSlot;
        ItemSlotData[] inventoryToAlter = toolSlots; // tham chieu den mang toolSlots

        if (inventoryType == InventorySlot.InventoryType.Item)
        {
            handSlot = equippedItemSlot;
            inventoryToAlter = itemSlots;
        }

        // Try casting the hand slot
        if(!StackItemToInventory(handSlot, inventoryToAlter))
        {
            // find an empty slot tp put the item in
            for (int i = 0; i < inventoryToAlter.Length; i++)
            {
                if (inventoryToAlter[i].IsEmpty())
                {
                    inventoryToAlter[i] = new ItemSlotData(handSlot);
                    handSlot.Empty();
                    break;
                }
            }
        }

        // update changes in the scene
        if (inventoryType == InventorySlot.InventoryType.Item)
        {
            RenderHand();
        }
        // cap nhat thay doi ui
        UIManager.instance.RenderInventory();
    }

    public void ShopToInventory(ItemSlotData itemToMove)
    {
        ItemSlotData[] inventoryToAlter = IsTool(itemToMove.itemData) ? toolSlots : itemSlots;
       
        // Try casting the hand slot
        if (!StackItemToInventory(itemToMove, inventoryToAlter))
        {
            // find an empty slot tp put the item in
            for (int i = 0; i < inventoryToAlter.Length; i++)
            {
                if (inventoryToAlter[i].IsEmpty())
                {
                    inventoryToAlter[i] = new ItemSlotData(itemToMove);
                    break;
                }
            }
        }
        // cap nhat thay doi ui
        UIManager.instance.RenderInventory();
        RenderHand();
    }

    // duyet qua cac item trong inventory de xem co the xep chong ko
    public bool StackItemToInventory(ItemSlotData itemSlot, ItemSlotData[] inventoryArray)
    {
        for(int i = 0; i < inventoryArray.Length; i++)
        {
            if (inventoryArray[i].Statckable(itemSlot))
            {
                inventoryArray[i].AddQuantity(itemSlot.quantity);
                itemSlot.Empty();
                return true;
            }
        }
        return false;
    }    
    public void RenderHand()
    {
        if (handPoint.childCount > 0)
        {
            Destroy(handPoint.GetChild(0).gameObject);
        }
        if (SlotEquipped(InventorySlot.InventoryType.Item))
        {
            Instantiate(GetEquippedSlotItem(InventorySlot.InventoryType.Item).model, handPoint);
        }

    }

    #region Get and check
    // get the slot item (ItemData)
    public ItemData GetEquippedSlotItem(InventorySlot.InventoryType inventoryType)
    {
        if (inventoryType == InventorySlot.InventoryType.Item)
        {
            return equippedItemSlot.itemData;
        }
        return equippedToolSlot.itemData;
    }

    // get function for the slots (ItemSlotData)
    public ItemSlotData GetEquippedSlot(InventorySlot.InventoryType inventoryType)
    {
        if (inventoryType == InventorySlot.InventoryType.Item)
        {
            return equippedItemSlot;
        }
        return equippedToolSlot;
    }
    // get function for the inventory slot
    public ItemSlotData[] GetInventorySlot(InventorySlot.InventoryType inventoryType)
    {
        if (inventoryType == InventorySlot.InventoryType.Item)
        {
            return itemSlots;
        }
        return toolSlots;
    }

    // check if a hand slot has an item
    public bool SlotEquipped(InventorySlot.InventoryType inventoryType)
    {
        if (inventoryType == InventorySlot.InventoryType.Item)
        {
            return !equippedItemSlot.IsEmpty();
        }
        return !equippedToolSlot.IsEmpty();
    }

    // check if the item is a tool
    public bool IsTool(ItemData item)
    {
        // Is it equipment
        // try to cast it as equipment first
        EquipmentData equipment = item as EquipmentData;
        if (equipment != null)
        {
            return true;

        }

        // Is it a seed
        // Try to cast it as a seed
        SeedData seed = item as SeedData;
        // if the seed is not null it is a seed
        return seed != null;
    }
    #endregion

    // Equip the handslot with an ItemData (Will overwrite the slot)
    public void EquipHandSlot(ItemData item)
    {
        if (IsTool(item))
        {
            equippedToolSlot = new ItemSlotData(item);
        }
        else
        {
            equippedItemSlot = new ItemSlotData(item);
        }
    }

    // Equip the handslot with an ItemSlotData (Will overwrite the slot)
    public void EquipHandSlot(ItemSlotData itemSlot)
    {
        //Get the item data from the slot
        ItemData item = itemSlot.itemData;

        if (IsTool(item))
        {
            equippedToolSlot = new ItemSlotData(itemSlot);
        }
        else
        {
            equippedItemSlot = new ItemSlotData(itemSlot);
        }
    }

    public void ConsumeItem(ItemSlotData itemSlot)
    {
        if(itemSlot.IsEmpty())
        {
            return;
        }
        itemSlot.Remove();
        RenderHand();
        UIManager.instance.RenderInventory();
    }

    #region Slot Validate
    private void OnValidate()
    {
        // validate hand slot
        ValidateInventorySlot(equippedToolSlot);
        ValidateInventorySlot(equippedItemSlot);
        // Validate the slots inn the inventory
        ValidateInventorySlots(toolSlots);
        ValidateInventorySlots(itemSlots);
    }

    // khi them itemdata vaof inspector, tu dong set solg la 1
    void ValidateInventorySlot(ItemSlotData slot)
    {
        if (slot.itemData != null && slot.quantity == 0)
        {
            slot.quantity = 1;
        }
    }
    // validate array 
    void ValidateInventorySlots(ItemSlotData[] array)
    {
        foreach(ItemSlotData slot in array)
        {
            ValidateInventorySlot(slot);
        }
    }
    #endregion
}
