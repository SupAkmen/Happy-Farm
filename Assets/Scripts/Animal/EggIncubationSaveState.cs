using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EggIncubationSaveState
{
    public int incubatorID;
    public int timeToIncubate;

    public EggIncubationSaveState(int incubatorID, int timeToIncubate)
    {
        this.incubatorID = incubatorID;
        this.timeToIncubate = timeToIncubate;
    }

    public void Tick()
    {
        timeToIncubate--;
        Debug.Log(incubatorID + " " + timeToIncubate);
    }
}
