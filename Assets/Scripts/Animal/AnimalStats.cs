using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AnimalStats : MonoBehaviour
{
    // tt mqh giua nguoi choi vs animal
    public static List<AnimalRelationshipState> animalRelationships = new List<AnimalRelationshipState>();

    // load tt animal
    static List<AnimalData> animals = Resources.LoadAll<AnimalData>("Animals").ToList();

    public static void StartAnimalCreation(AnimalData animalType)
    {
        UIManager.instance.TriggerNamingPrompt($"Give your new {animalType.name} a name", (inputStr) =>
        {
            // create a new animal and add it to the animal relationship
            animalRelationships.Add(new AnimalRelationshipState(inputStr, animalType));
        }
        );
    }

    public static void LoadStats(List<AnimalRelationshipState> relationshipsToLoad)
    {
        if(relationshipsToLoad == null)
        {
            animalRelationships = new List<AnimalRelationshipState>();
            return;
        }
        Debug.Log("load animal data");
        animalRelationships = relationshipsToLoad;
    }

    // get animal by type
    public static List<AnimalRelationshipState> GetAnimalsByType(string animalTypeName)
    {
        return animalRelationships.FindAll(x => x.animalType == animalTypeName);
    } 
    
    public static List<AnimalRelationshipState> GetAnimalsByType(AnimalData animalType)
    {
        return GetAnimalsByType(animalType.name);
    }

    public static void OnDayReset()
    {
        foreach (AnimalRelationshipState animal in AnimalStats.animalRelationships)
        {
            // increase frinedshippoint if you has spoken with the animal
            if (animal.hasTalkedToday)
            {
                animal.friendshipPoints += 30;
            }
            else
            {
                animal.friendshipPoints -= ((10 - (animal.friendshipPoints/200)));
            }

            if(animal.giftGivenToday)
            {
                animal.Mood += 15;
            }
            else
            {
                animal.Mood -= 100;
                animal.friendshipPoints -= 20;
            }



            animal.hasTalkedToday = false;
            // when you fed
            animal.giftGivenToday = false;
            animal.givenProduceToday = false;

            //Advance the age of animal
            animal.age++;
        }
    }
        

    // get the animal type from a string
    public static AnimalData GetAnimalTypeFromString(string name)
    {
        return animals.Find(i=>i.name == name);
    }

}
