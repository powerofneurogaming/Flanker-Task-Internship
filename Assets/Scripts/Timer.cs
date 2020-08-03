// Unity libraries
using UnityEngine;
using UnityEngine.UI;

// State manager for game timer. Probably the best code I've written since this started.
// I am not proud of that. It means I've written a lot of really questionable code in other places.
public class Timer : MonoBehaviour
{
    // 'Pseudo-singleton' - uses static instance of GameManager to allow for access to GameManager object
    // !! IS NOT PERSISTENT ACROSS MULTIPLE SCENES !!
    public static Timer Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // timer and absolute time of correct answers
    float timer;
    float time;
    float totalTime;

    // Timer for Time Trial
    public float globalTimer;

    // Absolute time of correct congruent and incongruent answers
    float congruentTime;
    float incongruentTime;

    // Sentinel for whether timer should be updated
    public bool timerStart;

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
            Debug.Log("Time: " + timer);

            // If there is time remaining, decrements timer
            if (globalTimer >= 0.0f)
            {
                globalTimer -= Time.deltaTime;
                if (globalTimer >= 0.0f)
                {
                    Debug.Log("Global time: " + globalTimer);
                }
            }
        }
        // Else if timer is negative, sets to 0
        if (globalTimer < 0.0f)
        {
            globalTimer = 0.0f;
            Debug.Log("Global time: " + globalTimer);
        }
    }

    // Reset timer; record time given correct answer
    public void resetTimer(bool record)
    {
        totalTime += timer;
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
    public float getTimer()
    {
        return timer;
    }

    // Getter for absolute time
    public float getTime()
    {
        return time;
    }

    // Getter for absolute time
    public float getTotalTime()
    {
        return totalTime;
    }

    // Getter for absolute congruent time
    public float getCongTime()
    {
        return congruentTime;
    }

    // Getter for absolute incongruent time
    public  float getIncongTime()
    {
        return incongruentTime;
    }
}
