using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FarmSaveState 
{
    public List<LandSaveState> landData;
    public List<CropSaveState> cropData;
    public List<EggIncubationSaveState> eggsIncubating;

    public FarmSaveState(List<LandSaveState> landData, 
        List<CropSaveState> cropData, 
        List<EggIncubationSaveState> eggIncubating)
    {
        this.landData = landData;
        this.cropData = cropData;
        this.eggsIncubating = eggIncubating;
    }

    // Exprt data
    public static FarmSaveState Export()
    {
        List<LandSaveState> landData = LandManager.farmData.Item1;
        List<CropSaveState> cropData = LandManager.farmData.Item2;
        List<EggIncubationSaveState> eggsIcubating = IncubationManager.eggIncubating;

        return new FarmSaveState(landData, cropData, eggsIcubating);
    }
    

    // Load in the data
    public void LoadData()
    {
        LandManager.farmData = new System.Tuple<List<LandSaveState>,List<CropSaveState>>(landData, cropData);
        IncubationManager.eggIncubating = eggsIncubating;
    }
}
