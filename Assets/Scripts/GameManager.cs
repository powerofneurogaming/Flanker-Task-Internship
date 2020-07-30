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

    // Sound sources
    AudioSource musicSource;
    AudioSource sfxSource;

    // Typewriter sound
    public AudioClip bombsound;
    public AudioClip drumroll;

    public Question[] questions; // Array of possible questions
    public static Question[] allTrialQuestions; // Array of randomly selected questions

    // Variables used to initialize given questions
    int givenQuestions; // Number of trials to be given (locked to '1' for Endless Mode)

    private Question previousQuestion; // Prevents duplicate questions
    private Question currentQuestion; // Prevents duplicate questions
    private int randQuestionIndex; // Used for choosing which of four questions

    // State variables for game
    public int globalIndex; // Index for question currently being given
    public int score;
    public int numAnswered;
    int wrongSentinel; // Wrong answers including missed questions (for Endless Mode)
    int maxWrong; // Max wrong answers based on difficulty mode
    int comboCounter; // Combo counter, also used for regenerative mode
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
    float worstTime;
    float bestCongTime;
    float worstCongTime;
    float bestIncongTime;
    float worstIncongTime;
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
    public GameObject intro;
    public GameObject scoreboard;
    public GameObject starboard;
    public GameObject bombSprite;
    public GameObject bombText;
    public GameObject explosion;
    public GameObject ttBombSprite;
    public GameObject ttBombText;
    public GameObject ttExplosion;
    public Sprite bomb3;
    public Sprite bomb2;
    public Sprite bomb1;
    public Sprite bomb0;

    public GameObject comboBox;
    public GameObject comboText;
    public GameObject plusBox;
    public Sprite plusButton;
    public Sprite blankPlus;

    public float volume;

    // Set up starting game state
    private void Start()
    {
        musicSource = Music.Instance.musicSource;
        sfxSource = SoundManager.Instance.audioSource;

        bombSprite.GetComponent<SpriteRenderer>().enabled = false;
        bombText.GetComponent<Text>().enabled = false;
        explosion.GetComponent<SpriteRenderer>().enabled = false;
        ttBombSprite.GetComponent<SpriteRenderer>().enabled = false;
        ttBombText.GetComponent<Text>().enabled = false;
        ttExplosion.GetComponent<SpriteRenderer>().enabled = false;

        // Only the left/right hands are tagged "button", so this assigns the left and right hands to the buttons array
        buttons = GameObject.FindGameObjectsWithTag("button");

        // Set up game state based on chosen mode and difficulty
        if (stateManager.Instance.gameMode == 0) // if classic mode
        {
            scoreboard.GetComponent<Text>().enabled = false;
            givenQuestions = stateManager.Instance.levels;
        }
        else if (stateManager.Instance.gameMode == 1) // if time trial mode
        {
            scoreboard.GetComponent<Text>().enabled = false;
            // Set number of questions and seconds per game based on difficulty 
            if (stateManager.Instance.difficulty == 0)
            {
                if (stateManager.Instance.stopwatch > 0)
                {
                    givenQuestions = 10;
                    Timer.globalTimer = 40.0f;
                }
                else
                {
                    givenQuestions = 10;
                    Timer.globalTimer = 20.0f;
                }
            }
            else if (stateManager.Instance.difficulty == 1)
            {
                if (stateManager.Instance.stopwatch > 0)
                {
                    givenQuestions = 20;
                    Timer.globalTimer = 80.0f;
                }
                else
                {
                    givenQuestions = 20;
                    Timer.globalTimer = 40.0f;
                }
            }
            else
            {
                if (stateManager.Instance.stopwatch > 0)
                {
                    givenQuestions = 30;
                    Timer.globalTimer = 120.0f;
                }
                else
                {
                    givenQuestions = 30;
                    Timer.globalTimer = 60.0f;
                }
            }
            timeTrial = true;
        }
        else // if endless mode
        {
            scoreboard.GetComponent<Text>().enabled = true;
            givenQuestions = 1;
            endlessMode = true;
        }

        // Set up time multipliers for classic mode
        if (timeTrial != true && endlessMode != true)
        {
            if (stateManager.Instance.difficulty == 0)
            {
                multiplier = 2.5f;
            }
            else if (stateManager.Instance.difficulty == 1)
            {
                multiplier = 2.0f;
            }
            else
            {
                multiplier = 1.5f;
            }
        }
        else if (endlessMode == true)
        {
            multiplier = 2.5f;
            if (stateManager.Instance.difficulty != 2)
            {
                maxWrong = 3 + stateManager.Instance.longFuse;
                updateBomb();
            }
            else
            {
                maxWrong = 1 + stateManager.Instance.longFuse;
                updateBomb();
            }
        }

        // Initialize array of trial questions based on the number of questions desired
        allTrialQuestions = new Question[givenQuestions];

        // Set game state to initial values
        globalIndex = 0;
        bestTime = float.NaN;
        worstTime = float.NaN;
        bestCongTime = float.NaN;
        worstCongTime = float.NaN;
        bestIncongTime = float.NaN;
        worstIncongTime = float.NaN;
        score = 0;
        wrongSentinel = 0;
        Timer.timerStart = false;
        congruentQuestions = 0;
        incongruentQuestions = 0;

        // Initialize max time to initial values
        if(timeTrial == false) // If not time trial mode, give a large block of time to establish an average
        {
            maxTime = 10f;
        }
        else // if time trial mode, all questions are given two seconds
        {
            if(stateManager.Instance.stopwatch > 0)
            {
                maxTime = 4f;
            }
            else
            {
                maxTime = 2f;
            }
        }

        // Turn off left/right buttons
        foreach (GameObject obj in buttons)
        {
            obj.SetActive(false);
        }

        // Turn off arrow textbox
        arrows.SetActive(false);

        // Blank Combo Counter
        comboText.GetComponent<Text>().text = "";

        // Set up questions
        LoadTrials();

        // Output questions to console
        foreach (Question question in allTrialQuestions)
        {
            Debug.Log("Congruency: " + question.isCongruent.ToString() +", Leftness: " + question.isLeft.ToString());
        }
        Debug.Log("Difficulty: " + stateManager.Instance.difficulty);
    }

    // Update timer every frame that a question is active; if over time, trigger 'none' selection
    private void Update()
    {
        currentTimer = Timer.getTimer();
        if (currentTimer >= maxTime)
        {
            userSelectNone();
        }

        if (timeTrial == true)
        {
            updateTimebomb();
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
        if (endlessMode == true)
        {
            bombSprite.GetComponent<SpriteRenderer>().enabled = true;
            bombText.GetComponent<Text>().enabled = true;
        }
        else if (timeTrial == true)
        {
            plusBox.SetActive(false);
            ttBombSprite.GetComponent<SpriteRenderer>().enabled = true;
            ttBombText.GetComponent<Text>().enabled = true;
        }

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

        // Start general end-state logic
        userSelectEnd(false, false);
    }

    // General end-state logic
    public void userSelectEnd(bool answered, bool correct)
    {
        // Get left/right buttons and turn them off
        foreach (GameObject obj in buttons)
        {
            obj.SetActive(false);
        }

        // Timer adjust logic: scaling multiplier x correct score average
        if (timeTrial == false) // Time mujltipliers do not apply in time trial mode
        {
            if (score != 0) // Only set a maxTime if there are correct answers to use to establish an average
            {
                float avg = Timer.getTime() / score; // Get average correct time

                // currMultiplier is the multiplier amount over 1.25x; this is needed to calculate steps from
                // (difficulty)x to 1x (versus steps from (difficulty)x to 0x)  
                float currMultiplier = multiplier - 1.25f;

                // If endless mode is off, the timer scales from (difficulty)x to 1x over the length of the game
                // Steps for multiplier decrease are based on number of questions
                //
                // Note I am adding +1.25 to the multiplier, since I removed it above
                if (endlessMode == false)
                {
                    currMultiplier = (currMultiplier / givenQuestions * (givenQuestions - globalIndex)) + 1.25f;
                }

                // If endless mdoe is on, the timer scales from (difficulty)x to 1.1x over 25 questions
                // The timer should LOCK at 1.1x average; 1.1x is chosen to give the player a fair shot at the
                // minimum timer multiplier
                //
                // Note I am adding +1.25 to the multiplier, since I removed it above
                else
                {
                    currMultiplier = (currMultiplier / 25 * (25 - (numAnswered + numUnanswered))) + 1.25f;
                }

                // If in endless mode and the multiplier somehow ends up under 1.1x, reset to 1.1x
                if (endlessMode == true && currMultiplier < 1.5f)
                {
                    currMultiplier = 1.25f;
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
            
            // Giving the player stars based on difficulty
            if(stateManager.Instance.difficulty == 0)
            {
                if(stateManager.Instance.goodLuckKiss > 0)
                {
                    stateManager.Instance.addStars(2);
                }
                else
                {
                    stateManager.Instance.addStars(1);
                }
            }
            else if (stateManager.Instance.difficulty == 1)
            {
                if (stateManager.Instance.goodLuckKiss > 0)
                {
                    stateManager.Instance.addStars(4);
                }
                else
                {
                    stateManager.Instance.addStars(2);
                }
            }
            else
            {
                if (stateManager.Instance.goodLuckKiss > 0)
                {
                    stateManager.Instance.addStars(8);
                }
                else
                {
                    stateManager.Instance.addStars(4);
                }
            }

            starboard.GetComponent<Text>().text = stateManager.Instance.getStars().ToString();
            
            if (endlessMode == true)
            {
                scoreboard.GetComponent<Text>().text = "Score: " + score;
                if(stateManager.Instance.goodGloves > 0)
                {
                    comboCounter += 2;
                }
                else
                {
                    comboCounter++;
                }

                if(comboCounter >= 2)
                {
                    comboBox.GetComponent<Image>().sprite = blankPlus;
                    comboText.GetComponent<Text>().text = comboCounter.ToString();
                }

                if(stateManager.Instance.difficulty == 0 && comboCounter % 10 == 0 && comboCounter != 0 && wrongSentinel > 0)
                {
                    wrongSentinel--;
                    updateBomb();
                }
            }

            // Set best time
            if (float.IsNaN(bestTime) || Timer.getTimer() < bestTime)
            {
                bestTime = Timer.getTimer();
            }

            // Set worst time
            if (float.IsNaN(worstTime) || Timer.getTimer() > worstTime)
            {
                worstTime = Timer.getTimer();
            }

            // Set best congruent time
            if ((float.IsNaN(bestCongTime) || Timer.getTimer() < bestCongTime) && allTrialQuestions[globalIndex].isCongruent == true)
            {
                bestCongTime = Timer.getTimer();
            }

            // Set worst congruent time
            if ((float.IsNaN(worstCongTime) || Timer.getTimer() > worstCongTime) && allTrialQuestions[globalIndex].isCongruent == true)
            {
                worstCongTime = Timer.getTimer();
            }

            // Set best incongruent time
            if ((float.IsNaN(bestIncongTime) || Timer.getTimer() < bestIncongTime) && allTrialQuestions[globalIndex].isCongruent == false)
            {
                bestIncongTime = Timer.getTimer();
            }

            // Set worst incongruent time
            if ((float.IsNaN(worstIncongTime) || Timer.getTimer() > worstIncongTime) && allTrialQuestions[globalIndex].isCongruent == false)
            {
                worstIncongTime = Timer.getTimer();
            }

            Timer.resetTimer(true);
            Debug.Log("Correct, Score: " + stateManager.Instance.score + ", Time: " + Timer.getTimer());
        }
        else
        {
            // Decreasing the timer in Time Trial mode
            if (timeTrial == true)
            {
                Timer.globalTimer--;
            }

            if (endlessMode == true)
            {
                wrongSentinel++;
                updateBomb();
                comboCounter = 0;

                comboBox.GetComponent<Image>().sprite = plusButton;
                comboText.GetComponent<Text>().text = "";
            }

            // Reset per question timer in any game mode
            Timer.resetTimer(false);
        }

        // Increment values for incorrect/missed question
        if(answered == true)
        {
            numAnswered++;
            if(correct == false) // DO NOT put this in the above 'else' block or it will trigger when not answering at all
            {
                numWrong++;
                Debug.Log("Incorrect, Score: " + stateManager.Instance.score + ", Time: " + Timer.getTimer());
            }
        }
        else
        {
            numUnanswered++;
        }

        // Advance to the next question
        globalIndex++;

        // If any exit condition is detected, transition to results screen
        // Exit conditions:
        //      Exhausted number of questions in Classic or Time Trial mode
        //      Exhausted number of wrong answers in Endless mode
        //      Ran out of time in Time Trial mode
        if ((endlessMode == true && wrongSentinel >= maxWrong) || (timeTrial == true && Timer.globalTimer <= 0.0f))
        {
            // Set Arrows textbox to the plus symbol sprite
            arrows.GetComponent<TextMeshProUGUI>().text = "";
            intro.SetActive(true);
            intro.GetComponent<Text>().text = "And the results are...!";
            StartCoroutine(bombOver());
        }
        else if (globalIndex >= givenQuestions && endlessMode == false)
        {
            // Set Arrows textbox to the plus symbol sprite
            arrows.GetComponent<TextMeshProUGUI>().text = "";
            intro.SetActive(true);
            intro.GetComponent<Text>().text = "And the results are...!";
            StartCoroutine(winOver());
        }
        // Else, proceed with game
        // Else, proceed with game
        else
        {
            // If in Endless Mode, roll next trial
            if (endlessMode == true)
            {
                resetTrials();
            }

            // Transition automatically without needing plus button
            startTrial();
        }
    }

    public void updateTimebomb()
    {
        ttBombText.GetComponent<Text>().text = string.Format("{0:0.000}", Mathf.Round(Timer.globalTimer * 1000) / 1000);

        if (Timer.globalTimer <= 0.0f || globalIndex >= givenQuestions)
        {
            return;
        }

        if (Timer.globalTimer < givenQuestions * 2 / 3)
        {
            ttBombSprite.GetComponent<SpriteRenderer>().sprite = bomb1;
        }
        else if (Timer.globalTimer < givenQuestions * 2 / 3 * 2)
        {
            ttBombSprite.GetComponent<SpriteRenderer>().sprite = bomb2;
        }
    }

    public void updateBomb()
    {
        int numLeft = maxWrong - wrongSentinel;
        bombText.GetComponent<Text>().text = numLeft.ToString();

        if (numLeft >= 3)
        {
            bombSprite.GetComponent<SpriteRenderer>().sprite = bomb3;
        }
        else if (numLeft == 2)
        {
            bombSprite.GetComponent<SpriteRenderer>().sprite = bomb2;
        }
        else
        {
            bombSprite.GetComponent<SpriteRenderer>().sprite = bomb1;
        }
    }

    // Game fail state
    IEnumerator bombOver()
    {
        GameObject bombRender;
        GameObject explodeRender;

        musicSource.Pause();

        if (timeTrial == true)
        {
            bombRender = ttBombSprite;
            explodeRender = ttExplosion;
        }
        else
        {
            bombRender = bombSprite;
            explodeRender = explosion;
        }

        bombRender.GetComponent<SpriteRenderer>().sprite = bomb0;
        explodeRender.GetComponent<SpriteRenderer>().enabled = true;
        sfxSource.PlayOneShot(bombsound, volume);

        yield return new WaitForSeconds(2.65f);

        moveToResults();
    }

    // Game win state
    IEnumerator winOver()
    {
        musicSource.Pause();

        sfxSource.PlayOneShot(drumroll, volume);

        yield return new WaitForSeconds(2.00f);

        moveToResults();
    }

    // At end of game, transition to results screen
    public void moveToResults()
    {
        // Remove items if exist
        if(timeTrial == true)
        {
            if (stateManager.Instance.stopwatch > 0)
            {
                stateManager.Instance.stopwatch--;
                PlayerPrefs.SetInt("stopwatch_" + stateManager.Instance.playerName, stateManager.Instance.stopwatch);
            }
        }
        else if(endlessMode == true && stateManager.Instance.difficulty == 0)
        {
            if (stateManager.Instance.goodGloves > 0)
            {
                stateManager.Instance.goodGloves--;
                PlayerPrefs.SetInt("goodGloves_" + stateManager.Instance.playerName, stateManager.Instance.goodGloves);
            }
            if (stateManager.Instance.longFuse > 0)
            {
                stateManager.Instance.longFuse = 0;
                PlayerPrefs.SetInt("longFuse_" + stateManager.Instance.playerName, stateManager.Instance.longFuse);
            }
        }
        if(stateManager.Instance.goodLuckKiss > 0)
        {
            stateManager.Instance.goodLuckKiss--;
            PlayerPrefs.SetInt("goodLuckKiss_" + stateManager.Instance.playerName, stateManager.Instance.goodLuckKiss);
        }

        // calculate average times
        finalTime = Timer.getTime() / score;
        finalCongTime = Timer.getCongTime() / congruentQuestions;
        finalIncongTime = Timer.getIncongTime() / incongruentQuestions;

        // Achievement: Complete each game mode at least once
        // Bronze: 1 mode
        // Silver: 2 modes
        // Gold: 3 modes
        AchievementManager.Instance.completeGamemode(stateManager.Instance.gameMode);

        // Achievement: Finish a game with no questions right
        // Bronze: 1 game
        // Silver: 2 games
        // Gold: 3 games
        if (score == 0)
        {
            AchievementManager.Instance.getAchievement(AchievementManager.Instance.achievementList[9], 1);
        }

        if (stateManager.Instance.gameMode == 0)
        {
            // Achievement: Beat a Classic Mode game with no questions wrong
            // Bronze: 1 game
            // Silver: 2 games
            // Gold: 3 games
            if (numWrong + numUnanswered == 0)
            {
                AchievementManager.Instance.getAchievement(AchievementManager.Instance.achievementList[0], 1);
            }

            // Achievement: Clear a Classic Mode game in a certain amount of time
            // Bronze: 10 seconds per question
            // Silver: 5 seconds per question
            // Gold: 2 seconds per question
            if (Timer.getTotalTime() / givenQuestions <= 2)
            {
                if (AchievementManager.Instance.achievementList[5].state == 2)
                {
                    AchievementManager.Instance.getAchievement(AchievementManager.Instance.achievementList[5], 1);
                }
                else if (AchievementManager.Instance.achievementList[5].state == 1)
                {
                    AchievementManager.Instance.getAchievement(AchievementManager.Instance.achievementList[5], 2);
                }
                else if (AchievementManager.Instance.achievementList[5].state == 0)
                {
                    AchievementManager.Instance.getAchievement(AchievementManager.Instance.achievementList[5], 3);
                }
            }
            else if (Timer.getTotalTime() / givenQuestions <= 5)
            {
                if (AchievementManager.Instance.achievementList[5].state == 1)
                {
                    AchievementManager.Instance.getAchievement(AchievementManager.Instance.achievementList[5], 1);
                }
                else if (AchievementManager.Instance.achievementList[5].state == 0)
                {
                    AchievementManager.Instance.getAchievement(AchievementManager.Instance.achievementList[5], 2);
                }
            }
            else if (Timer.getTotalTime() / givenQuestions <= 10)
            {
                if (AchievementManager.Instance.achievementList[5].state == 0)
                {
                    AchievementManager.Instance.getAchievement(AchievementManager.Instance.achievementList[5], 1);
                }
            }

            // Achievement: Complete each difficulty on classic mode
            // Bronze: Complete easy
            // Silver: Complete medium
            // Gold: Complete hard
            if (stateManager.Instance.difficulty == 0)
            {
                if (AchievementManager.Instance.achievementList[2].state == 0)
                {
                    AchievementManager.Instance.getAchievement(AchievementManager.Instance.achievementList[2], 1);
                }
            }
            else if (stateManager.Instance.difficulty == 1)
            {
                if (AchievementManager.Instance.achievementList[2].state == 1)
                {
                    AchievementManager.Instance.getAchievement(AchievementManager.Instance.achievementList[2], 1);
                }
                else if (AchievementManager.Instance.achievementList[2].state == 0)
                {
                    AchievementManager.Instance.getAchievement(AchievementManager.Instance.achievementList[2], 2);
                }
            }
            else
            {
                if (AchievementManager.Instance.achievementList[2].state == 2)
                {
                    AchievementManager.Instance.getAchievement(AchievementManager.Instance.achievementList[2], 1);
                }
                else if (AchievementManager.Instance.achievementList[2].state == 1)
                {
                    AchievementManager.Instance.getAchievement(AchievementManager.Instance.achievementList[2], 2);
                }
                else if (AchievementManager.Instance.achievementList[2].state == 0)
                {
                    AchievementManager.Instance.getAchievement(AchievementManager.Instance.achievementList[2], 3);
                }
            }
        }
        else if (stateManager.Instance.gameMode == 1)
        {
            // Achievement: Complete each difficulty on time trial mode
            // Bronze: Complete 10q20s
            // Silver: Complete 20q40s
            // Gold: Complete 30q60s
            if (stateManager.Instance.difficulty == 0)
            {
                if (AchievementManager.Instance.achievementList[3].state == 0)
                {
                    AchievementManager.Instance.getAchievement(AchievementManager.Instance.achievementList[3], 1);
                }
            }
            else if (stateManager.Instance.difficulty == 1)
            {
                if (AchievementManager.Instance.achievementList[3].state == 1)
                {
                    AchievementManager.Instance.getAchievement(AchievementManager.Instance.achievementList[3], 1);
                }
                else if (AchievementManager.Instance.achievementList[3].state == 0)
                {
                    AchievementManager.Instance.getAchievement(AchievementManager.Instance.achievementList[3], 2);
                }
            }
            else
            {
                if (AchievementManager.Instance.achievementList[3].state == 2)
                {
                    AchievementManager.Instance.getAchievement(AchievementManager.Instance.achievementList[3], 1);
                }
                else if (AchievementManager.Instance.achievementList[3].state == 1)
                {
                    AchievementManager.Instance.getAchievement(AchievementManager.Instance.achievementList[3], 2);
                }
                else if (AchievementManager.Instance.achievementList[3].state == 0)
                {
                    AchievementManager.Instance.getAchievement(AchievementManager.Instance.achievementList[3], 3);
                }
            }
        }
        else if (stateManager.Instance.gameMode == 2)
        {
            // Achievement: Last for a given number of questions in Endless Mode
            // Bronze: 10 questions
            // Silver: 20 questions
            // Gold: 50 questions
            if (score + numWrong + numUnanswered >= 50)
            {
                if (AchievementManager.Instance.achievementList[6].state == 2)
                {
                    AchievementManager.Instance.getAchievement(AchievementManager.Instance.achievementList[6], 1);
                }
                else if (AchievementManager.Instance.achievementList[6].state == 1)
                {
                    AchievementManager.Instance.getAchievement(AchievementManager.Instance.achievementList[6], 2);
                }
                else if (AchievementManager.Instance.achievementList[6].state == 0)
                {
                    AchievementManager.Instance.getAchievement(AchievementManager.Instance.achievementList[6], 3);
                }
            }
            else if (score + numWrong + numUnanswered >= 20)
            {
                if (AchievementManager.Instance.achievementList[6].state == 1)
                {
                    AchievementManager.Instance.getAchievement(AchievementManager.Instance.achievementList[6], 1);
                }
                else if (AchievementManager.Instance.achievementList[6].state == 0)
                {
                    AchievementManager.Instance.getAchievement(AchievementManager.Instance.achievementList[6], 2);
                }
            }
            else if (score + numWrong + numUnanswered >= 10)
            {
                if (AchievementManager.Instance.achievementList[6].state == 0)
                {
                    AchievementManager.Instance.getAchievement(AchievementManager.Instance.achievementList[6], 1);
                }
            }

            // Achievement: Complete each difficulty on endless mode
            // Bronze: Complete regenerative
            // Silver: Complete 3 wrong
            // Gold: Complete 1 wrong
            if (stateManager.Instance.difficulty == 0)
            {
                if (AchievementManager.Instance.achievementList[4].state == 0)
                {
                    AchievementManager.Instance.getAchievement(AchievementManager.Instance.achievementList[4], 1);
                }
            }
            else if (stateManager.Instance.difficulty == 1)
            {
                if (AchievementManager.Instance.achievementList[4].state == 1)
                {
                    AchievementManager.Instance.getAchievement(AchievementManager.Instance.achievementList[4], 1);
                }
                else if (AchievementManager.Instance.achievementList[4].state == 0)
                {
                    AchievementManager.Instance.getAchievement(AchievementManager.Instance.achievementList[4], 2);
                }
            }
            else
            {
                if (AchievementManager.Instance.achievementList[4].state == 2)
                {
                    AchievementManager.Instance.getAchievement(AchievementManager.Instance.achievementList[4], 1);
                }
                else if (AchievementManager.Instance.achievementList[4].state == 1)
                {
                    AchievementManager.Instance.getAchievement(AchievementManager.Instance.achievementList[4], 2);
                }
                else if (AchievementManager.Instance.achievementList[4].state == 0)
                {
                    AchievementManager.Instance.getAchievement(AchievementManager.Instance.achievementList[4], 3);
                }
            }
        }

        int starRating = 4;
        int achieveRating = 4;

        while(starRating != AchievementManager.Instance.achievementList[1].state || achieveRating != AchievementManager.Instance.achievementList[11].state)
        {
            starRating = AchievementManager.Instance.achievementList[1].state;
            achieveRating = AchievementManager.Instance.achievementList[11].state;

            // Achievement: Get a certain amount of stars
            // Bronze: 50 stars
            // Silver: 100 stars
            // Gold: 200 stars
            if (stateManager.Instance.getStars() >= 200)
            {
                if (AchievementManager.Instance.achievementList[1].state == 2)
                {
                    AchievementManager.Instance.getAchievement(AchievementManager.Instance.achievementList[1], 1);
                }
                else if (AchievementManager.Instance.achievementList[1].state == 1)
                {
                    AchievementManager.Instance.getAchievement(AchievementManager.Instance.achievementList[1], 2);
                }
                else if (AchievementManager.Instance.achievementList[1].state == 0)
                {
                    AchievementManager.Instance.getAchievement(AchievementManager.Instance.achievementList[1], 3);
                }
            }
            else if (stateManager.Instance.getStars() >= 100)
            {
                if (AchievementManager.Instance.achievementList[1].state == 1)
                {
                    AchievementManager.Instance.getAchievement(AchievementManager.Instance.achievementList[1], 1);
                }
                else if (AchievementManager.Instance.achievementList[1].state == 0)
                {
                    AchievementManager.Instance.getAchievement(AchievementManager.Instance.achievementList[1], 2);
                }
            }
            else if (stateManager.Instance.getStars() >= 50)
            {
                if (AchievementManager.Instance.achievementList[1].state == 0)
                {
                    AchievementManager.Instance.getAchievement(AchievementManager.Instance.achievementList[1], 1);
                }
            }

            // Achievement: Get all achievements
            // Bronze: All bronze or better
            // Silver: All silver or better
            // Gold: All gold or better
            AchievementManager.Instance.achievementsAchievement();
        }

        // Save data and transition to results screen
        stateManager.Instance.score = score;
        stateManager.Instance.wrong = numWrong;
        stateManager.Instance.unanswered = numUnanswered;
        stateManager.Instance.avgTime = finalTime;
        stateManager.Instance.bestTime = bestTime;
        stateManager.Instance.worstTime = worstTime;
        stateManager.Instance.bestCongTime = bestCongTime;
        stateManager.Instance.worstCongTime = worstCongTime;
        stateManager.Instance.bestIncongTime = bestIncongTime;
        stateManager.Instance.worstIncongTime = worstIncongTime;
        stateManager.Instance.congTimeAvg = finalCongTime;
        stateManager.Instance.incongTimeAvg = finalIncongTime;

        stateManager.Instance.saveStars();

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
