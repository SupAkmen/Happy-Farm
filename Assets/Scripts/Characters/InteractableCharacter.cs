using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableCharacter : InteractableObject
{
    public CharacterData characterData;

    // luu tru thong tin quann he cuar NPC
    NPCRelationshipState relationship;

    Quaternion defaultRotation;

    // ktra neu LookAt is being excuted
    bool isTurning = false;

    private void Start()
    {
        relationship = RelationshipStats.GetRelationship(characterData);

        defaultRotation = transform.rotation;
    }
    public override void PickUp()
    {
        LookAtPlayer();
        TriggerDialogue();
    }

    #region Rotation
    // khi dang noi chuyen qua mat ve phia ngoi choi
    void LookAtPlayer()
    {
        // get player transform
        Transform player = FindObjectOfType<PlayerController>().transform;
        // huong
        Vector3 dir = player.position - transform.position;
        // lock dir y vi ko can npc xaoy chieu y
        dir.y = 0;
        // convert the dir vector to quanternion
        Quaternion lookRot = Quaternion.LookRotation(dir);
        // look player
        StartCoroutine(LookAt(lookRot));
    }

    // back ve rotatio
    void ResetRotation()
    {
        StartCoroutine(LookAt(defaultRotation));
    }

    IEnumerator LookAt(Quaternion lookRoot)
    {
        if(isTurning)
        {
            isTurning= false;
        }
        else
        {
            isTurning = true;
        }
        while(transform.rotation != lookRoot)
        {
            if(!isTurning)
            {
                // stop coroutine
                yield break;
            }
            transform.rotation = Quaternion.RotateTowards(transform.rotation,lookRoot,720 * Time.fixedDeltaTime);
            yield return new WaitForFixedUpdate();
        }
        isTurning = false;
    }

    #endregion

    #region Dialogue Conversation
    void TriggerDialogue()
    {
        //check if the player is holing anything

        if (InventoryManager.instance.SlotEquipped(InventorySlot.InventoryType.Item))
        {
            GiftDialogue();
            Debug.Log("Tang qua");
            return;
        }

        List<DialogueLine> dialogueToHave = characterData.defaultDialogue;

        Action onDialogueEnd = null;

        // ket thuc hoi thoai se quay ve rotate ban dau
        onDialogueEnd += ResetRotation;

        // neu la lan dau gap
        if (RelationshipStats.FirstMeeting(characterData))
        {
            dialogueToHave = characterData.onFirstMeet;

            onDialogueEnd += OnFirstMeeting;
        }

        if (RelationshipStats.IsFirstConversationOfTheDay(characterData))
        {
            onDialogueEnd += OnFirstConversation;
        }

        DialogueManager.instance.StartDialogues(dialogueToHave, onDialogueEnd);
    }
    void OnFirstConversation()
    {
        Debug.Log("This is the first conversation of the day");

        // add point
        RelationshipStats.AddFriendPoints(characterData, 20);

        relationship.hasTalkedToday = true;
    }

    //handle gift giving
    void GiftDialogue()
    {
        Debug.Log("Toi muon tang quan");
        if (!EligibleForGift()) return;
        Debug.Log("Toi da tang quan");
        
        // lay tt item player holding
        ItemSlotData handSlot = InventoryManager.instance.GetEquippedSlot(InventorySlot.InventoryType.Item);

        List<DialogueLine> dialogueToHave = characterData.neutralGiftDialogue;

        Action onDialogueEnd = () =>
        {
            // danh dau qua hom nay da tang
            relationship.giftGivenToday = true;

            // remove item from player
            InventoryManager.instance.ConsumeItem(handSlot);
        }
        ;

        // ket thuc hoi thoai se quay ve rotate ban dau
        onDialogueEnd += ResetRotation;

        bool isBirthday = RelationshipStats.IsBirthday(characterData);

        int pointsToAdd = 0;

        // ktra de xac dinh xem dialog nao se xuat hien
        switch(RelationshipStats.GetReactionToGift(characterData,handSlot.itemData))
        {
            case RelationshipStats.GiftReaction.Like:
                dialogueToHave = characterData.likeGiftDialogue;
                pointsToAdd = 80;
                if (isBirthday) dialogueToHave = characterData.birthdayLikeGiftDialogue;
                break;
            case RelationshipStats.GiftReaction.Dislike:
                dialogueToHave = characterData.disLikeGiftDialogue;
                pointsToAdd = -20;
                if (isBirthday) dialogueToHave = characterData.birthdayDisLikeGiftDialogue;

                break;
            case RelationshipStats.GiftReaction.Neutral:
                dialogueToHave = characterData.neutralGiftDialogue;
                pointsToAdd = 20;
                if (isBirthday) dialogueToHave = characterData.neutralGiftDialogue;

                break;
        }

        // nhan diem khi sinh nhat
        if(isBirthday)
        {
            pointsToAdd *= 8;
        }

        RelationshipStats.AddFriendPoints(characterData,pointsToAdd);

        DialogueManager.instance.StartDialogues(dialogueToHave, onDialogueEnd);

    }

    bool EligibleForGift()
    {
        // nguoi choi chua mo khoa nhan vat
        if(RelationshipStats.FirstMeeting(characterData))
        {
            Debug.Log("Ko quen");
            DialogueManager.instance.StartDialogues(DialogueManager.CreateSimpleMessage("You have not unlocked this character yet."));
            return false;
        }
        // nguoi choi da tang qua hom nay
        if (RelationshipStats.GiftGivenToday(characterData))
        {
            Debug.Log("hom nau tang roi");
            DialogueManager.instance.StartDialogues(DialogueManager.CreateSimpleMessage($"You have already given {characterData.name} a gift today."));
            return false;
        }

        return true;
    }

    // mo khoa sau khi lan gap dau tien
    void OnFirstMeeting()
    {
        RelationshipStats.UnlockCharacter(characterData);
        // cap nhat tt mqh
        relationship = RelationshipStats.GetRelationship(characterData);

    }
    #endregion

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        PickUp();
    //    }
    //}
}
