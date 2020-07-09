// System libraries
using System.Collections;

// Unity libraries
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

// GameManager equivalent for the tutorial
public class TutorialManager : MonoBehaviour
{
    // Pseudo-singleton, see GameManager for details
    // Again, this !! DOES NOT !! persist across scenes
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
    public string[] prompts; // Array of tutorial messages
    public static Question[] allTrialQuestions; // Array of selected questions

    // Question state information
    int givenQuestions; // Number of trials to be given
    public int globalIndex; // Index for current question

    // Sentinels
    bool isAnswered; // Disables the plus button until a question is answerewd
    bool isHeld; // isHeld and holdTime are used for the hold prompt on the first text message
    float holdTime;

    // The plus button
    [SerializeField]
    GameObject plusButton;

    // Return button at the end of the tutorial
    [SerializeField]
    GameObject backButton;

    // transition time between prompts
    [SerializeField]
    private float questionTransitionTime;

    // Text boxes for arrows/prompts
    public GameObject introText;
    public GameObject arrows;

    // Left and right hand buttons
    GameObject[] buttons;

    // Set up starting game state
    private void Start()
    {
        buttons = GameObject.FindGameObjectsWithTag("button");
        globalIndex = 0;

        // Get the player level from the previous scene; if zero, start endless mode
        givenQuestions = 5;

        // Initialize hold time to zero for first prompt
        holdTime = 0.0f;

        // Set game state to initial values
        isAnswered = true;

        // Get left/right buttons and turn them off
        foreach (GameObject obj in buttons)
        {
            obj.SetActive(false);
        }

        // Hide return button for now
        backButton.SetActive(false);

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

        // Display first text prompt
        StartCoroutine(introDisplay());
    }

    // Handles initial tutorial text
    IEnumerator introDisplay()
    {
        introText.GetComponent<TextMeshProUGUI>().text = "Welcome to the Flanker Task!";
        yield return new WaitForSeconds(questionTransitionTime);
        introText.GetComponent<TextMeshProUGUI>().fontSize = 24;
        introText.GetComponent<TextMeshProUGUI>().text = "First we will learn about center. The circle below is your center. Please click and hold, and wait.";
    }

    // Fires when button is held down
    public void holdDown()
    {
        isHeld = true;
    }

    // Fires if button is released too early
    public void holdUp()
    {
        isHeld = false;
    }

    // Keeps track of how long you held button, resets if released too early
    private void Update()
    {
        if (isHeld == true)
        {
            // Increment hold timer while holding down button
            holdTime += Time.deltaTime;

            // Once hold time exceeds given time, trigger next scene
            if(holdTime >= 1.5f)
            {
                // Disable intro text
                introText.SetActive(false);

                // Start tutorial
                isHeld = false;
                startTrial();
            }
        }

        // If not being held, reset hold timer
        else if(holdTime != 0.0f)
        {
            holdTime = 0.0f;
        }
    }

    // Hardcoded questions based on tutorial text
    void LoadTrials()
    {
        allTrialQuestions[0] = questions[1];
        allTrialQuestions[1] = questions[1];
        allTrialQuestions[2] = questions[0];
        allTrialQuestions[3] = questions[2];
        allTrialQuestions[4] = questions[3];
    }

    // Start a trial
    public void startTrial()
    {
        if(globalIndex <= 4)
        {
            plusButton.GetComponent<UnityEngine.UI.Button>().interactable = false;
        }
        else
        {
            plusButton.SetActive(false);
            backButton.SetActive(true);
        }

        // If isAnswered is false, block this entire function; the question has not been answered yet
        if (isAnswered == true)
        {
            arrows.GetComponent<TextMeshProUGUI>().text = "\n\n\n<sprite=\"handsprites\" name=\"plus_symbol\">";

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
        arrows.GetComponent<TextMeshProUGUI>().text = "\n\n\n<sprite=\"handsprites\" name=\"plus_symbol\">";

        // Wait for given time between questions
        yield return new WaitForSeconds(questionTransitionTime);

        // Start timer and display trial
        arrows.GetComponent<TextMeshProUGUI>().text = prompts[globalIndex] + "\n\n";
        if (globalIndex == 1 || globalIndex == 2)
        {
            arrows.GetComponent<TextMeshProUGUI>().text += "\n";
        }
        arrows.GetComponent<TextMeshProUGUI>().text += trial;

        foreach (GameObject obj in buttons)
        {
            obj.SetActive(true);
        }
    }

    // Right button logic
    public void userSelectRight()
    {
        if (!allTrialQuestions[globalIndex].isLeft)
        {
            arrows.GetComponent<TextMeshProUGUI>().text = "\n\n\n<sprite=\"handsprites\" name=\"plus_symbol\">";

            // Get left/right buttons and turn them off
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

        startTrial();
    }

    // At end of game, transition to results screen
    public void moveToResults()
    {
        endTutorial();
        Instance = null;
    }

    public void endTutorial()
    {
        // Set to true since the player has now played the tutorial
        if (tutorialGate.Instance)
        {
            tutorialGate.Instance.setTrue();
        }

        // Set tutorial text to final prompt
        arrows.GetComponent<TextMeshProUGUI>().text = prompts[5];
    }
}
