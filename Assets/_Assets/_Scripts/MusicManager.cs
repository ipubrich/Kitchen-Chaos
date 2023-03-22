using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private const string PLAYER_PREFS_MUSIC_VOLUME = "MusicVolume";
    // in order to call this from the Options UI we need a singleton
    public static MusicManager Instance { get; private set; }

    private float volume = .3f;
    private AudioSource audioSource;

    private void Awake()
    {
        volume = PlayerPrefs.GetFloat(PLAYER_PREFS_MUSIC_VOLUME, .3f);
        Instance = this;
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = volume;
    }
    public void ChangeVolume()
    {
        volume += .1f;
        //volume = volume % 1.1f;
        if (volume > 1.1f)
        {
            volume = 0f;
        }
        audioSource.volume = volume;

        PlayerPrefs.SetFloat(PLAYER_PREFS_MUSIC_VOLUME, volume);
        PlayerPrefs.Save();
    }
    public float GetVolume()
    {
        return volume;
    }
}
