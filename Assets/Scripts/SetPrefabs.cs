// Unity libraries
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Handles initializin game state, as well as setting player name and number of trials
public class SetPrefabs : MonoBehaviour
{
    // AudioClip for carriage return sound efect
    public AudioClip carriage_return;

    // Text field for Player Name in Intro and number of levels in Classic Select
    public Text playerName;

    // Player name temp holder
    public string pName;

    // Set fresh game state 
    public void setupPrefabs()
    {
        // Get player name from text box
        pName = playerName.text;

        // If player name is blank, use default
        if (pName.Length <= 0)
        {
            return;
        }
        else
        {
            stateManager.Instance.playerName = pName.ToLower();
        }

        // Play carriage return sound
        if (!SoundManager.Instance.audioSource.isPlaying)
        {
            SoundManager.Instance.audioSource.PlayOneShot(carriage_return, stateManager.Instance.volume);
        }

        // Load achievements, stars, and items for user
        AchievementManager.Instance.loadAchievements();
        stateManager.Instance.loadStars();
        stateManager.Instance.loadItems();

        // Check if tutorial has been played before by the given user
        tutorialGate.Instance.getPlayed();

        // Transition to either title screen or tutorial depending on if the tutorial has been played before
        if(tutorialGate.Instance.hasPlayedTutorial == true)
        {
            Debug.Log("Name: " + stateManager.Instance.playerName);
            SceneManager.LoadScene("Title");
        }
        else
        {
            SceneManager.LoadScene("Tutorial");
        }
    }

    // On trial select screen, get number of trials
    public void setLevel()
    {
        // Get number of levels from user text input
        string level = playerName.text;

        // parse user input
        int.TryParse(level, out int level_int);

        // If valid user input, process input and start game
        if (level_int != 0)
        {
            if (level_int > 50)
            {
                level_int = 50;
            }
            stateManager.Instance.levels = level_int;
            if (!SoundManager.Instance.audioSource.isPlaying)
            {
                SoundManager.Instance.audioSource.PlayOneShot(carriage_return, stateManager.Instance.volume);
            }
            stateManager.Instance.gameMode = 0;
            SceneManager.LoadScene("Flanker Main");
        }
    }

    // Code for starting Time Trial mode
    public void setTimed()
    {
        stateManager.Instance.gameMode = 1;
        if (!SoundManager.Instance.audioSource.isPlaying)
        {
            SoundManager.Instance.audioSource.PlayOneShot(carriage_return, stateManager.Instance.volume);
        }
        SceneManager.LoadScene("Flanker Main");
    }

    // Code for starting Endless mode
    public void setEndless()
    {
        stateManager.Instance.gameMode = 2;
        if (!SoundManager.Instance.audioSource.isPlaying)
        {
            SoundManager.Instance.audioSource.PlayOneShot(carriage_return, stateManager.Instance.volume);
        }
        SceneManager.LoadScene("Flanker Main");
    }

    // Enter key catching on scenes with text boxes
    public void Update()
    {
        if(Input.GetKey(KeyCode.Return))
        {
            if (SceneManager.GetActiveScene().name == "intro")
            {
                setupPrefabs();
            }
        }
    }
}