using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPCRelationshipListing : MonoBehaviour
{
    [Header("Sprites")]
    public Sprite emptyHeart, fullHeart;

    [Header("UI Elements")]
    public Image portraitImage;
    public TextMeshProUGUI nameText;
    public Image[] hearts;

    public void Display(CharacterData characterData,NPCRelationshipState relationship)
    {
      
        portraitImage.sprite = characterData.portrait;
        nameText.text = relationship.name;

        DisplayHearts(relationship.Hearts());
    }
    public void Display(AnimalData animalData,AnimalRelationshipState relationship)
    {
      
        portraitImage.sprite = animalData.portrait;
        nameText.text = relationship.name;

        DisplayHearts(relationship.Hearts());
    }

    public void DisplayHearts(float number)
    {
        foreach (Image heart in hearts)
        {
            heart.sprite = emptyHeart;
        }

        for(int i = 0; i < number; i++)
        {
            hearts[i].sprite = fullHeart;
        }
    }
}
