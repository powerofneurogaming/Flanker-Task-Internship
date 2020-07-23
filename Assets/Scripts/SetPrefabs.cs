// Unity libraries
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Handles initializin game state, as well as setting player name and number of trials
public class SetPrefabs : MonoBehaviour
{
    public AudioClip carriage_return;
    public float volume = 0.5f;

    // Can't figure out where this is hooked up... I try to rename it to 'PlayerInput' and everything breaks.
    // When I need to start adding more complex user menus I need to ask Khalil for help.
    public GameObject playerName;

    public string name;

    // Set fresh game state 
    public void setupPrefabs()
    {
        // Play carriage return sound
        if (!SoundManager.Instance.audioSource.isPlaying)
        {
            SoundManager.Instance.audioSource.PlayOneShot(carriage_return, volume);
        }

        // Initialize player data to default
        PlayerPrefs.SetInt("PlayerLevel", 0);
        PlayerPrefs.SetInt("PlayerScore", 0);
        PlayerPrefs.SetInt("Wrong Answers", 0);
        PlayerPrefs.SetInt("Unanswered Trials", 0);
        PlayerPrefs.SetFloat("avgTime", 0.0f);
        PlayerPrefs.SetFloat("avgCongTime", 0.0f);
        PlayerPrefs.SetFloat("avgIncongTime", 0.0f);

        // Get name from text box
        name = playerName.GetComponent<Text>().text;

        // If name is blank, use default
        if (name.Length <= 0)
        {
            stateManager.Instance.playerName = "NoName";
        }
        else
        {
            stateManager.Instance.playerName = name;
        }

        AchievementManager.Instance.loadAchievements();
        starManager.Instance.loadStars();

        // Check if tutorial has been played before by the given user
        tutorialGate.Instance.getPlayed();

        // Transition to either title screen or tutorial depending on if the tutorial has been played before
        if(tutorialGate.Instance.hasPlayedTutorial == true)
        {
            SceneManager.LoadScene("Title");
        }
        else
        {
            SceneManager.LoadScene("Tutorial");
        }
    }

    // On trial select screen, get number of trials
    // If non-number, set to zero (endless mode)
    public void setLevel()
    {
        // Temporary code for starting Endless Mode from Classic Mode screen
        if (endlessModeToggle.endlessMode == true)
        {
            PlayerPrefs.SetInt("PlayerLevel", 0);
            PlayerPrefs.SetInt("GameMode", 2);
            SceneManager.LoadScene("Flanker Main");
        }
        // Else, regular Classic Mode code
        else
        {
            // Get number of levels from user text input
            string level = playerName.GetComponent<Text>().text;

            // parse user input
            int.TryParse(level, out int level_int);

            // If valid user input and not endless mode, process input
            if (level_int != 0 && endlessModeToggle.endlessMode == false)
            {
                if (level_int > 100)
                {
                    level_int = 100;
                }
                PlayerPrefs.SetInt("PlayerLevel", level_int);
                if (!SoundManager.Instance.audioSource.isPlaying)
                {
                    SoundManager.Instance.audioSource.PlayOneShot(carriage_return, volume);
                }
                PlayerPrefs.SetInt("GameMode", 0);
                SceneManager.LoadScene("Flanker Main");
            }
        }
    }

    // Code for starting Time Trial mode
    public void setTimed()
    {
        PlayerPrefs.SetInt("GameMode", 1);
        if (!SoundManager.Instance.audioSource.isPlaying)
        {
            SoundManager.Instance.audioSource.PlayOneShot(carriage_return, volume);
        }
        SceneManager.LoadScene("Flanker Main");
    }

    // Code for starting Endless mode
    public void setEndless()
    {
        PlayerPrefs.SetInt("GameMode", 2);
        if (!SoundManager.Instance.audioSource.isPlaying)
        {
            SoundManager.Instance.audioSource.PlayOneShot(carriage_return, volume);
        }
        SceneManager.LoadScene("Flanker Main");
    }

    // 
    public void Update()
    {
        if(Input.GetKey(KeyCode.Return))
        {
            if (SceneManager.GetActiveScene().name == "intro")
            {
                setupPrefabs();
            }
            else if(SceneManager.GetActiveScene().name == "Classic Select")
            {
                setLevel();
            }
        }
    }
}
   
