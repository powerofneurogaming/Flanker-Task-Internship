// System libraries
using System.Collections;

// Unity libraries
using UnityEngine;
using TMPro;

// GameManager equivalent for the tutorial
public class TutorialManager : MonoBehaviour
{
    public Question[] questions; // Array of possible questions
    public string[] prompts; // Array of tutorial messages
    public static Question[] allTrialQuestions; // Array of selected questions

    // Audio source and tutorial sound effects
    AudioSource sfxSource;
    public AudioClip whistleUp;
    public AudioClip whistleDown;

    // Question state information
    int givenQuestions; // Number of trials to be given
    public int globalIndex; // Index for current question

    // Sentinels
    bool isHeld; // isHeld and holdTime are used for the hold prompt on the first text message
    float holdTime;

    // Sentinel to avoid constant repeated button-presses
    bool handClicked;

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

    [SerializeField]
    GameObject persistBackButton;

    // Set up starting game state
    private void Start()
    {
        // Connect audio source to SFX manager
        sfxSource = SoundManager.Instance.audioSource;

        // Enable exit button if player has played tutorial before
        if (tutorialGate.Instance.hasPlayedTutorial == true)
        {
            persistBackButton.SetActive(true);
        }

        // Get hand buttons
        buttons = GameObject.FindGameObjectsWithTag("button");

        // Start from first prompt
        globalIndex = 0;

        // Get the player level from the previous scene; if zero, start endless mode
        givenQuestions = 5;

        // Initialize hold time to zero for first prompt
        holdTime = 0.0f;
        handClicked = false;

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
                sfxSource.PlayOneShot(whistleUp, stateManager.Instance.volume);
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
        // Make sure button is un-interactable for most of tutorial, then disable it and enable the
        // return button at the end of the tutorial
        if(globalIndex == 0)
        {
            plusButton.GetComponent<UnityEngine.UI.Button>().interactable = false;
        }

        // Show plus symbol to recenter player's gaze
        arrows.GetComponent<TextMeshProUGUI>().text = "\n\n\n<sprite=\"handsprites\" name=\"plus_symbol\">";

        // Set current trial
        Question trial = allTrialQuestions[globalIndex];
        StartCoroutine(displayTrial(trial.flankerArrows));
    }

    // Display newly set current trial
    IEnumerator displayTrial(string trial)
    {
        // Wait for given time between questions
        yield return new WaitForSeconds(questionTransitionTime);

        // Display prompt and trial
        arrows.GetComponent<TextMeshProUGUI>().text = prompts[globalIndex] + "\n\n";
        if (globalIndex == 1 || globalIndex == 2)
        {
            arrows.GetComponent<TextMeshProUGUI>().text += "\n";
        }
        arrows.GetComponent<TextMeshProUGUI>().text += trial;

        // Turn hand buttons back on
        foreach (GameObject obj in buttons)
        {
            obj.SetActive(true);
        }
    }

    // Wrapper for userSelectEnd() to gate behind correct answer
    public void userSelectRight()
    {
        // Only proceed if player has chosen the correct answer
        if (!allTrialQuestions[globalIndex].isLeft)
        {
            sfxSource.PlayOneShot(whistleUp, stateManager.Instance.volume);
            userSelectEnd();
        }
        // Achievement: Click the wrong hand in the tutorial a given number of times
        // Bronze: 1 time
        // Silver: 2 times
        // Gold: 3 times
        else
        {
            if(handClicked == false)
            {
                sfxSource.PlayOneShot(whistleDown, stateManager.Instance.volume);
                AchievementManager.Instance.getAchievement(AchievementManager.Instance.achievementList[8], 1);

                // Achievement: Get all achievements
                // Bronze: All bronze or better
                // Silver: All silver or better
                // Gold: All gold or better
                AchievementManager.Instance.achievementsAchievement();
                StartCoroutine(AchievementPopup.Instance.achPop());
            }
            handClicked = true;
        }
    }

    // Wrapper for userSelectEnd() to gate behind correct answer
    public void userSelectLeft()
    {
        // Only proceed if player has chosen the correct answer
        if (allTrialQuestions[globalIndex].isLeft)
        {
            sfxSource.PlayOneShot(whistleUp, stateManager.Instance.volume);
            userSelectEnd();
        }
        // Achievement: Click the wrong hand in the tutorial a given number of times
        // Bronze: 1 time
        // Silver: 2 times
        // Gold: 3 times
        else
        {
            if (handClicked == false)
            {
                sfxSource.PlayOneShot(whistleDown, stateManager.Instance.volume);
                AchievementManager.Instance.getAchievement(AchievementManager.Instance.achievementList[8], 1);

                // Achievement: Get all achievements
                // Bronze: All bronze or better
                // Silver: All silver or better
                // Gold: All gold or better
                AchievementManager.Instance.achievementsAchievement();
            }
            handClicked = true;
        }
    }

    // General end-state logic
    public void userSelectEnd()
    {
        handClicked = false;

        // Get left/right buttons and turn them off
        foreach (GameObject obj in buttons)
        {
            obj.SetActive(false);
        }

        // Advance to the next question
        globalIndex++;

        // If exit condition is detected, end tutorial
        if (globalIndex >= givenQuestions)
        {
            endTutorial();
        }
        else
        {
            // Proceed to next prompt
            startTrial();
        }
    }

    public void endTutorial()
    {
        // Disable plus button and enable return button
        persistBackButton.SetActive(false);
        plusButton.SetActive(false);
        backButton.SetActive(true);

        // Set tutorial text to final prompt
        arrows.GetComponent<TextMeshProUGUI>().text = prompts[5];

        // Set to true since the player has now played the tutorial
        if (tutorialGate.Instance)
        {
            tutorialGate.Instance.setTrue();
        }
    }
}