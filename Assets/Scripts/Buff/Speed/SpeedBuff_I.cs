using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBuff_I : SpeedBuff
{
    private void Awake()
    {
        buffName = "SpeedBuff_I";
        buffValue = 10.0f;
        buffDesc = "Slightly increase bar's movement speed";
    }

    public override void Apply()
    {
        base.Apply();
    }
}
