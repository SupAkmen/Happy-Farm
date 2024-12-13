using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationManager : MonoBehaviour
{
    public static LocationManager Instance;

    public List<StartPoint> startPoints;

    private void Awake()
    {
        // if there is more than 1 instance,destroy gameobject
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

 
    }

    public Transform GetPlayerStartingPoint(SceneTransitionManager.Location enteringFrom)
    {
        StartPoint startingPoint = startPoints.Find(x=> x.enteringFrom == enteringFrom);

        return startingPoint.playerStart;
    }
}
