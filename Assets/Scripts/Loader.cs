using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;


public static class Loader
{
    public static GameObject musicHandler;
    public static AudioSource audioSource;

    public enum LoaderScene
    {
        MainMenu,
        LoadingScene,
        SampleScene,
        Level1,
        Level2,
        Level3,
        Level4,
        Boss,
        Endless,
        GameOverScene
    }

    private static LoaderScene targetScene;
    public static string failedScene;

    public static void Load(LoaderScene targetScene)
    {
        Loader.targetScene = targetScene;

        PauseBGMBeforeLoadingScreen();

        SceneManager.LoadScene(LoaderScene.LoadingScene.ToString());
    }

    public static void LoadFailedScene()
    {
        SceneManager.LoadScene(failedScene);
        failedScene = "";
    }

    public static IEnumerator LoaderCallback(float timeToWait)
    {
        yield return new WaitForSecondsRealtime(timeToWait);
        SceneManager.LoadScene(targetScene.ToString());

        if(audioSource) audioSource.Play();
    }

    public static void PauseBGMBeforeLoadingScreen()
    {
        musicHandler = GameObject.FindGameObjectWithTag("Music");
        audioSource = musicHandler.GetComponent<AudioSource>();
        audioSource.Pause();
    }

    public static LoaderScene GetNextLevel()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        
        if (currentScene == "SampleScene")
        {
            return LoaderScene.Level1;
        }
        else if (currentScene == "Level1")
        {
            return LoaderScene.Level2;
        }
        else if (currentScene == "Level2")
        {
            return LoaderScene.Level3;
        }
        else if (currentScene == "Level3")
        {
            return LoaderScene.Level4;
        }
        else if (currentScene == "Level4")
        {
            return LoaderScene.Boss;
        }
        else
        {
            Debug.LogError("Not a valid Level");
        }

        LoaderScene nextScene = LoaderScene.MainMenu;
        return nextScene;
    }
}
