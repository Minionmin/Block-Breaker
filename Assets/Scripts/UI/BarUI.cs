using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarUI : MonoBehaviour
{
    public static BarUI Instance { get; private set; }

    [SerializeField] private Image gauge;


    private void Awake()
    {
        Instance = this;
    }

    public void UpdateSliderUI(int currentVal, int maxVal)
    {
        gauge.fillAmount = (float)currentVal / maxVal;
    }
}
