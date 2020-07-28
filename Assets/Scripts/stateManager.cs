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

    public string playerName;

    int starScore;

    public int wrong;
    public int unanswered;
    public int score;
    public float avgTime;
    public float bestTime;
    public float worstTime;
    public float bestCongTime;
    public float worstCongTime;
    public float bestIncongTime;
    public float worstIncongTime;
    public float congTimeAvg;
    public float incongTimeAvg;

    public int gameMode;
    public int difficulty;
    public int levels;

    // ITEMS
    public bool timeTrial;
    public bool endlessMode;
    public bool nightItem;
    public bool handdPurchased;
    public bool[] characters;
    public int longFuse;
    public int stopwatch;
    public int goodGloves;
    public int goodLuckKiss;

    // SETTINGS
    public float music_volume;
    public float sfx_volume;
    public bool nightMode;
    public bool oldHand;

    public void Start()
    {
        music_volume = PlayerPrefs.GetFloat("musicVol", 0.5f);
        Music.Instance.musicSource.volume = music_volume;

        sfx_volume = PlayerPrefs.GetFloat("sfxVol", 0.5f);
        SoundManager.Instance.audioSource.volume = sfx_volume;
    }

    public void loadItems()
    {
        timeTrial = PlayerPrefs.GetInt("timeTrial_" + playerName, 0) == 1 ? true : false;
        endlessMode = PlayerPrefs.GetInt("endlessMode_" + playerName, 0) == 1 ? true : false;
        timeTrial = PlayerPrefs.GetInt("timeTrial_" + playerName, 0) == 1 ? true : false;
    }

    public void loadStars()
    {
        starScore = PlayerPrefs.GetInt("starScore_" + playerName, 0);
    }

    public int getStars()
    {
        return starScore;
    }

    public void addStars(int stars)
    {
        starScore += stars;
    }

    public void saveStars()
    {
        PlayerPrefs.SetInt("starScore_" + playerName, starScore);
    }
}
