using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NextMatchUI : MonoBehaviour
{
    public static NextMatchUI Instance { get; private set; }

    [SerializeField] private Button nextButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Image fadePlane;

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
        fadePlane.DOFade(1.0f, 0.5f)
           .onComplete = () => 
           {
               GameHandler.Instance.ActivateBall();
               Loader.Load(Loader.GetNextLevel()); 
           };
    }

    private void ReturnToMainMenu()
    {
        fadePlane.DOFade(1.0f, 1.0f)
            .onComplete = () =>
            {
                SceneManager.LoadScene(SceneName.MAIN_MENU);
                // We used FindObjectWithTag in the previous version but for the better performance
                // we are now using SerializeField instead
                Destroy(DontDestroy.Instance.gameObject);
            };
    }

    public void Show()
    {
        Vector3 nextOriginalPosition = nextButton.transform.localPosition;
        nextButton.transform.localPosition = new Vector3(nextButton.transform.localPosition.x, -1000.0f, nextButton.transform.localPosition.z);
        nextButton.transform.DOLocalMove(nextOriginalPosition, 1.0f, false).SetEase(Ease.OutBack, 0.5f);

        Vector3 mainmenuOriginalPosition = mainMenuButton.transform.localPosition;
        mainMenuButton.transform.localPosition = new Vector3(mainMenuButton.transform.localPosition.x, -1000.0f, mainMenuButton.transform.localPosition.z);
        mainMenuButton.transform.DOLocalMove(mainmenuOriginalPosition, 1.0f, false).SetEase(Ease.OutBack, 0.5f);

        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
