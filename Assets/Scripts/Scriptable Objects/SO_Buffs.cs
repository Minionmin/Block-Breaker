using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Buff list", menuName = "Scriptable Object/New buff list")]
public class SO_Buffs : ScriptableObject
{
    public List<Buff> buffs;
}
