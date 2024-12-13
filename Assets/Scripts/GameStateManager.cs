using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour,ITimeTracker
{
    public static GameStateManager instance { get; private set; }

    bool screenFadeOut;
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
        TimeManager.instance.RegisterTracker(this);
    }
    public void ClockUpdate(GameTimeStamp timestamp)
    {
        UpdateFarmState(timestamp);
        UpdateShippingState(timestamp);
        IncubationManager.UpdateEggs();

        if(timestamp.hour ==  0 && timestamp.minute == 0)
        {
            OnDayReset();
        }
    }

    // call when the day has been reset
    void OnDayReset()
    {
        Debug.Log("Reset");

        foreach(NPCRelationshipState npc in RelationshipStats.relationships)
        {
            npc.hasTalkedToday = false;
            npc.giftGivenToday = false;
        }
        AnimalFeedManager.ResetFeedBoxes();
        AnimalStats.OnDayReset();
      
    }

    public void UpdateShippingState(GameTimeStamp timestamp)
    {
        if(timestamp.hour == ShippingBin.hourToShip && timestamp.minute == 0)
        {
            ShippingBin.ShipItems();
        }
    }
    public void UpdateFarmState(GameTimeStamp timestamp)
    {
        //cap nhat crop and land save kjhi nguoi choi o ngoia trang trai
        if (SceneTransitionManager.Instance.currentLocation != SceneTransitionManager.Location.Farm)
        {
            if (LandManager.farmData == null) return;
            List<LandSaveState> landData = LandManager.farmData.Item1;
            List<CropSaveState> cropData = LandManager.farmData.Item2;

            // neu nhu dat ko co cay nao duoc trong thi ko can cap nhat
            if (cropData.Count == 0) return;

            for (int i = 0; i < cropData.Count; i++)
            {
                // get the crop and corresponding land data
                CropSaveState crop = cropData[i];
                LandSaveState land = landData[crop.landId];

                // neu cay trang thai heo thi bo qua thoi gian
                if (crop.cropState == CropBehaviour.CropState.Wilted)
                {
                    continue;
                }

                // update the land's state
                land.ClockUpdate(timestamp);

                // update the crop's state based on the land state
                if (land.landStatus == Land.LandStatus.Watered)
                {
                    crop.Grow();
                }
                else if (crop.cropState != CropBehaviour.CropState.Seed)
                {
                    crop.Wither();
                }

                cropData[i] = crop;
                landData[crop.landId] = land;
            }
        }
    }

    public void Sleep()
    {
        UIManager.instance.FadeOutScreen();
        screenFadeOut = false;
        StartCoroutine(TransitionTime());
    }

    IEnumerator TransitionTime()
    {
        GameTimeStamp timestampOfNextDay = TimeManager.instance.GetGameTimestamp();

        timestampOfNextDay.day += 1;
        timestampOfNextDay.hour = 6;
        timestampOfNextDay.minute = 0;

        while(!screenFadeOut)
        {
            yield return new WaitForSeconds(1f);
        }

        TimeManager.instance.SkipTime(timestampOfNextDay);

        SaveManager.Save(ExportSaveState());

        screenFadeOut = false;
        UIManager.instance.ResetFadeDefault();

    }

    public void OnFadeOutComplete()
    {
        screenFadeOut = true;
    }

    public GameSaveState ExportSaveState()
    {
        // retrieve farm data
        FarmSaveState farmSaveState = FarmSaveState.Export();

        // retrieve inventoryslot
        InventorySaveState inventorySaveState = InventorySaveState.Export();

        PlayerSaveState playerSaveState = PlayerSaveState.Export();

        // Time
        GameTimeStamp timestamp = TimeManager.instance.GetGameTimestamp();

        RelationshipSaveState relationshipSaveState = RelationshipSaveState.Export();

        return new GameSaveState(farmSaveState,inventorySaveState,timestamp,playerSaveState,relationshipSaveState);
    }

    public void LoadSave()
    {
        if (!SaveManager.HasSave())
        {
            Debug.LogWarning("Không tìm thấy file lưu, quá trình tải bị hủy.");
            return;
        }
        // Retrieve the loaded save
        GameSaveState save = SaveManager.Load();

        if (save == null)
        {
            Debug.LogError("Dữ liệu lưu không hợp lệ.");
            return;
        }

        // Load up the parts
        // Time
        TimeManager.instance.LoadTime(save.timestamp);

        // Inventory
        save.inventorySaveState.LoadData();

        // Farming data
        save.farmSaveState.LoadData();

        // PlayerStats
        save.playerSaveState.LoadData();

        // Relationships
        save.relationshipSaveState.LoadData();

    }
}