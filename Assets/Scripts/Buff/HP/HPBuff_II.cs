using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBuff_II : HPBuff
{
    private void Awake()
    {
        buffName = "HPBuff_II";
        buffValue = 2.0f;
        buffDesc = $"Heal you by {(int)buffValue}";
    }

    public override void Apply()
    {
        base.Apply();
    }
}
