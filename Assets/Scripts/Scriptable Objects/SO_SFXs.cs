using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New SFX", menuName = "Scriptable Object/New SFX set")]
public class SO_SFXs : ScriptableObject
{
    public AudioClip blockDestroyedSFX;
    public AudioClip explodeBlockDestroyedSFX;
    public AudioClip teleportSFX;
    public AudioClip teleportDoorDestroyedSFX;
    public AudioClip bossHitSFX;
    public AudioClip textSFX;
}
