using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Character/Character")]
public class CharacterData : ScriptableObject
{
    public Sprite portrait;
    public GameTimeStamp birthday;
    public List<ItemData> likes;
    public List<ItemData> disLikes;

    [Header("Dialogue")]
    public List<DialogueLine> onFirstMeet;
    public List<DialogueLine> defaultDialogue;

    [Header("Gift Dialogue")]
    public List<DialogueLine> likeGiftDialogue;
    public List<DialogueLine> disLikeGiftDialogue;
    public List<DialogueLine> neutralGiftDialogue;

    [Header("Gift Birthday Dialogue")]
    public List<DialogueLine> birthdayLikeGiftDialogue;
    public List<DialogueLine> birthdayDisLikeGiftDialogue;
    public List<DialogueLine> birthdayNeutralGiftDialogue;
}
