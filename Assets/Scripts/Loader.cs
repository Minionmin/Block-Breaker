using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    public enum Scene
    {
        MainMenu,
        LoadingScene,
        SampleScene,
        Level1,
        GameOverScene
    }

    private static Scene targetScene;
    public static string failedScene;

    public static void Load(Scene targetScene)
    {
        Loader.targetScene = targetScene;

        SceneManager.LoadScene(Scene.LoadingScene.ToString());
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
    }
}
