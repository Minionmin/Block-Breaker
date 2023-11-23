using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBuff_III : SpeedBuff
{
    private void Awake()
    {
        buffName = "SpeedBuff_III";
        buffValue = 30.0f;
        buffDesc = "Greatly increase bar's movement speed";
    }

    public override void Apply()
    {
        base.Apply();
    }
}
