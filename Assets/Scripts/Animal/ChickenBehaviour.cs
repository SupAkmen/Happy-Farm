using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChickenBehaviour : AnimalBehaviour
{
    protected override void Start()
    {
        base.Start();

    }

    void LayEgg()
    {
        // check age
        AnimalData animalType = AnimalStats.GetAnimalTypeFromString(relationship.animalType);

        if(relationship.age < animalType.dayToMature)
        {
            return;
        }

        if(relationship.Mood > 30 && !relationship.givenProduceToday)
        {
            ItemData egg = InventoryManager.instance.GetItemFromString("Egg");
            Instantiate(egg.model,transform.position, Quaternion.identity);
            relationship.giftGivenToday = true;
        }
    }
}
