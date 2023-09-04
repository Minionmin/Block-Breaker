using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HitUI : MonoBehaviour
{
    // UI visual doesn't need to know about logic
    [SerializeField] private TextMeshProUGUI hitText;

    public void SetHitText(int hitLeft)
    {
        hitText.text = hitLeft.ToString();
    }
}
