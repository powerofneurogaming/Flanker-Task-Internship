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

    [SerializeField]
    public AudioClip twelveSt;

    [SerializeField]
    public AudioClip mapleLeaf;

    [SerializeField]
    public AudioClip stormyWeather;

    [SerializeField]
    public AudioClip entertainer;

    // Volume setter
    public void setVolume(float volume)
    {
        musicSource.volume = volume;
    }
}
