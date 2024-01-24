using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LifeManager : MonoBehaviour
{
    public static LifeManager Instance { get; private set; }

    /// <summary> Life UI text </summary>
    [SerializeField] private TextMeshProUGUI lifeText;
    /// <summary> Life UI icon </summary>
    [SerializeField] private Image lifeIcon;

    /// <summary> Player's remaining life </summary>
    [SerializeField] private int life;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateVisual();
    }

    /// <summary> Update life UI visual </summary>
    public void UpdateVisual()
    {
        lifeText.text = $"{life}";
    }

    /// <summary> Increase life by input value </summary>
    public void IncreaseLife(int value)
    {
        // Animation
        lifeIcon.transform.DOScale(Vector3.one * 1.5f, 0.5f).SetEase(Ease.InOutQuint)
            .onComplete = () => lifeIcon.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.InOutQuint);

        life += value;
        UpdateVisual();
    }

    /// <summary> Decrease life by input value </summary>
    public void DecreaseLife()
    {
        // Animation
        lifeIcon.transform.DORotate(new Vector3(0.0f, 0.0f, -90.0f), 0.5f, RotateMode.Fast)
            .onComplete = () => lifeIcon.transform.DORotate(Vector3.zero, 0.5f, RotateMode.Fast);

        life--;
        UpdateVisual();
    }

    public int GetLife() { return life; }

    /// <summary> Set life UI and update visual </summary>
    public void SetLife(int life)
    {
        this.life = life;
        UpdateVisual();
    }
}
