// Unity libraries
using UnityEngine;

// AudioSource script for music
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

    // Volume setter
    public void setVolume(float volume)
    {
        musicSource.volume = volume;
    }
}
