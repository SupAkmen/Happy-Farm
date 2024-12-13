using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalFeedManager : MonoBehaviour
{
    // feedboxes for different animal
    public static Dictionary<AnimalData, bool[]> feedboxStatus = new Dictionary<AnimalData, bool[]>();

    public FeedBox[] feedBoxes;

    // the associated
    public AnimalData animal;

    private void OnEnable()
    {
        feedBoxes = GetComponentsInChildren<FeedBox>();
        RegisterFeedBoxes();
        LoadFeedBoxData();
    }

    public static void ResetFeedBoxes()
    {
        feedboxStatus = new Dictionary<AnimalData, bool[]>();
    }

    public void FeedAnimal(int id)
    {
        // get the list of all animals associated with the feed boxes
        List<AnimalRelationshipState> eligibleAnimals = new List<AnimalRelationshipState>();

        // find the first animal we haven't fed yet
        foreach(AnimalRelationshipState a in eligibleAnimals)
        {
            if(!a.giftGivenToday)
            {
                a.giftGivenToday = true;
                Debug.Log(a.name + "is fed.");
                break;
            }
        }
        // update the status accordingly
        feedboxStatus[animal][id] = true;

        LoadFeedBoxData();
    }

    //Assign each feedbox an ID
    void RegisterFeedBoxes()
    {
        for(int i = 0; i < feedBoxes.Length; i++)
        {
            feedBoxes[i].id = i;
        }
    }

    void LoadFeedBoxData()
    {
        // check if feedboxstatus contain the dictionary entry for our animal
        if(!feedboxStatus.ContainsKey(animal))
        {
            //if not create anew entry with default feed box status (false)
            feedboxStatus.Add(animal,new bool[feedBoxes.Length]);
        }

        // get the current feedbox status
        bool[] currentFeedBoxStatus = feedboxStatus[animal];

        for(int i = 0;i < feedBoxes.Length;i++)
        {
            feedBoxes[i].SetFeedState(currentFeedBoxStatus[i]);
        }
    }
}
