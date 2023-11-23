using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeBuff_II : SizeBuff
{
    private void Awake()
    {
        buffName = "SizeBuff_II";
        buffValue = 1.0f;
        buffDesc = "Moderately increase bar's size";
    }

    public override void Apply()
    {
        base.Apply();
    }
}