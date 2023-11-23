using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Block list", menuName = "Scriptable Object/New block list")]
public class SO_Blocks : ScriptableObject
{
    public List<Block> blocks;
}