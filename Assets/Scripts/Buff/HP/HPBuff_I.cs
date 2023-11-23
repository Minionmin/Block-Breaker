using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBuff_I : HPBuff
{
    private void Awake()
    {
        buffName = "HPBuff_I";
        buffValue = 1.0f;
        buffDesc = $"Heal you by {(int)buffValue}";
    }

    public override void Apply()
    {
        base.Apply();
    }
}
