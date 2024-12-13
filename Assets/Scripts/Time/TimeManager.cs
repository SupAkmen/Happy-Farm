using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager instance;

    [Header("Internal Clock")]
    [SerializeField]
    GameTimeStamp timestamp;
    public float timeSacle = 1.0f;

    [Header("Day and night cycle")]
    public Transform sunTransform;
    private float indoorAngle = 40;

    //Observer pattern : List of object to inform of changes to the time
    List<ITimeTracker> listeners = new List<ITimeTracker>();

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
        timestamp = new GameTimeStamp(0, GameTimeStamp.Season.Spring, 1, 6, 0);
        StartCoroutine(TimeUpdate());
    }

    public void LoadTime(GameTimeStamp timestamp)
    {
        this.timestamp = new GameTimeStamp(timestamp);
    }

    IEnumerator TimeUpdate()
    {
        while (true)
        {
            Tick();
            yield return new WaitForSeconds(1 / timeSacle);
        }
    }

    public void Tick()
    {
        timestamp.UpdateClock();

        //inform the ;isteners of the new time state
        foreach (ITimeTracker listener in listeners)
        {
            listener.ClockUpdate(timestamp);
        }

        UpdateSunMovement();
    }

    public void SkipTime(GameTimeStamp timeToSkipTo)
    {
        // convert to minutes
        int timeToSkipInMinutes = GameTimeStamp.TimestampToMinutes(timeToSkipTo);
        Debug.Log("Time to skip : " + timeToSkipInMinutes);
        int timeNowInMinutes = GameTimeStamp.TimestampToMinutes(timestamp);
        Debug.Log("time now : " +  timeNowInMinutes);

        int differenceInMinutes = timeToSkipInMinutes - timeNowInMinutes;
        Debug.Log(differenceInMinutes + " minutes to skip ");

        if (differenceInMinutes <= 0) return;

        for(int i = 0; i < differenceInMinutes; i++)
        {
            Tick();
        }
    }

    // Day and night cycle
    void UpdateSunMovement()
    {
        // disable if indoor
        if(SceneTransitionManager.Instance.CurrentIndoor())
        {
            sunTransform.eulerAngles = new Vector3(indoorAngle, 0, 0);
            return;
        }

        int timeInMinutes = GameTimeStamp.HoursToMinutes(timestamp.hour) + timestamp.minute;
        // sun move 15do/1h => 0.25do/1p ; midnight 
        float sunAngle = 0.25f * timeInMinutes - 90;
        // apply angle to the direct light
        sunTransform.eulerAngles = new Vector3(sunAngle, 0, 0);
    }

    public GameTimeStamp GetGameTimestamp()
    {
        return new GameTimeStamp(timestamp);
    }

    // handing listeners

    // add the object to the list of listeners
    public void RegisterTracker(ITimeTracker listener)
    {
        listeners.Add(listener);
    }
    // remove the object to the list of listeners
    public void UnregisterTracker(ITimeTracker listener)
    {
        listeners.Remove(listener);
    }
}
