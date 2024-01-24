using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HowToPlayUI : MonoBehaviour
{
    public static HowToPlayUI Instance { get; private set; }

    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private RectTransform background;

    [Header("How to play visual")]
    [SerializeField] private List<GameObject> howToPlayGuides;

    [Header("Buttons")]
    [SerializeField] private Button nextButton1;
    [SerializeField] private Button nextButton2;
    [SerializeField] private Button nextButton3;
    [SerializeField] private Button nextButton4;

    [SerializeField] private Button backButton2;
    [SerializeField] private Button backButton3;
    [SerializeField] private Button backButton4;
    [SerializeField] private Button backButton5;

    [SerializeField] private Button closeButton;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Initialize();
    }

    /// <summary> Initializing buttons </summary>
    private void Initialize()
    {
        // Hide all guides then show the 1st one
        foreach (GameObject guide in howToPlayGuides)
        {
            HideGuide(guide);
        }
        ShowGuide(howToPlayGuides[0]);

        // Next Buttons
        nextButton1.onClick.AddListener(() =>
        {
            // Hide 1st Page
            HideGuide(howToPlayGuides[0]);
            // Show 2nd page
            ShowGuide(howToPlayGuides[1]);
        });
        nextButton2.onClick.AddListener(() =>
        {
            // Hide 2nd Page
            HideGuide(howToPlayGuides[1]);
            // Show 3rd page
            ShowGuide(howToPlayGuides[2]);
        });
        nextButton3.onClick.AddListener(() =>
        {
            // Hide 3rd Page
            HideGuide(howToPlayGuides[2]);
            // Show 4th page
            ShowGuide(howToPlayGuides[3]);
        });
        nextButton4.onClick.AddListener(() =>
        {
            // Hide 4th Page
            HideGuide(howToPlayGuides[3]);
            // Show 5th page
            ShowGuide(howToPlayGuides[4]);
        });

        // Back buttons
        backButton2.onClick.AddListener(() =>
        {
            // Hide 2nd page
            HideGuide(howToPlayGuides[1]);
            // Show 1st page
            ShowGuide(howToPlayGuides[0]);
        });
        backButton3.onClick.AddListener(() =>
        {
            // Hide 3rd page
            HideGuide(howToPlayGuides[2]);
            // Show 2nd page
            ShowGuide(howToPlayGuides[1]);
        });
        backButton4.onClick.AddListener(() =>
        {
            // Hide 4th page
            HideGuide(howToPlayGuides[3]);
            // Show 3rd page
            ShowGuide(howToPlayGuides[2]);
        });
        backButton5.onClick.AddListener(() =>
        {
            // Hide 5th page
            HideGuide(howToPlayGuides[4]);
            // Show 4th page
            ShowGuide(howToPlayGuides[3]);
        });

        // Close button
        closeButton.onClick.AddListener(() =>
        {
            Hide();
        });

        gameObject.SetActive(false);
    }

    /// <summary> Show target page </summary>
    private void ShowGuide(GameObject howToPlayVisual)
    {
        howToPlayVisual.gameObject.SetActive(true);
    }

    /// <summary> Hide target page </summary>
    private void HideGuide(GameObject howToPlayVisual)
    {
        howToPlayVisual.gameObject.SetActive(false);
    }

    /// <summary> Show this UI </summary>
    public void Show()
    {
        background.localScale = Vector3.one;

        rectTransform.localScale = Vector3.zero;
        rectTransform.DOScale(Vector3.one, 0.6f).SetEase(Ease.OutBack);

        gameObject.SetActive(true);
    }

    /// <summary> Hide this UI </summary>
    private void Hide()
    {
        background.localScale = Vector3.zero;

        rectTransform.localScale = Vector3.one;
        rectTransform.DOScale(Vector3.zero, 0.6f).SetEase(Ease.OutBack, 0.1f)
            .onComplete = () => { gameObject.SetActive(false); };
    }
}
