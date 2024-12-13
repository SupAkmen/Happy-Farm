using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NamingPrompt : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI promptText;
    [SerializeField] TMP_InputField inputField;

    Action<string> onConfirm;
    Action onPromptComplete;

    public void QueuePromptAction(Action action)
    {
        onPromptComplete += action;
    }

    public void CreatePrompt(string message,Action<string> onConfirm)
    {
        this.onConfirm = onConfirm;

        promptText.text = message;
    }

    public void Confirm()
    {
        // invoke the callback and pass in the input field string
        onConfirm?.Invoke(inputField.text);

        // Reset the acyion
        onConfirm = null;

        inputField.text = "";

        gameObject.SetActive(false);

        onPromptComplete?.Invoke();
        onPromptComplete = null;
    }
}
