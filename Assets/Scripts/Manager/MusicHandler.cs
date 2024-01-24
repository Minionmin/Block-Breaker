using UnityEngine;

public class MusicHandler : MonoBehaviour
{
    public static MusicHandler Instance { get; private set; }

    [SerializeField] private AudioSource audioSource;

    /// <summary> Music volume level </summary>
    public int volume = 1;

    private void Awake()
    {
        Instance = this;
        audioSource.Play();
    }

    private void Start()
    {
        // audioSource volume range is 0.0 - 1.0
        // Get player's default setting
        AdjustVolume();
    }

    private void AdjustVolume()
    {
        volume = PlayerPrefs.GetInt(PlayerPrefsKeyword.MUSIC_VOLUME, 1);
        audioSource.volume = (float)volume / 10;
    }

    /// <summary> Increment music volume level by 1 </summary>
    public void ChangeVolume()
    {
        volume = volume + 1;

        if(volume > 10)
        {
            volume = 0;
        }

        // Save the value to PlayerPrefs
        PlayerPrefs.SetInt(PlayerPrefsKeyword.MUSIC_VOLUME, volume);

        // Adjust volume after setting changed
        AdjustVolume();
    }
}
