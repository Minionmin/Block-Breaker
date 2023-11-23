using DG.Tweening;
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

    [Header("UI")]
    [SerializeField] private Button startGameButton;
    [SerializeField] private RectTransform startButtonRect;
    [SerializeField] private Button exitButton;
    [SerializeField] private RectTransform exitButtonRect;
    [SerializeField] private RectTransform titleRect;

    private void Awake()
    {
        Instance = this;

        startGameButton.onClick.AddListener(() =>
        {
            // Hide Mainmenu UI & 
            // Show Gamemode UI
            PanelFadeOut();
        });

        exitButton.onClick.AddListener(() => { Application.Quit(); });
    }

    private void Start()
    {
        PanelFadeIn();
    }

    private void PanelFadeIn()
    {
        // Title animation setup
        titleRect.transform.localScale = Vector3.zero;

        // Exit button slides in
        Vector3 exitOriginalPosition = exitButtonRect.transform.localPosition;
        exitButtonRect.transform.localPosition = new Vector3(exitOriginalPosition.x, -1000.0f, exitOriginalPosition.z);
        exitButtonRect.transform.DOLocalMove(exitOriginalPosition, 1.5f, false).SetEase(Ease.OutBack, 0.5f);

        // Start button slides in
        Vector3 startOriginalPosition = startButtonRect.transform.localPosition;
        startButtonRect.transform.localPosition = new Vector3(startOriginalPosition.x, -1000.0f, startOriginalPosition.z);
        startButtonRect.transform.DOLocalMove(startOriginalPosition, 2.0f, false)
            .SetEase(Ease.OutBack, 1.0f).onComplete = () =>
            {
                titleRect.transform.DOScale(Vector3.one, 1.5f).SetEase(Ease.OutElastic);
            };
    }

    private void PanelFadeOut()
    {
        exitButtonRect.transform.DOLocalMove(new Vector3(exitButtonRect.localPosition.x, -1000.0f, exitButtonRect.localPosition.z), 1.5f, false).SetEase(Ease.OutBack, 0.5f);
        startButtonRect.transform.DOLocalMove(new Vector3(startButtonRect.localPosition.x, -1000.0f, startButtonRect.localPosition.z), 2.05f, false).SetEase(Ease.OutBack, 1f)
            .onComplete = () => GamemodeUI.Instance.Show();
        titleRect.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.OutQuint);
    }

    public void LoaderCallback()
    {
        SceneManager.LoadScene(gameScene);
    }
}
