using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NextMatchUI : MonoBehaviour
{
    public event Action OnMainMenuClicked;

    public static NextMatchUI Instance { get; private set; }

    [SerializeField] private Button nextButton;
    [SerializeField] private Button mainMenuButton;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        nextButton.onClick.AddListener(LoadNextLevel);
        mainMenuButton.onClick.AddListener(ReturnToMainMenu);
    }

    private void LoadNextLevel()
    {
        Loader.Load(Loader.GetNextLevel());
    }

    private void ReturnToMainMenu()
    {
        OnMainMenuClicked?.Invoke();
        SceneManager.LoadScene("MainMenu");
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
