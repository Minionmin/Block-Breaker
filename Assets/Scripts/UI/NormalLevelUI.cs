using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class NormalLevelUI : MonoBehaviour
{
    public static NormalLevelUI Instance;

    /// <summary> Level label </summary>
    [SerializeField] private TextMeshProUGUI levelLabel;

    /// <summary> End position for animation </summary>
    [SerializeField] private RectTransform targetRect;

    /// <summary> End scale for animation </summary>
    [SerializeField] private float targetScale;

    /// <summary> Level start animation </summary>
    public Sequence startSequence;

    private void Awake()
    {
        Instance = this;
    }
    
    private void Start()
    {
    }

    public void PlayLevelStartAnimation()
    {
        // Check for current level text
        SetLevelText();

        levelLabel.rectTransform.localPosition = Vector3.zero + new Vector3(0f, 300f, 0f);
        levelLabel.rectTransform.localScale = Vector3.zero;

        levelLabel.rectTransform.DOScale(targetScale, 1f)
            .onComplete = () =>
            {
                startSequence = DOTween.Sequence()
                    .SetLink(gameObject)
                    .Append(levelLabel.rectTransform.DOLocalMove(targetRect.localPosition, 0.2f))
                    .Append(levelLabel.rectTransform.DOScale(Vector3.one, 0.2f))
                    .OnComplete(() =>
                    {
                        CameraManager.Instance.Shake(0.3f, 3f, 60);
                    });
            };

    }

    private void SetLevelText()
    {
        var currentScene = SceneManager.GetActiveScene().name;

        if (currentScene == SceneName.A_0)
        {
            levelLabel.text = "A0";
        }
        else if (currentScene == SceneName.A_1)
        {
            levelLabel.text = "A1";
        }
        else if (currentScene == SceneName.A_2)
        {
            levelLabel.text = "A2";
        }
        else if (currentScene == SceneName.A_3)
        {
            levelLabel.text = "A3";
        }
        else if (currentScene == SceneName.A_4)
        {
            levelLabel.text = "A4";
        }
        else if (currentScene == SceneName.A_BOSS)
        {
            levelLabel.text = "ABOSS";
        }
    }

    /// <summary> Show this UI </summary>
    public void Show()
    {
        gameObject.SetActive(true);
    }

    /// <summary> Hide this UI </summary>
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
