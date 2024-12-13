using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnimalRender : MonoBehaviour
{
    NavMeshAgent agent;

    [SerializeField] GameObject childModel, adultModel;

    GameObject animatorToWorkWith;
    int age;
    AnimalData animalType;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void RenderAnimal(int age,string animalName)
    {
        animalType = AnimalStats.GetAnimalTypeFromString(animalName);

        this.age = age;

        animatorToWorkWith = (age >= animalType.dayToMature) ? adultModel : childModel;

        childModel.gameObject.SetActive(false);
        adultModel.gameObject.SetActive(false);

        // this model we will work with
        animatorToWorkWith.gameObject.SetActive(true);
    }

   
}
