using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableObject : MonoBehaviour
{

    public ItemData item;

    public UnityEvent onInteract = new UnityEvent();
    public virtual void PickUp()
    {
        onInteract?.Invoke();

        InventoryManager.instance.EquipHandSlot(item);
        InventoryManager.instance.RenderHand();
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PickUp();
        }
    }
}
