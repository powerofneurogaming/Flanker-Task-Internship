using UnityEngine;

// Singleton for handling the bulk of game state data (stars, points, results screen data, etc)
// Also contains initialization code for stars and items
public class stateManager : MonoBehaviour
{
    // Singleton
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

    // State values for results screen
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

    // State values for gamemode
    public int gameMode;
    public int difficulty;
    public int levels;

    // ITEMS - UNLOCKABLES
    public bool timeTrial;
    public bool endlessMode;
    public bool nightPurchased;
    public bool handPurchased;

    // ITEMS - CONSUMABLES
    public int longFuse;
    public int stopwatch;
    public int goodGloves;
    public int goodLuckKiss;

    // SETTINGS
    public float music_volume;
    public float sfx_volume;
    public bool nightMode;

    // Hardcoded base volume
    public float volume;

    // Initialize game volume
    public void Start()
    {
        volume = 0.5f;
        music_volume = PlayerPrefs.GetFloat("musicVol", 0.2f);
        Music.Instance.musicSource.volume = music_volume;

        sfx_volume = PlayerPrefs.GetFloat("sfxVol", 1.0f);
        SoundManager.Instance.audioSource.volume = sfx_volume;
    }

    // Load player's items on game start
    public void loadItems()
    {
        timeTrial = PlayerPrefs.GetInt("timeTrial_" + playerName, 0) == 1 ? true : false;
        endlessMode = PlayerPrefs.GetInt("endlessMode_" + playerName, 0) == 1 ? true : false;
        nightPurchased = PlayerPrefs.GetInt("nightPurchased_" + playerName, 0) == 1 ? true : false;

        longFuse = PlayerPrefs.GetInt("longFuse_" + playerName, 0);
        stopwatch = PlayerPrefs.GetInt("stopwatch_" + playerName, 0);
        goodGloves = PlayerPrefs.GetInt("goodGloves_" + playerName, 0);
        goodLuckKiss = PlayerPrefs.GetInt("goodLuckKiss_" + playerName, 0);

        nightMode = PlayerPrefs.GetInt("nightMode_" + playerName, 0) == 1 ? true : false;
    }

    // Load player's star count on game start
    public void loadStars()
    {
        starScore = PlayerPrefs.GetInt("starScore_" + playerName, 0);
    }

    // Star getter
    public int getStars()
    {
        return starScore;
    }

    // Star adder (when player earns stars in-game)
    public void addStars(int stars)
    {
        starScore += stars;
    }

    // Star remover (for buying items)
    public void removeStars(int stars)
    {
        starScore -= stars;
    }

    // Star resetter (for resetting user profile)
    public void resetStars()
    {
        starScore = 0;
    }

    // Star saver (separate from other functions to minimize disk use)
    public void saveStars()
    {
        PlayerPrefs.SetInt("starScore_" + playerName, starScore);
    }
}
