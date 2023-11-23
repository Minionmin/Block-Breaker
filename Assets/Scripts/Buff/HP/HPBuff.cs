/**********************************************
 * 
 *  HPBuff.cs 
 *  Hp-type buff template (this class shouldn't be used directly, use the child classes)
 * 
 *  製作者：Phansuwan Chaichumphon （ミン）
 * 
 **********************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBuff : Buff
{
    private void Awake()
    {
        // Define buff type (Size)
        buffType = BarBuff.HPBuff;
    }

    public override void Apply()
    {
        // Get LifeManager singleton and modify hp
        LifeManager.Instance.IncreaseLife((int)buffValue);
    }
}
