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

    double avgTimeRound;
    double congTimeRound;
    double incongTimeRound;
    double flankerRound;

    public Text scoreText;

    // Start is called before the first frame update
    void Start()
    {
        score = PlayerPrefs.GetInt("PlayerScore");
        avgTime = PlayerPrefs.GetFloat("avgTime");
        congTime = PlayerPrefs.GetFloat("avgCongTime");
        incongTime = PlayerPrefs.GetFloat("avgIncongTime");

        avgTimeRound = System.Math.Round(avgTime, 3);
        congTimeRound = System.Math.Round(congTime, 3);
        incongTimeRound = System.Math.Round(incongTime, 3);

        if (congTime >= incongTimeRound)
        {
            flankerEffect = congTime - incongTime;
        }
        else
        {
            flankerEffect = incongTime - congTime;
        }


        flankerRound = System.Math.Round(flankerEffect, 3);

        scoreText.text = "You Got: " + score +
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