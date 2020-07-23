using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stateManager : MonoBehaviour
{
    public static stateManager Instance { get; private set; }

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

    public float music_volume;
    public float sfx_volume;
    public int difficulty;
    public string playerName;

    public void Start()
    {
        music_volume = PlayerPrefs.GetFloat("musicVol", 0.5f);
        Music.Instance.musicSource.volume = music_volume;

        sfx_volume = PlayerPrefs.GetFloat("sfxVol", 0.5f);
        SoundManager.Instance.audioSource.volume = sfx_volume;
    }
}
