// System libraries
using System.Collections;

// Unity libraries
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Question[] questions; // Array of possible questions
    public static Question[] allTrialQuestions; // Array of randomly selected questions

    // Variables used to initialize given questions
    int givenQuestions; // Number of trials to be given, or 0 to trigger endless mode (10 question loop/re-init)
    private Question previousQuestion;
    private Question currentQuestion;
    private int randQuestionIndex;

    // State variables for game
    public static int globalIndex; // Index for current question
    public static int score;
    public int numAnswered;
    int wrongSentinel;
    int numWrong;
    int numUnanswered;
    private int maxTime;
    float currentTimer;

    // Number of congruent vs incongruent questions answered, for calculating final time states
    public static int congruentQuestions;
    public static int incongruentQuestions;

    // Final time states for results screen
    float finalTime;
    float finalCongTime;
    float finalIncongTime;

    // Sentinels
    bool isAnswered; // Disables the plus button until a question is answerewd
    public static bool endlessMode; // Enables endless mode if givenQuestions is initially zero

    // transition time between questions
    [SerializeField]
    private float questionTransitionTime = 0.5f;

    // Text box for arrows - !!REMOVE AND REPLACE WITH BASIC ARROW ASSETS!!
    public GameObject arrows;

    // Set up starting game state
    private void Start()
    {
        // Get the player level from the previous scene; if zero, start endless mode
        givenQuestions = PlayerPrefs.GetInt("PlayerLevel");
        if(givenQuestions == 0)
        {
            givenQuestions = 1;
            endlessMode = true;
        }
        else
        {
            endlessMode = false;
        }

        // Set game state to initial values
        score = 0;
        wrongSentinel = 0;
        isAnswered = true;
        Timer.timerStart = false;
        congruentQuestions = 0;
        incongruentQuestions = 0;

        // Initialize max time to (number of questions) seconds
        maxTime = givenQuestions;

        // Get left/right buttons and turn them off
        GameObject[] buttons = GameObject.FindGameObjectsWithTag("button");
        foreach (GameObject obj in buttons)
        {
            obj.SetActive(false);
        }

        // Turn off arrows
        arrows.SetActive(false);

        // Initialize array of trial questions based on the number of questions desired
        allTrialQuestions = new Question[givenQuestions];

        // Set up questions
        LoadTrials();

        // Output questions to console
        foreach (Question question in allTrialQuestions)
        {
            Debug.Log(question.flankerArrows);
        }
    }

    // Update timer every frame that a question is active; if over time, trigger 'none' selection
    private void Update()
    {
        currentTimer = Timer.getTimer();
        if (currentTimer >= maxTime)
        {
            userSelectNone();
        }
    }

    // Initialize questions in question array
    void LoadTrials()
    {
        // For every question in the array of trials
        for (int i = 0; i < allTrialQuestions.Length; i++)
        {
            // Choose a random question type and assign it to the current trial; ensure no two questions in a row are the same
            do
            {
                randQuestionIndex = Random.Range(0, questions.Length);
                currentQuestion = questions[randQuestionIndex];
            } while (previousQuestion != null && previousQuestion.flankerArrows == currentQuestion.flankerArrows);
            previousQuestion = currentQuestion;
            allTrialQuestions[i] = currentQuestion;
        }
    }

    // Start a trial
    public void startTrial()
    {
        arrows.GetComponent<Text>().text = "+";

        // If isAnswered is false, block this entire function; the question has not been answered yet
        if (isAnswered == true)
        {
            // Turn the arrows on
            arrows.SetActive(true);

            // Set current trial
            Question trial = allTrialQuestions[globalIndex];
            StartCoroutine(displayTrial(trial.flankerArrows));

            // Reset is answered sentinel;
            isAnswered = false;
        }
    }

    // Display newly set current trial
    IEnumerator displayTrial(string trial)
    {
        // Display '+'
        arrows.GetComponent<Text>().text = "+";

        // Wait for given time between questions
        yield return new WaitForSeconds(questionTransitionTime);

        // Start timer and display trial
        Timer.timerStart = true;
        arrows.GetComponent<Text>().text = trial;
    }

    // Right button logic
    public void userSelectRight()
    {
        // Get left/right buttons and turn them off
        GameObject[] buttons = GameObject.FindGameObjectsWithTag("button");
        foreach (GameObject obj in buttons)
        {
            obj.SetActive(false);
        }

        // disable timer
        Timer.timerStart = false;

        // If right is correct, trigger correct answer logic, else increment wrong answer counter
        if (!allTrialQuestions[globalIndex].isLeft)
        {
            answerCorrect();
        }
        else
        {
            answerIncorrect();
        }
    }

    // Left button logic
    public void userSelectLeft()
    {
        // Get left/right buttons and turn them off
        GameObject[] buttons = GameObject.FindGameObjectsWithTag("button");
        foreach (GameObject obj in buttons)
        {
            obj.SetActive(false);
        }

        // disable timer
        Timer.timerStart = false;

        // If left is correct, trigger correct answer logic, else increment wrong answer counter
        if (allTrialQuestions[globalIndex].isLeft)
        {
            answerCorrect();
        }
        else
        {
            answerIncorrect();
        }
    }

    // No selection logic
    public void userSelectNone()
    {
        // Get left/right buttons and turn them off
        GameObject[] buttons = GameObject.FindGameObjectsWithTag("button");
        foreach (GameObject obj in buttons)
        {
            obj.SetActive(false);
        }
        // disable timer
        Timer.timerStart = false;

        // If Endless mode is on, increment wrong answer sentinel
        if (endlessMode == true)
        {
            wrongSentinel++;
        }

        // Start general end-state logic
        userSelectEnd(false, false);
    }

    // General end-state logic
    public void userSelectEnd(bool answered, bool correct)
    {
        // Reset timer and increment values given correct/incorrect input
        if(correct == true)
        {
            Timer.resetTimer(true);
        }
        else
        {
            Timer.resetTimer(false);
        }
        if(answered == true)
        {
            numAnswered++;
            if(correct == false)
            {
                numWrong++;
            }
        }
        else
        {
            numUnanswered++;
        }

        // 
        if (maxTime >= 2)
        {
            maxTime--;
        }

        globalIndex++;

        isAnswered = true;

        if (globalIndex >= givenQuestions || wrongSentinel >= 3)
        {
            moveToResults();
        }
    }

    public void answerCorrect()
    {
        score++;
        Debug.Log("Correct, Score: " + PlayerPrefs.GetInt("PlayerScore") + ", Time: " + Timer.getTimer());
        userSelectEnd(true, true);
    }

    public void answerIncorrect()
    {
        if (endlessMode == true)
        {
            wrongSentinel++;
        }
        Debug.Log("Incorrect, Score: " + PlayerPrefs.GetInt("PlayerScore") + ", Time: " + Timer.getTimer());
        userSelectEnd(true, false);
    }

    public void moveToResults()
    {
        if (endlessMode == false || wrongSentinel >= 3)
        {
            finalTime = Timer.getTime() / score;
            finalCongTime = Timer.getCongTime() / congruentQuestions;
            finalIncongTime = Timer.getIncongTime() / incongruentQuestions;

            if (float.IsNaN(finalTime))
            {
                finalTime = 0.0f;
            }
            if (float.IsNaN(finalCongTime))
            {
                finalCongTime = 0.0f;
            }
            if (float.IsNaN(finalIncongTime))
            {
                finalIncongTime = 0.0f;
            }

            PlayerPrefs.SetInt("Unanswered Trials", numUnanswered);
            PlayerPrefs.SetInt("Wrong Answers", numWrong);
            PlayerPrefs.SetFloat("avgTime", finalTime);
            PlayerPrefs.SetFloat("avgCongTime", finalCongTime);
            PlayerPrefs.SetFloat("avgIncongTime", finalIncongTime);
            PlayerPrefs.SetInt("PlayerScore", score);
            SceneManager.LoadScene("Flanker Result");
        }
        else
        {
            globalIndex = 0;
            allTrialQuestions = new Question[givenQuestions];
            LoadTrials();
            foreach (Question question in allTrialQuestions)
            {
                Debug.Log(question.flankerArrows);
            }
        }
    }
}
