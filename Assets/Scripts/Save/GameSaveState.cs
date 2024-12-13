using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameSaveState 
{
    /* Json
    // Farm data
    public List<LandSaveState> landData;
    public List<CropSaveState> cropData;

    // Inventory
    public ItemSlotData[] toolSlots;
    public ItemSlotData[] itemSlots;

    public ItemSlotData equippedItemSlot;
    public ItemSlotData equippedToolSlot;

    // Time
    public GameTimeStamp timestamp;

    public GameSaveState(List<LandSaveState> landData, List<CropSaveState> cropData, ItemSlotData[] toolSlots, ItemSlotData[] itemSlots, ItemSlotData equippedItemSlot, ItemSlotData equippedToolSlot, GameTimeStamp timestamp)
    {
        this.landData = landData;
        this.cropData = cropData;
        this.toolSlots = toolSlots;
        this.itemSlots = itemSlots;
        this.equippedItemSlot = equippedItemSlot;
        this.equippedToolSlot = equippedToolSlot;
        this.timestamp = timestamp;
    }
    */

    // Farm data
    public FarmSaveState farmSaveState;

    // Inventory
    public InventorySaveState inventorySaveState;

    // Time
    public GameTimeStamp timestamp;

    // PlayerStats
    public PlayerSaveState playerSaveState;

    //relationship
    public RelationshipSaveState relationshipSaveState;

    public GameSaveState(FarmSaveState farmSaveState, 
        InventorySaveState inventorySaveState,
        GameTimeStamp timestamp, 
        PlayerSaveState playerSaveState, 
        RelationshipSaveState relationshipSaveState)
    {
        this.farmSaveState = farmSaveState;
        this.inventorySaveState = inventorySaveState;
        this.timestamp = timestamp;
        this.playerSaveState = playerSaveState;
        this.relationshipSaveState = relationshipSaveState;
    }
}
