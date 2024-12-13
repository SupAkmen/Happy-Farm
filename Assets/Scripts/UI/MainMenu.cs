using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
   public Button loadGameButton;
   public void NewGame()
   {
        StartCoroutine(LoadGameAsync(SceneTransitionManager.Location.PlayerHome,null));
   }
   public void Continue()
   {
        // do muon vao game phai tai canh trc nen ta tai canh trc sau do moi tai du lieu
        StartCoroutine(LoadGameAsync(SceneTransitionManager.Location.PlayerHome, LoadGame));

   }
    // To be called after the scene loaded
    void LoadGame()
    {
        if (GameStateManager.instance == null) return;
        GameStateManager.instance.LoadSave();
    }
   public void Quit()
   {

   }

    IEnumerator LoadGameAsync(SceneTransitionManager.Location scene,Action onFirstFrameLoad )
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene.ToString());

        DontDestroyOnLoad(gameObject);
        while(!asyncLoad.isDone)
        {
            yield return null;
            Debug.Log("Loading");
        }

        // doi cho tat ca cac ham start dc goi thi moi vao
        yield return new WaitForEndOfFrame();
        // if an action is assigned, call it
        onFirstFrameLoad?.Invoke();

        Destroy(gameObject);
    }

    private void Start()
    {
        // bat/tat btn khi co file hay ko
        loadGameButton.interactable = SaveManager.HasSave();
    }
}
