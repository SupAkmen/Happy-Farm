using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObstacleGenerator))]
public class LandManager : MonoBehaviour
{
    public static LandManager instance { get; private set; }

    public static Tuple<List<LandSaveState>, List<CropSaveState>> farmData = null;

    List<Land> landPlots = new List<Land>();

    List<LandSaveState> landData = new List<LandSaveState>();
    List<CropSaveState> cropData = new List<CropSaveState>();   

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
    private void OnEnable()
    {
        RegisterLandPlots();
        StartCoroutine(LoadFarmData());
       
    }

    // ham duoc goi sau khi start cua land duoc goi de tranh gay xung dot
    IEnumerator LoadFarmData()
    {
        yield return new WaitForEndOfFrame();

        //load farm data if any
        if (farmData != null)
        {
            // load in any saved data
            ImportLandData(farmData.Item1);
            ImportCropData(farmData.Item2);
        }
        else
        {
            // new game
            // generate obstacle
            GetComponent<ObstacleGenerator>().GenerateObstacles(landPlots);
        }
    }

    private void OnDestroy()
    {
        // chi luu lai thong tin khi canh bi huy
        farmData = new Tuple<List<LandSaveState>,List<CropSaveState>>(landData,cropData);
    }
    #region Register and Deregister
    void RegisterLandPlots()
    {
        foreach (Transform landTransform in transform)
        {
            Land land = landTransform.GetComponent<Land>();
            landPlots.Add(land);

            // creat a corresponding LandSaveState
            landData.Add(new LandSaveState());

            land.id = landPlots.Count - 1;
        }
    }

    // register the crop onto the instance
    public void RegisterCrop(int landId, SeedData seedToGrow, CropBehaviour.CropState cropState, int growth, int health)
    {
        cropData.Add(new CropSaveState(landId,seedToGrow.name, cropState, growth, health));
    }
    // cay co the thu hoach nen co the bien mat
    public void DeregisterCrop(int landId)
    {
        cropData.RemoveAll(x => x.landId == landId);
    }
    #endregion

    #region State Change
    // update the corresponding Land data on ever change to the Land's State
    public void OnLandStateChange(int id,Land.LandStatus landStatus,GameTimeStamp lastWatered,Land.FarmObstacleStatus obstacleStatus)
    {
        landData[id] = new LandSaveState(landStatus,lastWatered,obstacleStatus);
    }

    // update the corresponding Crop data on ever change to the Land's State
    public void OnCropStateChange(int landId,CropBehaviour.CropState cropState,int growth,int health )
    {
        if(landId < 0 || landId < landPlots.Count)
        {
            return;
        }
       int cropIndex =  cropData.FindIndex(x => x.landId == landId);

        string seedToGrow = cropData[cropIndex].seedToGrow;

        cropData[cropIndex] = new CropSaveState(landId,seedToGrow,cropState, growth, health);
    }
    #endregion

    #region Load Data
    // load over the static farmdata onto the instance's landData
    public void ImportLandData(List<LandSaveState> landDatasetToLoad)
    {
        for(int i = 0; i < landDatasetToLoad.Count; i++)
        {
            // get the individual land save state
            LandSaveState landDataToLoad = landDatasetToLoad[i];
            // load it up onto the land instance
            landPlots[i].LoadLandData(landDataToLoad.landStatus, landDataToLoad.lastWatered,landDataToLoad.obstacleStatus);
        }

        landData = landDatasetToLoad;
    }
    

    // load over the static farmdata onto the instance's landData
    public void ImportCropData(List<CropSaveState> cropDatasetToLoad)
    {
        cropData = cropDatasetToLoad;
        foreach (CropSaveState cropSave in cropDatasetToLoad)
        {
            //access the land
            Land landToPlant = landPlots[cropSave.landId];
            //spawn the crop
            CropBehaviour cropToPlant = landToPlant.SpawnCrop();
            // load in the data
            SeedData seedToGrow = (SeedData)InventoryManager.instance.GetItemFromString(cropSave.seedToGrow);
            cropToPlant.LoadCropData(cropSave.landId,seedToGrow,cropSave.cropState,cropSave.growth,cropSave.health);
        }
       
    }
    #endregion
}
