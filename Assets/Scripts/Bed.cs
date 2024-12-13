using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bed : InteractableObject
{
    public override void PickUp()
    {
        UIManager.instance.TriggerYesNoPrompt("Do you want to sleep",GameStateManager.instance.Sleep);
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if(other.CompareTag("Player"))
    //    {
    //        PickUp();
    //    }    
    //}

}
