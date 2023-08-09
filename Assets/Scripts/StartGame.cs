using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(GameStart());
    }

    void Update()
    {
        
    }
    private IEnumerator GameStart()
    {
        /*AsyncOperation loadMain = SceneManager.LoadSceneAsync("Main", LoadSceneMode.Additive);
        while (loadMain.progress < 0.9f)
        {
            yield return null;
        }
        loadMain.allowSceneActivation = false;
        if (!PlayerPrefs.HasKey("level"))
        {
            PlayerPrefs.SetInt("level", 1);
        }
        AsyncOperation loadLevel = SceneManager.LoadSceneAsync("Level " + PlayerPrefs.GetInt("level"), LoadSceneMode.Additive);
        while (loadLevel.progress < 0.9f)
        {
            yield return null;
        }
        loadMain.allowSceneActivation = true;
        while (!loadMain.isDone || !loadLevel.isDone)
        {
            yield return null;
        }*/
        AsyncOperation loadMain = SceneManager.LoadSceneAsync("Main", LoadSceneMode.Additive);
        while (!loadMain.isDone)
        {
            yield return null;
        }
        SceneManager.UnloadSceneAsync("Start_Game");
        yield break;
    }
}
