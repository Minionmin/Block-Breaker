using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamemodeUI : MonoBehaviour
{
    public static GamemodeUI Instance { get; private set; }

    [SerializeField] private Button normalButton;
    [SerializeField] private Button endlessButton;
    [SerializeField] private Image fadePlane;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        normalButton.onClick.AddListener(() => {
            fadePlane.DOFade(1.0f, 0.5f).onComplete = () => Loader.Load(SceneName.A_0);
        });

        endlessButton.onClick.AddListener(() => {
            fadePlane.DOFade(1.0f, 0.5f).onComplete = () => Loader.Load(SceneName.ENDLESS);
        });

        Hide();
    }

    public void Show()
    {
        // Animation
        normalButton.transform.localScale = Vector3.zero;
        endlessButton.transform.localScale = Vector3.zero;
        normalButton.transform.DOScale(Vector3.one, 0.8f).SetEase(Ease.OutBack);
        endlessButton.transform.DOScale(Vector3.one, 0.8f).SetEase(Ease.OutBack);

        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
