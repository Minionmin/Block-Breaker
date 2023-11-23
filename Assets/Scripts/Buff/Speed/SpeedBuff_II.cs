using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBuff_II : SpeedBuff
{
    private void Awake()
    {
        buffName = "SpeedBuff_II";
        buffValue = 20.0f;
        buffDesc = "Moderately increase bar's movement speed";
    }

    public override void Apply()
    {
        base.Apply();
    }
}
