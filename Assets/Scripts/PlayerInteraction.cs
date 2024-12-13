using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    PlayerController playerController;
     
    Land selectedLand = null;

    InteractableObject selectedObject = null;

    void Start()
    {
        playerController = transform.parent.GetComponent<PlayerController>();
    }

   
    void Update()
    {
        RaycastHit hit;

        Debug.DrawRay(transform.position, Vector3.down * 1, Color.red);

        if (Physics.Raycast(transform.position,Vector3.down, out hit, 1))
        {
            OnInteractableHit(hit);
        }
    }

    void OnInteractableHit(RaycastHit hit)
    {
        Collider other = hit.collider;

        if(other.tag == "Land")
        {
            Land land = other.GetComponent<Land>();
            Select(land);
            return;
        } 
        
        if(other.tag == "Item")
        {
           selectedObject = other.GetComponent<InteractableObject>();
           return;
        }

        // deselect the interactable if the player is not standing on anything at the moment
        if(selectedObject != null)
        {
            selectedObject = null;
        }

        if (selectedLand != null)
        {
            selectedLand.Select(false);
            selectedLand = null;
        }

    }

    void Select(Land land)
    {
        if (selectedLand != null)
        {
            selectedLand.Select(false);
        }

        selectedLand = land;
        land.Select(true);
    }

    public void Interact()
    {
        if(InventoryManager.instance.SlotEquipped(InventorySlot.InventoryType.Item))
        {
            return;
        }
        if(selectedLand != null)
        {
            selectedLand.Interact();
            return;
        }

        Debug.Log("Not on any land");
    }

    public void ItemInteract()
    {
        if(selectedObject != null)
        {
            // pick it up
            selectedObject.PickUp();
        }
    }

    public void ItemKeep()
    {
        if (InventoryManager.instance.SlotEquipped(InventorySlot.InventoryType.Item))
        {
            InventoryManager.instance.HandToInventory(InventorySlot.InventoryType.Item);
            return;
        }

    }
}
