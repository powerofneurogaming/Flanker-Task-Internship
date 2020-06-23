// System libraries
using System.IO;
using System.Linq;

// Unity libraries
using UnityEngine;
using UnityEngine.UI;

// Sets up and manages results screen
public class Score : MonoBehaviour
{
    // Holders for final game state
    int unanswered;
    int wrong;
    int score;
    float avgTime;
    float congTime;
    float incongTime;
    float flankerEffect;

    // Holders for rounded averages / Flanker Effect (for results screen)
    float avgTimeRound;
    float congTimeRound;
    float incongTimeRound;
    float flankerRound;

    // ID for entry in CSV file
    int resultNum;

    // Game elements for results readout and 'View Results' button
    public Text scoreText;
    public GameObject resultsButton;

    // Set up results readout, write to CSV file
    void Start()
    {
        // Disable game results text
        scoreText.enabled = false;

        // Set file path to CSV file
        string filePath = "flanker.csv";

        // Get game state from previous scene
        wrong = PlayerPrefs.GetInt("Wrong Answers");
        unanswered = PlayerPrefs.GetInt("Unanswered Trials");
        score = PlayerPrefs.GetInt("PlayerScore");
        avgTime = PlayerPrefs.GetFloat("avgTime");
        congTime = PlayerPrefs.GetFloat("avgCongTime");
        incongTime = PlayerPrefs.GetFloat("avgIncongTime");

        // Calculate rounded averages for results text
        avgTimeRound = Mathf.Round(avgTime * 1000) / 1000;
        congTimeRound = Mathf.Round(congTime * 1000) / 1000;
        incongTimeRound = Mathf.Round(incongTime * 1000) / 1000;

        // Calculate Flanker Effect
        if (congTime >= incongTimeRound)
        {
            flankerEffect = congTime - incongTime;
        }
        else
        {
            flankerEffect = incongTime - congTime;
        }

        // Round Flanker Effect for results text
        flankerRound = Mathf.Round(flankerEffect * 1000) / 1000;

        // If CSV file does not exist, create it and set up label row
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath,"Test Number,Name,Score,Wrong,Unanswered,Average Time,Average Congruent Time,Average Incongruent Time,Flanker Effect\n");
        }

        // Set current game ID based on number of existing lines in CSV file
        resultNum = File.ReadLines(filePath).Count();

        // Write current game to CSV file
        File.AppendAllText(filePath, resultNum + "," + Congrats_Text.Player + "," + score + "," + wrong + "," + unanswered + "," + avgTime + "," + congTime + "," + incongTime + "," + flankerEffect + "\n");
    }

    // When you click 'View Results', display game results
    public void displayResults()
    {
        scoreText.enabled = true; // Enable game results text
        resultsButton.SetActive(false); // Hide 'View Results' button

        // Populate game results text with information
        scoreText.text = "End Summary:" +
                         "\nScore: " + score +
                         "\nWrong: " + wrong +
                         "\nUnanswered: " + unanswered +
                         "\nAvg Time: " + avgTimeRound +
                         "\nCongruent Avg: " + congTimeRound +
                         "\nIncongruent Avg: " + incongTimeRound +
                         "\nFlanker Effect: " + flankerRound;
    }
}