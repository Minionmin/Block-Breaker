using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LifeManager : MonoBehaviour
{
    public static LifeManager Instance { get; private set; }

    private GameObject mainUI;
    private TextMeshProUGUI lifeText;
    private Image lifeIcon;

    [SerializeField] private int life;

    private void Awake()
    {
        Instance = this;

        // Setup variables
        lifeText = GameObject.FindGameObjectWithTag("LifeTextTag").GetComponent<TextMeshProUGUI>();
        lifeIcon = GameObject.Find("LifeIcon").GetComponent<Image>();
    }

    private void Start()
    {
        UpdateVisual();
    }

    public void UpdateVisual()
    {
        lifeText.text = $"{life}";
    }

    public void IncreaseLife(int value)
    {
        // Animation
        lifeIcon.transform.DOScale(Vector3.one * 1.5f, 0.5f).SetEase(Ease.InOutQuint)
            .onComplete = () => lifeIcon.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.InOutQuint);

        life += value;
        UpdateVisual();
    }

    public void DecreaseLife()
    {
        // Animation
        lifeIcon.transform.DORotate(new Vector3(0.0f, 0.0f, -90.0f), 0.5f, RotateMode.Fast)
            .onComplete = () => lifeIcon.transform.DORotate(Vector3.zero, 0.5f, RotateMode.Fast);

        life--;
        UpdateVisual();
    }

    public int GetLife()
    {
        return life;
    }

    public void SetLife(int life)
    {
        this.life = life;
        UpdateVisual();
    }
}
