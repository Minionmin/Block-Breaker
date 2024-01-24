using UnityEngine;

public class SFXHandler : MonoBehaviour
{
    public static SFXHandler Instance {  get; private set; }

    [SerializeField] public SO_SFXs sfxSO;

    /// <summary> SFX volume level </summary>
    public int volume = 1;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // Get player's default setting
        AdjustVolume();
    }

    public void PlaySFX(AudioClip audioClip, Vector3 position, float volumeMultiplier = 0.5f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volume * volumeMultiplier);
    }

    private void AdjustVolume()
    {
        volume = PlayerPrefs.GetInt(PlayerPrefsKeyword.SFX_VOLUME, 1);

    }
    /// <summary> Increment SFX volume level by 1 </summary>
    public void ChangeVolume()
    {
        volume = volume + 1;

        if (volume > 10)
        {
            volume = 0;
        }

        // Save the value to PlayerPrefs
        PlayerPrefs.SetInt(PlayerPrefsKeyword.SFX_VOLUME, volume);
    }
}
