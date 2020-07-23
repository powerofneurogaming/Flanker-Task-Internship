using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    // Singleton
    public static Music Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public AudioSource musicSource;

    private void Start()
    {
        musicSource.volume = 0.5f;
    }

    public void setVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public bool mute()
    {
        musicSource.mute = !musicSource.mute;
        return musicSource.mute;
    }
}
