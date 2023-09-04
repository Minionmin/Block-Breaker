using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SFXHandler : MonoBehaviour
{
    public static SFXHandler Instance {  get; private set; }

    [SerializeField] public SO_SFXs sfxSO;

    private void Awake()
    {
        Instance = this;
    }

    public void PlaySFX(AudioClip audioClip, Vector3 position, float volume = 0.5f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volume);
    }
}
