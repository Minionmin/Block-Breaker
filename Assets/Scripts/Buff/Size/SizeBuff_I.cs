using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeBuff_I : SizeBuff
{
    private void Awake()
    {
        buffName = "SizeBuff_I";
        buffValue = 0.5f;
        buffDesc = "Slightly increase bar's size";
    }

    public override void Apply()
    {
        base.Apply();
    }
}
