// System libraries
using System.Collections;

// Unity libraries
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

// Manages game state. A big monolithic block of code that only causes me immense suffering.
// Most likely needs to be broken up into several different scripts, because any time I try
// to hook a game UI element up to it instead of hooking it up to a separate script, the game
// explodes and fails spectacularly. This file is the bane of my existence.
//
// TODO: Break Left and Right button logic out into a separate script so I can get rid of the
//       plus button and stop needing to update the scoreboard every frame like some sort of
//       barbarian.
public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance { get; private set; }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public Question[] questions; // Array of possible questions
    public string[] prompts;
    public static Question[] allTrialQuestions; // Array of randomly selected questions

    // Variables used to initialize given questions
    int givenQuestions; // Number of trials to be given, or 0 to trigger endless mode (10 question loop/re-init)
    private Question previousQuestion;
    private Question currentQuestion;
    private int randQuestionIndex;

    // State variables for game
    public int globalIndex; // Index for current question

    // Sentinels
    bool isAnswered; // Disables the plus button until a question is answerewd
    bool isHeld;
    float holdTime;

    // transition time between questions
    float questionTransitionTime = 3.0f;

    // Text box for arrows
    public GameObject arrows;
    public GameObject leftHand;
    public GameObject rightHand;
    public GameObject introText;

    // Set up starting game state
    private void Start()
    {
        globalIndex = 0;

        // Get the player level from the previous scene; if zero, start endless mode
        givenQuestions = 5;

        holdTime = 0.0f;

        // Set game state to initial values
        isAnswered = true;

        isHeld = false;

        // Get left/right buttons and turn them off
        GameObject[] buttons = GameObject.FindGameObjectsWithTag("button");
        foreach (GameObject obj in buttons)
        {
            obj.SetActive(false);
        }

        // Turn off arrows
        arrows.GetComponent<TextMeshProUGUI>().text = "";

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

    public void holdDown()
    {
        isHeld = true;
    }

    public void holdUp()
    {
        isHeld = false;
    }

    private void Update()
    {
        if (isHeld == true)
        {
            holdTime += Time.deltaTime;
            if(holdTime >= 1.5f)
            {
                holdTime = 0.0f;
                introText.SetActive(false);
                leftHand.SetActive(true);
                rightHand.SetActive(true);
                startTrial();
            }
        }
        else if(holdTime != 0.0f)
        {
            holdTime = 0.0f;
        }
    }

    // Initialize questions in question array
    void LoadTrials()
    {
        allTrialQuestions[0] = questions[1];
        allTrialQuestions[1] = questions[1];
        allTrialQuestions[2] = questions[0];
        allTrialQuestions[3] = questions[2];
        allTrialQuestions[4] = questions[3];
    }

    // Start a trialf
    public void startTrial()
    {
        // If isAnswered is false, block this entire function; the question has not been answered yet
        if (isAnswered == true)
        {
            arrows.GetComponent<TextMeshProUGUI>().text = "\n\n\n<sprite=\"handsprites\" name=\"plus_symbol\">";

            // Set current trial
            Question trial = allTrialQuestions[globalIndex];
            displayTrial(trial.flankerArrows);

            // Reset is answered sentinel;
            isAnswered = false;
        }
    }

    // Display newly set current trial
    public void displayTrial(string trial)
    {
        // Start timer and display trial
        arrows.GetComponent<TextMeshProUGUI>().text = prompts[globalIndex] + "\n\n";
        if (globalIndex == 1 || globalIndex == 2)
        {
            arrows.GetComponent<TextMeshProUGUI>().text += "\n";
        }
        arrows.GetComponent<TextMeshProUGUI>().text += trial;
    }

    // Right button logic
    public void userSelectRight()
    {
        if (!allTrialQuestions[globalIndex].isLeft)
        {
            arrows.GetComponent<TextMeshProUGUI>().text = "\n\n\n<sprite=\"handsprites\" name=\"plus_symbol\">";

            // Get left/right buttons and turn them off
            GameObject[] buttons = GameObject.FindGameObjectsWithTag("button");
            foreach (GameObject obj in buttons)
            {
                obj.SetActive(false);
            }
            userSelectEnd();
        }
    }

    // Left button logic
    public void userSelectLeft()
    {
        // If left is correct, trigger correct answer logic, else trigger incorrect question logic
        if (allTrialQuestions[globalIndex].isLeft)
        {
            arrows.GetComponent<TextMeshProUGUI>().text = "\n\n\n<sprite=\"handsprites\" name=\"plus_symbol\">";

            // Get left/right buttons and turn them off
            GameObject[] buttons = GameObject.FindGameObjectsWithTag("button");
            foreach (GameObject obj in buttons)
            {
                obj.SetActive(false);
            }
            userSelectEnd();
        }
    }

    // General end-state logic
    public void userSelectEnd()
    {
        // Advance to the next question
        globalIndex++;

        // If exit condition is detected, transition to results screen
        if (globalIndex >= givenQuestions)
        {
            moveToResults();
        }
        else
        {
            // Enable clicking of plus button. TODO: REMOVE PLUS BUTTON
            isAnswered = true;
        }
    }

    // At end of game, transition to results screen
    public void moveToResults()
    {
        endTutorial();
        Instance = null;
    }

    public void endTutorial()
    {
        // Display '+'
        arrows.GetComponent<TextMeshProUGUI>().text = prompts[5];
    }
}
