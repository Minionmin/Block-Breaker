using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Buff List", menuName = "Scriptable Object/New Buff List")]
public class SO_Buffs : ScriptableObject
{
    public List<string> buffTexts;
}
