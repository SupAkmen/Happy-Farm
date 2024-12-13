using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelationshipStats : MonoBehaviour
{
    // the relationship data of the NPCs that the player has met in the game.
    public static List<NPCRelationshipState> relationships = new List<NPCRelationshipState>();

    public enum GiftReaction
    {
        Like,Dislike,Neutral
    }

    public static void LoadStats(List<NPCRelationshipState> relationshipsToLoad)
    {
        if (relationshipsToLoad == null)
        {
            relationships = new List<NPCRelationshipState>();
            return;
        }
        Debug.Log("load relationdata");
        relationships = relationshipsToLoad;
    }

    // check if the player has met this NPC
    public static bool FirstMeeting(CharacterData character)
    {
        return !relationships.Exists(i=> i.name == character.name);
    }

    // get information relationship abount a character
    public static NPCRelationshipState GetRelationship(CharacterData character)
    {
        // check if it is the first meeting
        if(FirstMeeting(character)) return null;

        return relationships.Find(i => i.name == character.name);
    }

    // add the character to the relationship data
    public static void UnlockCharacter(CharacterData character)
    {
        relationships.Add(new NPCRelationshipState(character.name));
    }

    // add friendpoints
    public static void AddFriendPoints(CharacterData character,int points)
    {
        if(FirstMeeting(character)) 
        {
            Debug.LogError("The player has not met this character yet");
            return;
        }
        GetRelationship(character).friendshipPoints += points;
    }


    // ktra xem co phai cuoc hoi thoai dau tien trongngay ko
    public static bool IsFirstConversationOfTheDay(CharacterData character)
    {
        // if the player is meeting him for the first time, definitely it
        if(FirstMeeting(character)) return true;

        NPCRelationshipState npc = GetRelationship(character);
        return !npc.hasTalkedToday;
    }

    // ktra xem hom nay da tang qua chua
    public static bool GiftGivenToday(CharacterData character)
    {
        NPCRelationshipState npc = GetRelationship(character);
        return npc.giftGivenToday;
    }

    public static GiftReaction GetReactionToGift(CharacterData character,ItemData item)
    {
        if (character.likes.Contains(item)) return GiftReaction.Like;
        if (character.likes.Contains(item)) return GiftReaction.Dislike;

        return GiftReaction.Neutral;
    }

    public static bool IsBirthday(CharacterData character)
    {
        GameTimeStamp birthday = character.birthday;
        GameTimeStamp today = TimeManager.instance.GetGameTimestamp();

        return (today.day == birthday.day) && (today.season == birthday.season);
    }
   
}
