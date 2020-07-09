// System libraries
using System.Collections;

// Unity libraries
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

// Monolithic code block for anaging game state
public class GameManager : MonoBehaviour
{
    // 'Pseudo-singleton' - uses static instance of GameManager to allow for access to GameManager object
    // !! IS NOT PERSISTENT ACROSS MULTIPLE SCENES !!
    public static GameManager Instance { get; private set; }

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
    public static Question[] allTrialQuestions; // Array of randomly selected questions

    // Variables used to initialize given questions
    int givenQuestions; // Number of trials to be given (locked to '1' for Endless Mode)
    int gameMode; // Game mode indicator
    int difficulty; // Difficulty indicator
    private Question previousQuestion; // Prevents duplicate questions
    private Question currentQuestion; // Prevents duplicate questions
    private int randQuestionIndex; // Used for choosing which of four questions

    // State variables for game
    public int globalIndex; // Index for question currently being given
    public int score;
    public int numAnswered;
    int wrongSentinel; // Wrong answers including missed questions (for Endless Mode)
    int numWrong; // Wrong answers excluding missed questions (for results / data collection)
    int numUnanswered;
    private float maxTime;
    float multiplier;
    float currentTimer;

    // Number of congruent vs incongruent questions answered, for calculating final time states
    public int congruentQuestions;
    public int incongruentQuestions;

    // Final time states for results screen
    float bestTime;
    float finalTime;
    float finalCongTime;
    float finalIncongTime;

    // Left/Right hand buttons
    GameObject[] buttons;

    // Gamemode flags (if neither is set, game is in 'Classic' mode
    public bool endlessMode;
    public bool timeTrial;

    // transition time between questions
    [SerializeField]
    private float questionTransitionTime;

    // Text box for arrows / hand sprites
    public GameObject arrows;

    // Set up starting game state
    private void Start()
    {
        // Only the left/right hands are tagged "button", so this assigns the left and right hands to the buttons array
        buttons = GameObject.FindGameObjectsWithTag("button");

        // Need to get these early to determine game mode config
        gameMode = PlayerPrefs.GetInt("GameMode");
        difficulty = PlayerPrefs.GetInt("Difficulty");

        // Set up game state based on chosen mode and difficulty
        if (gameMode == 0) // if classic mode
        {
            givenQuestions = PlayerPrefs.GetInt("PlayerLevel");
        }
        else if (gameMode == 1) // if time trial mode
        {
            if (difficulty == 0)
            {
                givenQuestions = 10;
                Timer.globalTimer = 20.0f;
            }
            else if (difficulty == 1)
            {
                givenQuestions = 20;
                Timer.globalTimer = 40.0f;
            }
            else
            {
                givenQuestions = 30;
                Timer.globalTimer = 60.0f;
            }
            timeTrial = true;
        }
        else // if endless mode
        {
            givenQuestions = 1;
            endlessMode = true;
        }

        // Set up time multipliers for classic and endless modes
        if(timeTrial != true)
        {
            if (difficulty == 0)
            {
                multiplier = 2.5f;
            }
            else if (difficulty == 1)
            {
                multiplier = 2.0f;
            }
            else
            {
                multiplier = 1.5f;
            }
        }

        // Initialize array of trial questions based on the number of questions desired
        allTrialQuestions = new Question[givenQuestions];

        // Set game state to initial values
        globalIndex = 0;
        bestTime = 0.0f;
        score = 0;
        wrongSentinel = 0;
        Timer.timerStart = false;
        congruentQuestions = 0;
        incongruentQuestions = 0;

        // Initialize max time to initial values
        if(timeTrial == false) // If not time trial mode, give a large block of time to establish an average
        {
            maxTime = 5f;
        }
        else // if time trial mode, all questions are given two seconds
        {
            maxTime = 2f;
        }

        // Turn off left/right buttons
        foreach (GameObject obj in buttons)
        {
            obj.SetActive(false);
        }

        // Turn off arrow textbox
        arrows.SetActive(false);

        // Set up questions
        LoadTrials();

        // Output questions to console
        foreach (Question question in allTrialQuestions)
        {
            Debug.Log("Congruency: " + question.isCongruent.ToString() +", Leftness: " + question.isLeft.ToString());
        }
        Debug.Log("Difficulty: " + difficulty);
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
        arrows.GetComponent<TextMeshProUGUI>().text = "<sprite=\"handsprites\" name=\"plus_symbol\">";
        // Turn the arrows on
        arrows.SetActive(true);

        // Set current trial
        Question trial = allTrialQuestions[globalIndex];
        StartCoroutine(displayTrial(trial.flankerArrows));
    }

