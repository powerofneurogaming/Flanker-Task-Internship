using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    int score;
    float avgTime;
    float congTime;
    float incongTime;
    float flankerEffect;

    float avgTimeRound;
    float congTimeRound;
    float incongTimeRound;
    float flankerRound;

    public Text scoreText;

    // Start is called before the first frame update
    void Start()
    {
        score = PlayerPrefs.GetInt("PlayerScore");
        avgTime = PlayerPrefs.GetFloat("avgTime");
        congTime = PlayerPrefs.GetFloat("avgCongTime");
        incongTime = PlayerPrefs.GetFloat("avgIncongTime");

        avgTimeRound = Mathf.Round(avgTime * 1000) / 1000;
        congTimeRound = Mathf.Round(congTime * 1000) / 1000;
        incongTimeRound = Mathf.Round(incongTime * 1000) / 1000;

        if (congTime >= incongTimeRound)
        {
            flankerEffect = congTime - incongTime;
        }
        else
        {
            flankerEffect = incongTime - congTime;
        }


        flankerRound = Mathf.Round(flankerEffect * 1000) / 1000;

        scoreText.text = "End Summary:" +
                         "\nScore: " + score +
                         "\nAvg Time: " + avgTimeRound +
                         "\nCongruent Avg: " + congTimeRound +
                         "\nIncongruent Avg: " + incongTimeRound + 
                         "\nFlanker Effect: " + flankerRound;
    }

    // Update is called once per frame
    void Update()
    {
    }
}