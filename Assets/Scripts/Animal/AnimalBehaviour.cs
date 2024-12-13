using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AnimalMovement))]
public class AnimalBehaviour : InteractableObject
{
    protected AnimalRelationshipState relationship;
    protected AnimalMovement movement;
    protected AnimalRender animalRender;
    [SerializeField]
    protected WorldBubble speechBubble;
    protected virtual void Start()
    {
        movement = GetComponent<AnimalMovement>();
    }
    public void LoadRelationship(AnimalRelationshipState relationship)
    {
        this.relationship = relationship;
        animalRender = GetComponent<AnimalRender>();
        animalRender.RenderAnimal(relationship.age, relationship.animalType);
    }

    public override void PickUp()
    {
        //if (relationship == null)
        //{
        //    Debug.Log("relationship not set");
        //    return;
        //}
        TriggerDialouge();
    }

    void TriggerDialouge()
    {
        movement.ToggleMovement(false);

        int mood = relationship.Mood;

        string dialogueLine = $"{relationship.name} seem ";

        Action onDialogueEnd = () =>
        {
            movement.ToggleMovement(true);
        };

        // check if the player has talked with the animal
        if(!relationship.hasTalkedToday)
        {
            onDialogueEnd += OnFirstConversation;
        }

        if(mood >= 200 && mood <= 250)
        {
            dialogueLine += "really happy today";
        }
        else if (mood >= 30 && mood < 200)
        {
            dialogueLine += "fine";
        }
        else
        {
            dialogueLine += "sad";
        }

        DialogueManager.instance.StartDialogues(DialogueManager.CreateSimpleMessage(dialogueLine),onDialogueEnd);
    }

    void OnFirstConversation()
    {
        relationship.Mood += 30;
        relationship.hasTalkedToday = true;

        
        speechBubble.gameObject.SetActive(true);

        WorldBubble.Emote emote = WorldBubble.Emote.Thinking;

        switch(relationship.Mood)
        {
            case int n when (n >= 200):
                emote = WorldBubble.Emote.Heart;
                break;
            case int n when (n < 30):
                emote = WorldBubble.Emote.Sad;
                break;
            case int n when (n>= 30 && n < 60):
                emote = WorldBubble.Emote.Bad;
                break;
            default:
                emote = WorldBubble.Emote.Happy;
                break;
        }

        speechBubble.Display(emote, 3f);
        Debug.Log($"{relationship.name} is now of mood {relationship.Mood}");

    }
}
