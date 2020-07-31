using UnityEngine;

// Handles whether to show the tutorial when starting the game for the first time
public class tutorialGate : MonoBehaviour
{
    // Singleton
    public static tutorialGate Instance { get; private set; }

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

    // hasPlayedTutorial sentinel
    public bool hasPlayedTutorial;

    // Setter for true
    public void setTrue()
    {
        PlayerPrefs.SetInt("HasPlayedTutorial_" + stateManager.Instance.playerName, true ? 1 : 0);
        hasPlayedTutorial = true;
    }

    // Setter for false (for resetting player profile)
    public void setFalse()
    {
        PlayerPrefs.SetInt("HasPlayedTutorial_" + stateManager.Instance.playerName, false ? 1 : 0);
        hasPlayedTutorial = false;
    }

    // Getter for whether has played tutorial
    public void getPlayed()
    {
        hasPlayedTutorial = PlayerPrefs.GetInt("HasPlayedTutorial_" + stateManager.Instance.playerName, 0) == 1 ? true : false;
    }
}
