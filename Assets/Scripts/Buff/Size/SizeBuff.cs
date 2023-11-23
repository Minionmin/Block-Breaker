/**********************************************
 * 
 *  SizeBuff.cs 
 *  Size-type buff template (this class shouldn't be used directly, use the child classes)
 * 
 *  製作者：Phansuwan Chaichumphon （ミン）
 * 
 **********************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeBuff : Buff
{
    private void Awake()
    {
        // Define buff type (Size)
        buffType = BarBuff.SizeBuff;
    }

    public override void Apply()
    {
        // Get bar object and resize it (scale)
        Transform bar = GameObject.Find("Bar").transform;

        bar.localScale = new Vector3 (bar.localScale.x + buffValue, bar.localScale.y, bar.localScale.z);
    }
}
