/**********************************************
 * 
 *  Buff.cs 
 *  All of the "Buff" parent class
 * 
 *  製作者：Phansuwan Chaichumphon （ミン）
 * 
 **********************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BarBuff
{
    SpeedBuff,
    SizeBuff,
    HPBuff
}

public class Buff : MonoBehaviour
{
    public string buffName { get; protected set; }
    public string buffDesc { get; protected set; }
    public BarBuff buffType { get; protected set; }
    public float buffValue { get; protected set; }

    public virtual void Apply() { }
}
