using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// AudioSource script for sound effects
public class SoundManager : MonoBehaviour
{
    // Singleton
    public static SoundManager Instance { get; private set; }

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

    public AudioSource audioSource;

    private void Start()
    {
        audioSource.volume = 0.5f;
    }

    public void setVolume(float volume)
    {
        audioSource.volume = volume;
    }

    public bool mute()
    {
        audioSource.mute = !audioSource.mute;
        return audioSource.mute;
    }
}