    // Display newly set current trial
    IEnumerator displayTrial(string trial)
    {
        // Wait for given time between questions
        yield return new WaitForSeconds(questionTransitionTime);

        // Start timer and display trial
        foreach (GameObject obj in buttons)
        {
            obj.SetActive(true);
        }

        Timer.timerStart = true;
        arrows.GetComponent<TextMeshProUGUI>().text = trial;
    }

    // Right button logic
    public void userSelectRight()
    {
        // disable timer
        Timer.timerStart = false;

        // Set Arrows textbox to the plus symbol sprite
        arrows.GetComponent<TextMeshProUGUI>().text = "<sprite=\"handsprites\" name=\"plus_symbol\">";

        // Get left/right buttons and turn them off
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

        arrows.GetComponent<TextMeshProUGUI>().text = "<sprite=\"handsprites\" name=\"plus_symbol\">";

        // Get left/right buttons and turn them off
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
        // disable per-question timer
        Timer.timerStart = false;

        // Get left/right buttons and turn them off
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
        // Timer adjust logic: scaling multiplier x correct score average
        if (timeTrial == false) // Time mujltipliers do not apply in time trial mode
        {
            if (score != 0) // Only set a maxTime if there are correct answers to use to establish an average
            {
                float avg = Timer.getTime() / score; // Get average correct time

                // currMultiplier is the multiplier amount over 1x; this is needed to calculate steps from
                // (difficulty)x to 1x (versus steps from (difficulty)x to 0x)  
                float currMultiplier = multiplier - 1.0f;

                // If endless mode is off, the timer scales from (difficulty)x to 1x over the length of the game
                // Steps for multiplier decrease are based on number of questions
                //
                // Note I am adding +1 to the multiplier, since I removed it above
                if (endlessMode == false)
                {
                    currMultiplier = (currMultiplier / givenQuestions * (givenQuestions - globalIndex)) + 1;
                }

                // If endless mdoe is on, the timer scales from (difficulty)x to 1.1x over 25 questions
                // The timer should LOCK at 1.1x average; 1.1x is chosen to give the player a fair shot at the
                // minimum timer multiplier
                //
                // Note I am adding +1 to the multiplier, since I removed it above
                else
                {
                    currMultiplier = (currMultiplier / 25 * (25 - (numAnswered + numUnanswered))) + 1;
                }

                // If in endless mode and the multiplier somehow ends up under 1.1x, reset to 1.1x
                if (endlessMode == true && currMultiplier < 1.1f)
                {
                    currMultiplier = 1.1f;
                }

                // Set time, output to console
                maxTime = avg * currMultiplier;
                Debug.Log("Current time multiplier: " + currMultiplier + ", max time: " + maxTime);
            }
        }

        // Reset timer, increment score if correct
        if (correct == true)
        {
            score++;
            if(bestTime == 0.0f || Timer.getTimer() < bestTime)
            {
                bestTime = Timer.getTimer();
            }
            Timer.resetTimer(true);
            Debug.Log("Correct, Score: " + PlayerPrefs.GetInt("PlayerScore") + ", Time: " + Timer.getTimer());
        }
        else
        {
            if (timeTrial == true)
            {
                Timer.globalTimer--;
            }
            Timer.resetTimer(false);
        }

        // Increment values for incorrect/missed question
        if(answered == true)
        {
            numAnswered++;
            if(correct == false) // DO NOT put this in the above 'else' block or it will trigger when not answering at all
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

        // If exit condition is detected, transition to results screen
        if ((globalIndex >= givenQuestions && endlessMode == false) || wrongSentinel >= 3 || (timeTrial == true && Timer.globalTimer <= 0.0f))
        {
            moveToResults();
        }
        
        // If in Endless Mode, roll next trial
        if (endlessMode == true)
        {
            resetTrials();
        }
        
        // Transition automatically without needing plus button
        startTrial();
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
        PlayerPrefs.SetFloat("bestTime", bestTime);
        PlayerPrefs.SetFloat("avgCongTime", finalCongTime);
        PlayerPrefs.SetFloat("avgIncongTime", finalIncongTime);
        Instance = null;
        Music.Instance.musicSource.Pause();
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
