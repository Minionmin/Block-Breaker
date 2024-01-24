using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    public static MainMenuUI Instance { get; private set; }

    [Header("UI")]
    [SerializeField] private Button startGameButton;
    [SerializeField] private RectTransform startButtonRect;
    [SerializeField] private Button howToPlayButton;
    [SerializeField] private RectTransform howToPlayButtonRect;
    [SerializeField] private Button exitButton;
    [SerializeField] private RectTransform exitButtonRect;
    [SerializeField] private RectTransform titleRect;

    [SerializeField] private Button settingButton;

    private void Awake()
    {
        Instance = this;

        startGameButton.onClick.AddListener(() =>
        {
            // Hide Mainmenu UI & 
            // Show Gamemode UI
            PanelFadeOut();
        });

        howToPlayButton.onClick.AddListener(() =>
        {
            HowToPlayUI.Instance.Show();
        });

        exitButton.onClick.AddListener(() => { Application.Quit(); });

        settingButton.onClick.AddListener(() =>
        {
            OptionUI.Instance.Show();
        });
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
        exitButtonRect.transform.DOLocalMove(exitOriginalPosition, 0.8f, false).SetEase(Ease.OutBack, 0.4f);

        // How to play button slides in
        Vector3 howOriginalPosition = howToPlayButtonRect.transform.localPosition;
        howToPlayButtonRect.transform.localPosition = new Vector3(howOriginalPosition.x, -1000.0f, howOriginalPosition.z);
        howToPlayButtonRect.transform.DOLocalMove(howOriginalPosition, 0.7f, false).SetEase(Ease.OutBack, 0.35f);

        // Start button slides in
        Vector3 startOriginalPosition = startButtonRect.transform.localPosition;
        startButtonRect.transform.localPosition = new Vector3(startOriginalPosition.x, -1000.0f, startOriginalPosition.z);
        startButtonRect.transform.DOLocalMove(startOriginalPosition, 0.6f, false)
            .SetEase(Ease.OutBack, 0.3f).onComplete = () =>
            {
                titleRect.transform.DOScale(Vector3.one, 0.8f).SetEase(Ease.OutElastic);
            };
    }

    private void PanelFadeOut()
    {
        exitButtonRect.transform.DOLocalMove(new Vector3(exitButtonRect.localPosition.x, -1000.0f, exitButtonRect.localPosition.z), 0.6f, false).SetEase(Ease.OutBack, 0.3f);
        howToPlayButtonRect.transform.DOLocalMove(new Vector3(howToPlayButtonRect.localPosition.x, -1000.0f, howToPlayButtonRect.localPosition.z), 0.7f, false).SetEase(Ease.OutBack, 0.35f);
        startButtonRect.transform.DOLocalMove(new Vector3(startButtonRect.localPosition.x, -1000.0f, startButtonRect.localPosition.z), 0.8f, false).SetEase(Ease.OutBack, 0.4f)
            .onComplete = () => GamemodeUI.Instance.Show();
        titleRect.transform.DOScale(Vector3.zero, 0.6f).SetEase(Ease.OutQuint);
    }

    public void LoaderCallback()
    {
        SceneManager.LoadScene(SceneName.A_0);
    }
}
