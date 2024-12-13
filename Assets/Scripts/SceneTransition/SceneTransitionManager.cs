using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance;
    public enum Location
    {
        Farm,PlayerHome,Town,ChickenCoop
    }
    public Location currentLocation;

    // nhung khu vuc ben trong
    static readonly Location[] indoor = { Location.PlayerHome,Location.ChickenCoop };

    Transform playerPoint;


    bool screenFadeOut;

    private void Awake()
    {
        // if there is more than 1 instance,destroy gameobject
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(gameObject);
        
        // OnLoacationLoad will be called when the scene is loaded
        SceneManager.sceneLoaded += OnLocationLoad;

        // Find player's transform
        playerPoint = FindObjectOfType<PlayerController>().transform;
    }

    public bool CurrentIndoor()
    {
        return indoor.Contains(currentLocation);
    }

    public void SwitchLocation(Location locationSwitch)
    {
        
        UIManager.instance.FadeOutScreen();
        screenFadeOut = false;
        StartCoroutine(ChangeScene(locationSwitch));
    }

    IEnumerator ChangeScene(Location locationSwitch)
    {

        // tat charactercontroller
        CharacterController playerCharacter = playerPoint.GetComponent<CharacterController>();
        playerCharacter.enabled = false;

        while (!screenFadeOut)
        {
            yield return new WaitForSeconds(0.1f);
        }

        screenFadeOut = false;
        UIManager.instance.ResetFadeDefault();
        SceneManager.LoadScene(locationSwitch.ToString());

       
    }
    public void OnFadeOutComplete()
    {
        screenFadeOut = true;
    }

    // call when a scec is loaded
    public void OnLocationLoad(Scene scene,LoadSceneMode mode)
    {
        // reassign the player's transform in case it was destroyed in the scene transition
        if (playerPoint == null)
        {
            PlayerController player = FindObjectOfType<PlayerController>();
            if (player != null)
            {
                playerPoint = player.transform;
            }
            else
            {
                Debug.LogError("PlayerController not found in the scene!");
                return;  // Stop further execution if the player is not found
            }
        }

        // the location player is coming from when the scene load
        Location oldLocation = currentLocation;

        // get the new location 
        Location newLocation = (Location) Enum.Parse(typeof(Location),scene.name);

        //if player is not coming from any new place,stop code
        if (currentLocation == newLocation) return;

        // tat charactercontroller
        CharacterController playerCharacter = playerPoint.GetComponent<CharacterController>();
        playerCharacter.enabled = false;

        // find the start point
        Transform startPoint = LocationManager.Instance.GetPlayerStartingPoint(oldLocation);

        if (playerPoint == null) return;

        // chang the player's pos to the start point
        playerPoint.position = startPoint.position;
        playerPoint.rotation = startPoint.rotation;

        // bat charactercontroller
        playerCharacter.enabled = true;

        // save the current location that we switched to
        currentLocation = newLocation;
    }
}
