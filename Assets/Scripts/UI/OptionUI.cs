using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour
{
    public static OptionUI Instance { get; private set; }

    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private RectTransform background;

    [SerializeField] private Button sfxButton;
    [SerializeField] private Button musicButton;
    [SerializeField] private Button closeButton;

    [SerializeField] private TextMeshProUGUI sfxLabel;
    [SerializeField] private TextMeshProUGUI musicLabel;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        sfxButton.onClick.AddListener(() =>
        {
            SFXHandler.Instance.ChangeVolume();
            UpdateSFXVolumeText();
        });

        musicButton.onClick.AddListener(() =>
        {
            MusicHandler.Instance.ChangeVolume();
            UpdateMusicVolumeText();
        });

        closeButton.onClick.AddListener(() =>
        {
            Hide();
        });

        UpdateSFXVolumeText();
        UpdateMusicVolumeText();
        gameObject.SetActive(false);
    }

    public void UpdateSFXVolumeText()
    {
        sfxLabel.text = "Sound Effect: " + PlayerPrefs.GetInt(PlayerPrefsKeyword.SFX_VOLUME, 1).ToString();
    }

    public void UpdateMusicVolumeText()
    {
        musicLabel.text = "Music: " + PlayerPrefs.GetInt(PlayerPrefsKeyword.MUSIC_VOLUME, 1).ToString();
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
    public void Hide()
    {
        background.localScale = Vector3.zero;

        rectTransform.localScale = Vector3.one;
        rectTransform.DOScale(Vector3.zero, 0.6f).SetEase(Ease.OutBack, 0.1f)
            .onComplete = () => { gameObject.SetActive(false); };
    }
}
