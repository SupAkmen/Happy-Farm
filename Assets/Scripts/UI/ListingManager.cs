using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ListingManager<T> : MonoBehaviour
{
    // sentry instantiate
    public GameObject listingEntryPrefab;
    // grid to instantiate the entry
    public Transform listingGrid;

    public void Render(List<T> listingItems)
    {
        // reset the listing if there was a previous one
        if (listingGrid.childCount > 0)
        {
            foreach (Transform child in listingGrid)
            {
                Destroy(child.gameObject);
            }
        }

        // crete a new listing for every item
        foreach (T listingItem in listingItems)
        {
            // tao ra 1 shop listing moi
            GameObject listingGameObject = Instantiate(listingEntryPrefab, listingGrid);

            DisplayListing(listingItem, listingGameObject);
        }
    }

    protected abstract void DisplayListing(T listingItem,GameObject listingGameObject);
}
