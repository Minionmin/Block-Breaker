using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadePlaneUI : MonoBehaviour
{
    public static FadePlaneUI Instance { get; private set; }
    public Image fadeImage { get; private set; }
    public Sequence fadeSeq;

    private void Awake()
    {
        Instance = this;
        fadeImage = GetComponent<Image>();
    }

    private void Start()
    {
        fadeSeq = DOTween.Sequence(fadeImage)
        .SetAutoKill(false)
        .Append(fadeImage.DOFade(0.0f, 0.5f));
    }
}
