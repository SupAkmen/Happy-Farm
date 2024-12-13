using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalListingManager : ListingManager<AnimalRelationshipState>
{
    protected override void DisplayListing(AnimalRelationshipState relationship, GameObject listingGameObject)
    {
        // get the animaldata from the string in the relationship
        AnimalData animalData = AnimalStats.GetAnimalTypeFromString(relationship.animalType);

        listingGameObject.GetComponent<NPCRelationshipListing>().Display(animalData, relationship);
    }
}
