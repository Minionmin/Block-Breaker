using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public static class Loader
{
    public static GameObject musicHandler;
    public static AudioSource audioSource;

    private static string targetScene;
    public static string failedScene;

    public static void Load(string sceneName)
    {
        targetScene = sceneName;

        PauseBGMBeforeLoadingScreen();

        SceneManager.LoadScene(SceneName.LOADING_SCENE);
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

    public static string GetNextLevel()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        
        if (currentScene == SceneName.A_0)
        {
            return SceneName.A_1;
        }
        else if (currentScene == SceneName.A_1)
        {
            return SceneName.A_2;
        }
        else if (currentScene == SceneName.A_2)
        {
            return SceneName.A_3;
        }
        else if (currentScene == SceneName.A_3)
        {
            return SceneName.A_4;
        }
        else if (currentScene == SceneName.A_4)
        {
            return SceneName.A_BOSS;
        }
        else
        {
            Debug.LogError("Not a valid Level");
            return SceneName.MAIN_MENU;
        }
    }
}
