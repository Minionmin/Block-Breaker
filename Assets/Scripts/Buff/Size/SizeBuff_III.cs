using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeBuff_III : SizeBuff
{
    private void Awake()
    {
        buffName = "SizeBuff_III";
        buffValue = 1.5f;
        buffDesc = "Greatly increase bar's size";
    }

    public override void Apply()
    {
        base.Apply();
    }
}
