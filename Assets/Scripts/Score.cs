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
    float bestTime;
    float congTime;
    float incongTime;
    float flankerEffect;

    // Holders for rounded averages / Flanker Effect (for results screen)
    float avgTimeRound;
    float bestTimeRound;
    float congTimeRound;
    float incongTimeRound;
    float flankerRound;

    // Best time output string, so the game shows if there is no best time
    // (i.e. the player got all trials wrong)
    string bestString;

    // ID for entry in CSV file
    int resultNum;

    // Game elements for results readout and 'View Results' button
    public Text scoreText;
    public GameObject resultsButton;
    public Button restartButton;
    public Button mainMenuButton;

    float allTimeBest;
    float allTimeBestAvg;
    float allTimeBestCong;
    float allTimeBestIncong;

    // Set up results readout, write to CSV file
    void Start()
    {
        allTimeBest = float.NaN;
        allTimeBestAvg = float.NaN;
        allTimeBestCong = float.NaN;
        allTimeBestIncong = float.NaN;

        // Disable game results text
        scoreText.enabled = false;
        restartButton.gameObject.SetActive(false);
        mainMenuButton.gameObject.SetActive(false);

        // Set file path to CSV file
        string filePath = "flanker.csv";

        // Get game state from previous scene
        wrong = PlayerPrefs.GetInt("Wrong Answers");
        unanswered = PlayerPrefs.GetInt("Unanswered Trials");
        score = PlayerPrefs.GetInt("PlayerScore");
        avgTime = PlayerPrefs.GetFloat("avgTime");
        bestTime = PlayerPrefs.GetFloat("bestTime");
        congTime = PlayerPrefs.GetFloat("avgCongTime");
        incongTime = PlayerPrefs.GetFloat("avgIncongTime");

        // Calculate rounded averages for results text
        avgTimeRound = Mathf.Round(avgTime * 1000) / 1000;
        bestTimeRound = Mathf.Round(bestTime * 1000) / 1000;
        congTimeRound = Mathf.Round(congTime * 1000) / 1000;
        incongTimeRound = Mathf.Round(incongTime * 1000) / 1000;

        // If no best time, don't output a best time
        if(float.IsNaN(bestTimeRound))
        {
            bestString = "\nNo best time...";
        }
        // Else, output best time, and if it is an all-time best time, update the all-time best time
        else
        {
            bestString = "\nBest Correct Time: " + bestTimeRound;
            allTimeBest = PlayerPrefs.GetFloat("allBestTime_" + SetPrefabs.name, float.NaN);
            if(bestTimeRound < allTimeBest || float.IsNaN(allTimeBest))
            {
                PlayerPrefs.SetFloat("allBestTime_" + SetPrefabs.name, bestTimeRound);
                allTimeBest = bestTimeRound;
            }
        }

        // Update all-time best average time
        if (!float.IsNaN(avgTimeRound))
        {
            allTimeBestAvg = PlayerPrefs.GetFloat("allBestAvg_" + SetPrefabs.name, float.NaN);
            if (avgTimeRound < allTimeBestAvg || float.IsNaN(allTimeBestAvg))
            {
                PlayerPrefs.SetFloat("allBestAvg_" + SetPrefabs.name, avgTimeRound);
                allTimeBestAvg = avgTimeRound;
            }
        }

        // Update all-time best average congruent time
        if (!float.IsNaN(congTimeRound))
        {
            allTimeBestCong = PlayerPrefs.GetFloat("allBestCongAvg_" + SetPrefabs.name, float.NaN);
            if (congTimeRound < allTimeBestCong || float.IsNaN(allTimeBestCong))
            {
                PlayerPrefs.SetFloat("allBestCongAvg_" + SetPrefabs.name, congTimeRound);
                allTimeBestCong = congTimeRound;
            }
        }

        // Update all-time best average incongruent time
        if (!float.IsNaN(incongTimeRound))
        {
            allTimeBestIncong = PlayerPrefs.GetFloat("allBestIncongAvg_" + SetPrefabs.name, float.NaN);
            if (incongTimeRound < allTimeBestIncong || float.IsNaN(allTimeBestIncong))
            {
                PlayerPrefs.SetFloat("allBestIncongAvg_" + SetPrefabs.name, incongTimeRound);
                allTimeBestIncong = incongTimeRound;
            }
        }

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
            File.WriteAllText(filePath,"testNumber,name,score,wrong,unanswered,averageTime,bestTime,globalBestTime,averageCongruentTime,averageIncongruentTime,flankerEffect,\n");
        }

        // Set current game ID based on number of existing lines in CSV file
        resultNum = File.ReadLines(filePath).Count();

        // Write current game to CSV file
        File.AppendAllText(filePath, resultNum + "," + Congrats_Text.Player + "," + score + "," + wrong + "," + unanswered + "," + avgTimeRound + "," + bestTimeRound + "," + allTimeBest + "," + congTimeRound + "," + incongTimeRound + "," + flankerRound + "\n");
    }

    // When you click 'View Results', display game results
    public void displayResults()
    {
        // UI buttons
        restartButton.gameObject.SetActive(true);
        mainMenuButton.gameObject.SetActive(true);

        scoreText.enabled = true; // Enable game results text
        resultsButton.SetActive(false); // Hide 'View Results' button

        // handle NaNs for divide-by-zero
        if (float.IsNaN(avgTimeRound))
        {
            avgTimeRound = 0.0f;
        }
        if (float.IsNaN(congTimeRound))
        {
            congTimeRound = 0.0f;
        }
        if (float.IsNaN(incongTimeRound))
        {
            incongTimeRound = 0.0f;
        }
        if (float.IsNaN(flankerRound))
        {
            flankerRound = 0.0f;
        }

        // Populate game results text with information
        scoreText.text = "End Summary:" +
                         "\nScore: " + score +
                         "\nWrong: " + wrong +
                         "\nUnanswered: " + unanswered +
                         "\nAvg Time: " + avgTimeRound +
                         bestString + 
                         "\nCongruent Avg: " + congTimeRound +
                         "\nIncongruent Avg: " + incongTimeRound +
                         "\nFlanker Effect: " + flankerRound;
    }
}