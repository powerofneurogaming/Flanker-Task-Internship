// System libraries
using System.Collections;

// Unity libraries
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Manages game state. A big monolithic block of code that only causes me immense suffering.
// Most likely needs to be broken up into several different scripts, because any time I try
// to hook a game UI element up to it instead of hooking it up to a separate script, the game
// explodes and fails spectacularly. This file is the bane of my existence.
//
// TODO: Break Left and Right button logic out into a separate script so I can get rid of the
//       plus button and stop needing to update the scoreboard every frame like some sort of
//       barbarian.
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
    private float maxTime;
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
        maxTime = 10f;

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
        // If isAnswered is false, block this entire function; the question has not been answered yet
        if (isAnswered == true)
        {
            arrows.GetComponent<Text>().text = "+";
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
        // disable timer
        Timer.timerStart = false;

        // Get left/right buttons and turn them off
        GameObject[] buttons = GameObject.FindGameObjectsWithTag("button");
        foreach (GameObject obj in buttons)
        {
            obj.SetActive(false);
        }

        // If right is correct, trigger correct answer logic, else trigger incorrect question logic
        if (!allTrialQuestions[globalIndex].isLeft)
        {
            userSelectEnd(true, true);
        }
        else
        {
            userSelectEnd(true, false);
        }
    }

    // Left button logic
    public void userSelectLeft()
    {
        // disable timer
        Timer.timerStart = false;

        // Get left/right buttons and turn them off
        GameObject[] buttons = GameObject.FindGameObjectsWithTag("button");
        foreach (GameObject obj in buttons)
        {
            obj.SetActive(false);
        }

        // If left is correct, trigger correct answer logic, else trigger incorrect question logic
        if (allTrialQuestions[globalIndex].isLeft)
        {
            userSelectEnd(true, true);
        }
        else
        {
            userSelectEnd(true, false);
        }
    }

    // No selection logic
    public void userSelectNone()
    {
        // disable timer
        Timer.timerStart = false;

        // Get left/right buttons and turn them off
        GameObject[] buttons = GameObject.FindGameObjectsWithTag("button");
        foreach (GameObject obj in buttons)
        {
            obj.SetActive(false);
        }

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
        // Timer adjust logic: 1.5x correct score average
        maxTime = Timer.getTime() / score * 1.5f;

        // Reset timer, increment score if correct
        if (correct == true)
        {
            score++;
            Timer.resetTimer(true);
            Debug.Log("Correct, Score: " + PlayerPrefs.GetInt("PlayerScore") + ", Time: " + Timer.getTimer());
        }
        else
        {
            Timer.resetTimer(false);
        }

        // Increment values for incorrect answer
        if(answered == true)
        {
            numAnswered++;
            if(correct == false) // DO NOT put this in the above 'else' block or it will trigger when not answering ast all
            {
                if (endlessMode == true)
                {
                    wrongSentinel++;
                }
                numWrong++;
                Debug.Log("Incorrect, Score: " + PlayerPrefs.GetInt("PlayerScore") + ", Time: " + Timer.getTimer());
            }
        }
        else
        {
            numUnanswered++;
        }

        // Advance to the next question
        globalIndex++;

        // Enable clicking of plus button. TODO: REMOVE PLUS BUTTON
        isAnswered = true;

        // If exit condition is detected, transition to results screen
        if ((globalIndex >= givenQuestions && endlessMode == false) || wrongSentinel >= 3)
        {
            moveToResults();
        }
        
        // If in Endless Mode, roll next trial
        if (endlessMode == true)
        {
            resetTrials();
        }
    }

    // At end of game, transition to results screen
    public void moveToResults()
    {
        // calculate average times
        finalTime = Timer.getTime() / score;
        finalCongTime = Timer.getCongTime() / congruentQuestions;
        finalIncongTime = Timer.getIncongTime() / incongruentQuestions;

        // handle NaNs for divide-by-zero
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

        // Save data and transition to results screen
        PlayerPrefs.SetInt("PlayerScore", score);
        PlayerPrefs.SetInt("Wrong Answers", numWrong);
        PlayerPrefs.SetInt("Unanswered Trials", numUnanswered);
        PlayerPrefs.SetFloat("avgTime", finalTime);
        PlayerPrefs.SetFloat("avgCongTime", finalCongTime);
        PlayerPrefs.SetFloat("avgIncongTime", finalIncongTime);
        SceneManager.LoadScene("Flanker Result");
    }

    // Reset game with new trial question for Endless Mode
    public void resetTrials()
    {
        globalIndex = 0; // Reset index to not iterate off end of array
        allTrialQuestions = new Question[givenQuestions]; // Reinitialize trial array
        LoadTrials(); // Assign question(s)
        // Output question(s) to console
        foreach (Question question in allTrialQuestions)
        {
            Debug.Log(question.flankerArrows);
        }
    }
}
