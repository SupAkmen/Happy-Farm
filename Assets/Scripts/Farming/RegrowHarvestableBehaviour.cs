using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegrowHarvestableBehaviour : InteractableObject
{
    CropBehaviour parentCrop;

    public void SetParent(CropBehaviour parentCrop)
    {
           this.parentCrop = parentCrop;
    }
    public override void PickUp()
    {
        InventoryManager.instance.EquipHandSlot(item);
        InventoryManager.instance.RenderHand();

        parentCrop.Regrow();

    }
}
