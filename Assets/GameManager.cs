using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public Question[] questions;
    public static Question[] allTrialQuestions;

    int givenQuestions;

    [SerializeField]
    private int maxTime;

    private Question previousQuestion;
    private Question currentQuestion;
    private int randQuestionIndex;
    public static int globalIndex;
    public int score;
    public int wrongAnswer;
    public int numAnswered;

    float finalTime;
    float finalCongTime;
    float finalIncongTime;

    float currentTimer;

    public static int congruentQuestions;
    public static int incongruentQuestions;

    bool isAnswered;
    bool endlessMode;

    //[SerializeField]
    //private int numQuestions;

    [SerializeField]
    private float questionTransitionTime = 1f;

    public Button plusButton;
    public GameObject arrows;

    private void Start()
    {
        givenQuestions = PlayerPrefs.GetInt("PlayerLevel");
        if(givenQuestions == 0)
        {
            givenQuestions = 10;
            endlessMode = true;
        }
        else
        {
            endlessMode = false;
        }
        score = 0;
        wrongAnswer = 0;
        isAnswered = true;
        Timer.timerStart = false;
        PlayerPrefs.SetInt("PlayerScore", score);

        congruentQuestions = 0;
        incongruentQuestions = 0;

        maxTime = givenQuestions;

        GameObject[] buttons = GameObject.FindGameObjectsWithTag("button");
        foreach (GameObject obj in buttons)
        {
            obj.SetActive(false);
        }
        arrows.SetActive(false);
        allTrialQuestions = new Question[givenQuestions];
        LoadTrials();
        foreach (Question question in allTrialQuestions)
        {
            Debug.Log(question.flankerArrows);
        }
    }

    private void Update()
    {
        currentTimer = Timer.getTimer();
        if (currentTimer >= maxTime)
        {
            userSelectNone();
        }
    }

    void LoadTrials()
    {
        for (int i = 0; i < allTrialQuestions.Length; i++)
        {
            randQuestionIndex = Random.Range(0, questions.Length);
            currentQuestion = questions[randQuestionIndex];

            //Debug.Log(currentQuestion.flankerArrows);
            //Debug.Log(currentlevel.ToString());

            if (previousQuestion == null)
            {
                allTrialQuestions[i] = currentQuestion;
            }
            else
            {
                while (previousQuestion.flankerArrows == currentQuestion.flankerArrows)
                {
                    randQuestionIndex = Random.Range(0, questions.Length);
                    currentQuestion = questions[randQuestionIndex];
                }
                allTrialQuestions[i] = currentQuestion;
            }
            previousQuestion = currentQuestion;
        }
    }
    public void startTrial()
    {
        if (isAnswered == true)
        {
            arrows.SetActive(true);

            // plusButton.interactable = false;

            Question trial = allTrialQuestions[globalIndex];
            StartCoroutine(displayTrial(trial.flankerArrows));
            isAnswered = false;
        }
    }
    IEnumerator displayTrial(string trial)
    {
        arrows.GetComponent<Text>().text = "+";
        yield return new WaitForSeconds(.5f);
        Timer.timerStart = true;
        arrows.GetComponent<Text>().text = trial;
    }



    public void userSelectRight()
    {
        Timer.timerStart = false;
        if (!allTrialQuestions[globalIndex].isLeft)
        {
            answerCorrect();
            Debug.Log("Correct, Score: " + PlayerPrefs.GetInt("PlayerScore") + ", Time: " + Timer.getTimer());
        } else
        {
            if (endlessMode == true)
            {
                wrongAnswer++;
            }
            Debug.Log("Incorrect, Score: " + PlayerPrefs.GetInt("PlayerScore") + ", Time: " + Timer.getTimer());
        }

        Timer.resetTimer(true);
        numAnswered++;

        userSelectEnd();
    }
    public void userSelectLeft()
    {
        Timer.timerStart = false;
        if (allTrialQuestions[globalIndex].isLeft)
        {
            answerCorrect();
            Debug.Log("Correct, Score: " + PlayerPrefs.GetInt("PlayerScore") + ", Time: " + Timer.getTimer());
        }
        else
        {
            if(endlessMode == true)
            {
                wrongAnswer++;
            }
            Debug.Log("Incorrect, Score: " + PlayerPrefs.GetInt("PlayerScore") + ", Time: " + Timer.getTimer());
        }

        Timer.resetTimer(true);
        numAnswered++;

        userSelectEnd();
    }

    public void userSelectNone()
    {
        Timer.timerStart = false;
        if (endlessMode == true)
        {
            wrongAnswer++;
        }
        Debug.Log("Incorrect, Score: " + PlayerPrefs.GetInt("PlayerScore") + ", Time: " + Timer.getTimer());

        Timer.resetTimer(false);

        userSelectEnd();
    }

    public void userSelectEnd()
    {
        if(maxTime >= 2)
        {
            maxTime--;
        }

        globalIndex++;

        isAnswered = true;

        if (globalIndex == givenQuestions || wrongAnswer >= 3)
        {
            moveToResults();
        }
        else
        {
            arrows.GetComponent<Text>().text = "+";
            GameObject[] buttons = GameObject.FindGameObjectsWithTag("button");

            foreach (GameObject obj in buttons)
            {
                obj.SetActive(false);
            }
        }
    }

    public void answerCorrect()
    {
        int score = PlayerPrefs.GetInt("PlayerScore");
        score++;
        PlayerPrefs.SetInt("PlayerScore", score);
    }

    public void moveToResults()
    {
        if (endlessMode == false || wrongAnswer >= 3)
        {
            finalTime = Timer.getTime() / numAnswered;
            finalCongTime = Timer.getCongTime() / congruentQuestions;
            finalIncongTime = Timer.getIncongTime() / incongruentQuestions;

            PlayerPrefs.SetFloat("avgTime", finalTime);
            PlayerPrefs.SetFloat("avgCongTime", finalCongTime);
            PlayerPrefs.SetFloat("avgIncongTime", finalIncongTime);
            SceneManager.LoadScene("Flanker Result");
        }
        else
        {
            GameObject[] buttons = GameObject.FindGameObjectsWithTag("button");
            foreach (GameObject obj in buttons)
            {
                obj.SetActive(false);
            }
            arrows.SetActive(false);
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
