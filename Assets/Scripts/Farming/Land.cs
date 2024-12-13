using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Land : MonoBehaviour,ITimeTracker
{
    public int id;
   public enum LandStatus
    {
        Soil,Farmland,Watered
    }

    public LandStatus landStatus;

    public Material soilMat,farmlandMat,wateredMat;

    new Renderer renderer;

    public GameObject select;

    // cache the time land was waterd
    GameTimeStamp timeWatered;

    [Header("Crop")]
    public GameObject cropPrefab;
    CropBehaviour cropPlanted = null; // current plant in land

    //obstacles
    public enum FarmObstacleStatus { None,Rock,Wood,Weeds}
    [Header("Obstacles")]
    public FarmObstacleStatus obstacleStatus;
    public GameObject rockPrefab, weedsPrefab, woodPrefab;

    // store the instantiate obstacle as a variable we can access it
    GameObject obstacleObject;

    void Start()
    {
        renderer = GetComponent<Renderer>();  
        
        SwitchLandStatus(LandStatus.Soil);

        Select(false);

        //SetObstacleStatus(FarmObstacleStatus.Rock);
      
        TimeManager.instance.RegisterTracker(this);
    }

    public void LoadLandData(LandStatus landStatusToSwitch,GameTimeStamp lastWatered,FarmObstacleStatus obstacleStatusToSwitch)
    {
        landStatus = landStatusToSwitch;
        timeWatered = lastWatered;

        Material materialToSwitch = soilMat;

        switch (landStatusToSwitch)
        {
            case LandStatus.Soil:
                materialToSwitch = soilMat;
                break;
            case LandStatus.Farmland:
                materialToSwitch = farmlandMat;
                break;
            case LandStatus.Watered:
                materialToSwitch = wateredMat;
                break;
        }
        renderer.material = materialToSwitch;

        // Obstacle
        switch (obstacleStatusToSwitch)
        {
            case FarmObstacleStatus.None:
                //destroy obstacle object if any
                if (obstacleObject != null) Destroy(obstacleObject);
                break;
            case FarmObstacleStatus.Rock:
                obstacleObject = Instantiate(rockPrefab, transform);
                break;
            case FarmObstacleStatus.Wood:
                obstacleObject = Instantiate(woodPrefab, transform);
                break;
            case FarmObstacleStatus.Weeds:
                obstacleObject = Instantiate(weedsPrefab, transform);
                break;
        }
        if (obstacleObject != null)
        {
            obstacleObject.transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
        }
        obstacleStatus = obstacleStatusToSwitch;

    }
    public void SwitchLandStatus(LandStatus statusToSwitch)
    {
        landStatus = statusToSwitch;
        Material materialToSwitch = soilMat;

        switch(statusToSwitch)
        {
            case LandStatus.Soil:
                materialToSwitch = soilMat;
                break;
            case LandStatus.Farmland:
                materialToSwitch = farmlandMat;
                break;
            case LandStatus.Watered:
                materialToSwitch = wateredMat;

                timeWatered = TimeManager.instance.GetGameTimestamp();
                break;
        }

        renderer.material = materialToSwitch;

        // cap nhat thong tin moi lan thay doi cua dat
        LandManager.instance.OnLandStateChange(id, landStatus, timeWatered,obstacleStatus);
    }

    public void SetObstacleStatus(FarmObstacleStatus statusToSwitch)
    {
        switch(statusToSwitch)
        {
            case FarmObstacleStatus.None:
                //destroy obstacle object if any
                if(obstacleObject != null) Destroy(obstacleObject);
                break;
            case FarmObstacleStatus.Rock:
                obstacleObject = Instantiate(rockPrefab, transform);
                break;
            case FarmObstacleStatus.Wood:
                obstacleObject = Instantiate(woodPrefab, transform);
                break;
            case FarmObstacleStatus.Weeds:
                obstacleObject = Instantiate(weedsPrefab, transform);
                break;
        }
        if (obstacleObject != null)
        {
            obstacleObject.transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
        }
        obstacleStatus = statusToSwitch;

        // cap nhat thong tin moi lan thay doi cua dat
        LandManager.instance.OnLandStateChange(id, landStatus, timeWatered, obstacleStatus);
    }

    public void Select(bool toggle)
    {
        select.SetActive(toggle);
    }    

    public void Interact()
    {
        ItemData toolSlot = InventoryManager.instance.GetEquippedSlotItem(InventorySlot.InventoryType.Tool);

        if(!InventoryManager.instance.SlotEquipped(InventorySlot.InventoryType.Tool))
        {
            return;
        }

        EquipmentData equipmentTool = toolSlot as EquipmentData;

        if (equipmentTool != null)
        {
            EquipmentData.ToolType toolType = equipmentTool.toolType;

            switch(toolType)
            {
                case EquipmentData.ToolType.Hoe:
                    SwitchLandStatus(LandStatus.Farmland);
                    break;
                case EquipmentData.ToolType.WateringCan:
                    if(landStatus != LandStatus.Soil)
                    {
                        SwitchLandStatus(LandStatus.Watered);
                    }
                    break;
                case EquipmentData.ToolType.Shovel:
                    if(cropPlanted != null)
                    {
                        cropPlanted.RemoveCrop();
                    }
                    if (obstacleStatus == FarmObstacleStatus.Weeds) SetObstacleStatus(FarmObstacleStatus.None);
                    break;
                case EquipmentData.ToolType.Axe:
                    if (obstacleStatus == FarmObstacleStatus.Wood) SetObstacleStatus(FarmObstacleStatus.None);
                    break;
                case EquipmentData.ToolType.Pickaxe:
                    if (obstacleStatus == FarmObstacleStatus.Rock) SetObstacleStatus(FarmObstacleStatus.None);
                    break;
            }

            return;
        }

        SeedData seedTool = toolSlot as SeedData;

        // de trong cay can : cam hat giong, dat trong trot hoac dat uot, ko co cay nao dc trong htai.
        if(seedTool != null && landStatus != LandStatus.Soil && cropPlanted == null && obstacleStatus == FarmObstacleStatus.None)
        {
            SpawnCrop();

            cropPlanted.Plant(id,seedTool);

            // consume item
            InventoryManager.instance.ConsumeItem(InventoryManager.instance.GetEquippedSlot(InventorySlot.InventoryType.Tool));
        }

    }

    public CropBehaviour SpawnCrop()
    {
        GameObject cropObject = Instantiate(cropPrefab, transform);

        cropObject.transform.position = new Vector3(transform.position.x, 0f, transform.position.z);

        cropPlanted = cropObject.GetComponent<CropBehaviour>();
        
        return cropPlanted;
    }

    public void ClockUpdate(GameTimeStamp timestamp)
    {
      if(landStatus == LandStatus.Watered)
        {
            // khoang thoi gian tu khi cay dc tuoi
           int hoursElapsed = GameTimeStamp.CompareTimestamps(timeWatered, timestamp);
           Debug.Log(hoursElapsed);
            
            if(cropPlanted != null)
            {
                cropPlanted.Grow();
            }

           if(hoursElapsed > 24)
            {
                SwitchLandStatus(LandStatus.Farmland);
            }
        }

      if(landStatus != LandStatus.Watered && cropPlanted != null)
        {
            if(cropPlanted.cropState != CropBehaviour.CropState.Seed)
            {
                cropPlanted.Wither();
            }
        }
    }

    private void OnDestroy()
    {
        //unsubcribe from the list on destroy
        TimeManager.instance.UnregisterTracker(this);
    }
}
