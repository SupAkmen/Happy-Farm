using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance { get; private set; }

    [Header("Dialogue")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI speakerText;
    public TextMeshProUGUI dialogueText;

    Queue<DialogueLine> dialogueQueue;
    Action onDialogueEnd = null;

    bool isTyping = false;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    public void StartDialogues(List<DialogueLine> dialogueLinesToQueue)
    {
        // chuyen list vao queue
        dialogueQueue = new Queue<DialogueLine>(dialogueLinesToQueue);
        UpdateDialogue();
    } 
    
    // khoi tao hoi thoia nhung voi 1 action thuc thi sau khi ket thuc
    public void StartDialogues(List<DialogueLine> dialogueLinesToQueue,Action onDialogueEnd)
    {
        StartDialogues(dialogueLinesToQueue);
        this.onDialogueEnd = onDialogueEnd;
    }

    // duyet qua tung dong hoi thoai
    public void UpdateDialogue()
    {
        if(isTyping)
        {
            isTyping = false;
            return;
        }
        // reset dialogue ve rong trc khi doan hoi thoai tiep
        dialogueText.text = string.Empty;

        if(dialogueQueue.Count == 0)
        {
            EndDialogue();
            return;
        }

        DialogueLine line = dialogueQueue.Dequeue();

        Talk(line.speaker, line.message);
    }

    public void EndDialogue()
    {
        dialoguePanel.SetActive(false);

        onDialogueEnd?.Invoke();

        // reset the action
        onDialogueEnd = null;
    }

    public void Talk(string speaker,string message)
    {
        dialoguePanel.SetActive(true);

        speakerText.text = speaker;

        speakerText.transform.parent.gameObject.SetActive(speaker != " ");

        //dialogueText.text = message;
        StartCoroutine(TypeText(message));

    }

    IEnumerator TypeText(string textToType)
    {
        isTyping = true;
        char[] charsToType = textToType.ToCharArray();
        for(int i=0; i<charsToType.Length; i++)
        {
            dialogueText.text += charsToType[i];
            yield return new WaitForEndOfFrame();

            if(!isTyping)
            {
                dialogueText.text = textToType;
                break;
            }
        }

        // typing is complete
        isTyping=false;
    }    

    public static List<DialogueLine> CreateSimpleMessage(string message)
    {
        // the dialogue you want to output
        DialogueLine messageDialogueLine = new DialogueLine("",message);

        List<DialogueLine> listToReturn = new List<DialogueLine>();

        listToReturn.Add(messageDialogueLine);

        return listToReturn;
    }

}
