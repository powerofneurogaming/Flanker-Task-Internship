// Unity libraries
using UnityEngine;
using UnityEngine.UI;

// State manager for game timer. Probably the best code I've written since this started.
// I am not proud of that. It means I've written a lot of really questionable code in other places.
public class Timer : MonoBehaviour
{
    // timer and absolute time of correct answers
    static float timer;
    static float time;

    public static float globalTimer;

    // Absolute time of correct congruent and incongruent answers
    static float congruentTime;
    static float incongruentTime;

    // Sentinel for whether timer should be updated
    public static bool timerStart;

    // Debug timer output
    public Text debugTimer;

    // Initialize starting state for timer
    void Start()
    {
        timer = 0.0f;
        time = 0.0f;
        congruentTime = 0.0f;
        incongruentTime = 0.0f;
        timerStart = false;
    }

    // Update timer every frame unless timer is disabled
    void Update()
    {
        if (timerStart == true)
        {
            timer += Time.deltaTime;
            debugTimer.text = "Debug Timer: " + (Mathf.Round(timer * 1000) / 1000).ToString();
            Debug.Log("Time: " + timer);
        }

        if (globalTimer >= 0.0f)
        {
            globalTimer -= Time.deltaTime;
        }
        else if (globalTimer < 0.0f)
        {
            globalTimer = 0.0f;
        }

    }

    // Reset timer; record time given correct answer
    public static void resetTimer(bool record)
    {
        if (record == true)
        {
            time += timer;
            if (GameManager.allTrialQuestions[GameManager.Instance.globalIndex].isCongruent == true)
            {
                congruentTime += timer;
                GameManager.Instance.congruentQuestions++;
            }
            else
            {
                incongruentTime += timer;
                GameManager.Instance.incongruentQuestions++;
            }
        }
        timer = 0.0f;
    }

    // Getter for timer
    public static float getTimer()
    {
        return timer;
    }

    // Getter for absolute time
    public static float getTime()
    {
        return time;
    }

    // Getter for absolute congruent time
    public static float getCongTime()
    {
        return congruentTime;
    }

    // Getter for absolute incongruent time
    public static float getIncongTime()
    {
        return incongruentTime;
    }
}
