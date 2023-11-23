using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBuff_III : HPBuff
{
    private void Awake()
    {
        buffName = "HPBuff_III";
        buffValue = 3.0f;
        buffDesc = $"Heal you by {(int)buffValue}";
    }

    public override void Apply()
    {
        base.Apply();
    }
}
