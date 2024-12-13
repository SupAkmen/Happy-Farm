using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldBubble : MonoBehaviour
{
    Transform cameraPos;
    [SerializeField]
    Animator speechAnimator;

    public enum Emote
    {
        Happy,Bad,Heart,Thinking,Sad
    }
    void Start()
    {
        cameraPos = FindObjectOfType<CameraController>().transform;
    }

    public void Display(Emote mood)
    {
        ResetAnimator();
        speechAnimator.SetBool(mood.ToString(), true);
    }

    public void Display(Emote mood,float time)
    {
        Display(mood);
        StartCoroutine(Delay(time));
    }

    IEnumerator Delay(float time)
    {
        yield return new WaitForSeconds(time);
        ResetAnimator();
        gameObject.SetActive(false);
    }

    public void ResetAnimator()
    {
        foreach(AnimatorControllerParameter param in speechAnimator.parameters)
        {
            speechAnimator.SetBool(param.name, false);
        }
    }

    private void OnDisable()
    {
        ResetAnimator() ;
    }

    void Update()
    {
        transform.rotation = cameraPos.rotation;
    }
}
