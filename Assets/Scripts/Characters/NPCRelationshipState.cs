using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NPCRelationshipState : MonoBehaviour
{
    public string name;
    public int friendshipPoints;

    public bool hasTalkedToday;
    public bool giftGivenToday;

    public float Hearts()
    {
        return friendshipPoints / 250;
    }

    public NPCRelationshipState(string name)
    {
        this.name = name;
        friendshipPoints = 0;
    }

    public NPCRelationshipState(string name, int friendshipPoints)
    {
        this.name = name;
        this.friendshipPoints = friendshipPoints;
    }
}
