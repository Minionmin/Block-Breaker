/**********************************************
 * 
 *  SpeedBuff.cs 
 *  Speed-type buff template (this class shouldn't be used directly, use the child classes)
 * 
 *  製作者：Phansuwan Chaichumphon （ミン）
 * 
 **********************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBuff : Buff
{
    private void Awake()
    {
        // Define buff type (Speed)
        buffType = BarBuff.SpeedBuff;
    }

    public override void Apply()
    {
        // For readability
        MoveBar moveBar = MoveBar.Instance;

        // Add buff's value to bar's movement speed
        moveBar.SetBarMovespeed(moveBar.GetBarMovespeed() + buffValue);
    }
}
