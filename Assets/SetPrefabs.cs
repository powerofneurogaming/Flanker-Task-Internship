﻿// Unity libraries
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Handles initializin game state, as well as setting player name and number of trials
public class SetPrefabs : MonoBehaviour
{
    // Can't figure out where this is hooked up... I try to rename it to 'PlayerInput' and everything breaks.
    // When I need to start adding more complex user menus I need to ask Khalil for help.
    public GameObject playerName;

    // Set fresh game state 
    public void setupPrefabs()
    {
        // Initialize player data to default
        PlayerPrefs.SetInt("PlayerLevel", 0);
        PlayerPrefs.SetInt("PlayerScore", 0);
        PlayerPrefs.SetInt("Wrong Answers", 0);
        PlayerPrefs.SetInt("Unanswered Trials", 0);
        PlayerPrefs.SetFloat("avgTime", 0.0f);
        PlayerPrefs.SetFloat("avgCongTime", 0.0f);
        PlayerPrefs.SetFloat("avgIncongTime", 0.0f);

        // Get name from text box
        string name = playerName.GetComponent<Text>().text;

        // If name is blank, use default
        if (name.Length <= 0)
        {
            PlayerPrefs.SetString("PlayerName", "NoName");
        }
        else
        {
            PlayerPrefs.SetString("PlayerName", name);
        }

        // Transition to trial select screen
        SceneManager.LoadScene("Select Screen");
    }

    // On trial select screen, get number of trials
    // If non-number, set to zero (endless mode)
    public void setLevel()
    {
        string level = playerName.GetComponent<Text>().text;
        int.TryParse(level, out int level_int);
        PlayerPrefs.SetInt("PlayerLevel", level_int);
        SceneManager.LoadScene("Flanker Main");
    }
}
   
