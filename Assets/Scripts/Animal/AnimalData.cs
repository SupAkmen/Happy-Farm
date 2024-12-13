using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Animals/Animal")]
public class AnimalData : ScriptableObject
{
    public Sprite portrait;

    public AnimalBehaviour animalObject;

    // so ngay de truong thanh
    public int dayToMature;

    public int purchasePrice;

    public ItemData produce;

    public SceneTransitionManager.Location locationToSpawn;
}
