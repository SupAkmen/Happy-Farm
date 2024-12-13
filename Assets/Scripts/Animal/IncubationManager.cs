using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class IncubationManager : MonoBehaviour
{
    public static List<EggIncubationSaveState> eggIncubating = new List<EggIncubationSaveState>();

    public const int daysToIncubate = 3;

    public List<Incubator> incubators;

    public static UnityEvent onEggUpdate = new UnityEvent();

    // goij khi player in scene
    private void OnEnable()
    {
        // them id vao moi long ap
        RegisterIncubators();

        // load thong tin long ap
        LoadIncubatorData();

        // load data moi khi cap nhat
        onEggUpdate.AddListener(LoadIncubatorData);
    }

    private void OnDestroy()
    {
        onEggUpdate.RemoveListener(LoadIncubatorData);
    }

    public static void UpdateEggs()
    {
        if (eggIncubating.Count == 0) return;

        foreach(EggIncubationSaveState egg in eggIncubating.ToList())
        {
            egg.Tick();
            onEggUpdate?.Invoke();
            if(egg.timeToIncubate <= 0)
            {
                eggIncubating.Remove(egg);

                // sinh ra ga
                AnimalData chickenData = AnimalStats.GetAnimalTypeFromString("Chicken");
                if (chickenData != null)
                {
                    AnimalStats.StartAnimalCreation(chickenData);
                    Debug.Log("Dat ten cho ga");
                }
                else
                {
                    Debug.LogError("Chicken data not found!");
                }
            }
        }
    }

    // danh so cho cac long ap
    void RegisterIncubators()
    {
        for(int i = 0; i < incubators.Count; i++)
        {
            incubators[i].incubationID = i;
        }
    }

    void LoadIncubatorData()
    {
        if (eggIncubating.Count == 0) return;

        foreach(EggIncubationSaveState egg in eggIncubating)
        {
            // lay id
            Incubator incubatorToLoad = incubators[egg.incubatorID];

            bool isIncubating = true;

            if(egg.timeToIncubate <= 0)
            {
                isIncubating = false;
            }

            incubatorToLoad.SetIncubationState(isIncubating,egg.timeToIncubate);
        }
    }
}
