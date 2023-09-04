using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LifeManager : MonoBehaviour
{
    public static LifeManager Instance { get; private set; }

    private GameObject mainUI;
    private TextMeshProUGUI lifeText;

    [SerializeField] private int life;

    private void Awake()
    {
        Instance = this;

        lifeText = GameObject.FindGameObjectWithTag("LifeTextTag").GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        UpdateVisual();
    }

    public void UpdateVisual()
    {
        lifeText.text = $"LIFE: {life}";
    }

    public void DecreaseLife()
    {
        life--;
    }

    public int GetLife()
    {
        return life;
    }

    public void SetLife(int life)
    {
        this.life = life;
    }
}
