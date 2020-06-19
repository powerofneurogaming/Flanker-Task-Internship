using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    static float timer;
    static float time;

    static float congruentTime;
    static float incongruentTime;

    public static bool timerStart;

    public Text debugTimer;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0.0f;
        time = 0.0f;

        congruentTime = 0.0f;

        incongruentTime = 0.0f;

        timerStart = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (timerStart == true)
        {
            timer += Time.deltaTime;
            debugTimer.text = "Debug Timer: " + (Mathf.Round(timer * 1000) / 1000).ToString();
            Debug.Log("Time: " + timer);
        }
    }

    public static void resetTimer(bool record)
    {
        if (record == true)
        {
            time += timer;
            if (GameManager.allTrialQuestions[GameManager.globalIndex].isCongruent == true)
            {
                congruentTime += timer;
                GameManager.congruentQuestions++;
            }
            else
            {
                incongruentTime += timer;
                GameManager.incongruentQuestions++;
            }
        }
        timer = 0.0f;
    }

    public static float getTimer()
    {
        return timer;
    }

    public static float getTime()
    {
        return time;
    }

    public static float getCongTime()
    {
        return congruentTime;
    }

    public static float getIncongTime()
    {
        return incongruentTime;
    }
}
