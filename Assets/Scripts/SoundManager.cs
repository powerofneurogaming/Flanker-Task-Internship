// Unity libraries
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

    // Volume setter
    public void setVolume(float volume)
    {
        audioSource.volume = volume;
    }
}
