using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBarUI : MonoBehaviour
{
    public static HPBarUI Instance { get; private set; }

    [SerializeField] private Image hpGauge;


    private void Awake()
    {
        Instance = this;
    }

    public void UpdateHPSliderUI(int currentHealth, int maxHealth)
    {
        hpGauge.fillAmount = (float)currentHealth / maxHealth;
    }
}
