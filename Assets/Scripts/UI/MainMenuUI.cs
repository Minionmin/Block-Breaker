using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    public static MainMenuUI Instance { get; private set; }

    private const string gameScene = "SampleScene";
    private const string loadingScene = "LoadingScene";

    [SerializeField] private Button startGameButton;
    [SerializeField] private Button exitButton;

    private void Awake()
    {
        startGameButton.onClick.AddListener(() => { Loader.Load(Loader.Scene.SampleScene); });
        exitButton.onClick.AddListener(() => { Application.Quit(); });
    }

    public void LoaderCallback()
    {
        SceneManager.LoadScene(gameScene);
    }
}
