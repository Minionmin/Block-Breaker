using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    public static PauseUI Instance { get; private set; } 

    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Image fadePlane;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        mainMenuButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(Loader.LoaderScene.MainMenu.ToString());
            DestroyAllDoNotDestroyOnLoad();
            Time.timeScale = 1.0f;
        });

        // Quit on clicked
        exitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }

    private void DestroyAllDoNotDestroyOnLoad()
    {
        DontDestroy doNotDestroyParent = GameObject.Find("Do Not Destroy").GetComponent<DontDestroy>();
        Destroy(doNotDestroyParent.gameObject);
    }

    public void Show()
    {
        // Pause the game on opening pause menu
        Time.timeScale = 0.0f;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        // Resume the game on closing pause menu
        Time.timeScale = 1.0f;
        gameObject.SetActive(false);
    }
}
