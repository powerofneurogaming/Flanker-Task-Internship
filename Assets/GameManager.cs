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

    [SerializeField]
    private int givenQuestions = 10;

    private Question previousQuestion;
    private Question currentQuestion;
    private int randQuestionIndex;
    public static int globalIndex;
    public int score;

    float finalTime;
    float finalCongTime;
    float finalIncongTime;

    public static int congruentQuestions;
    public static int incongruentQuestions;

    bool isAnswered;

    //[SerializeField]
    //private int numQuestions;

    [SerializeField]
    private float questionTransitionTime = 1f;

    public Button plusButton;
    public GameObject arrows;

    private void Start()
    {
        score = 0;
        isAnswered = true;
        Timer.timerStart = false;
        PlayerPrefs.SetInt("PlayerScore", score);

        congruentQuestions = 0;
        incongruentQuestions = 0;

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
        arrows.GetComponent<Text>().text = "+";
        GameObject[] buttons = GameObject.FindGameObjectsWithTag("button");

        if (!allTrialQuestions[globalIndex].isLeft)
        {
            answerCorrect();
            Debug.Log("Correct, Score: " + PlayerPrefs.GetInt("PlayerScore") + ", Time: " + Timer.getTimer());
        } else {
            Debug.Log("Incorrect, Score: " + PlayerPrefs.GetInt("PlayerScore") + ", Time: " + Timer.getTimer());
        }

        Timer.resetTimer();
        globalIndex++;

        foreach (GameObject obj in buttons)
        {
            obj.SetActive(false);
        }

        isAnswered = true;

        if (globalIndex == givenQuestions)
        {
            moveToResults();
        }
    }
    public void userSelectLeft()
    {
        Timer.timerStart = false;
        arrows.GetComponent<Text>().text = "+";
        GameObject[] buttons = GameObject.FindGameObjectsWithTag("button");

        if (allTrialQuestions[globalIndex].isLeft)
        {
            answerCorrect();
            Debug.Log("Correct, Score: " + PlayerPrefs.GetInt("PlayerScore") + ", Time: " + Timer.getTimer());
        }
        else {
            Debug.Log("Incorrect, Score: " + PlayerPrefs.GetInt("PlayerScore") + ", Time: " + Timer.getTimer());
        }

        Timer.resetTimer();
        globalIndex++;

        foreach (GameObject obj in buttons)
        {
            obj.SetActive(false);
        }

        isAnswered = true;

        if (globalIndex == givenQuestions)
        {
            moveToResults();
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
        finalTime = Timer.getTime() / givenQuestions;
        finalCongTime = Timer.getCongTime() / congruentQuestions;
        finalIncongTime = Timer.getIncongTime() / incongruentQuestions;

        PlayerPrefs.SetFloat("avgTime", finalTime);
        PlayerPrefs.SetFloat("avgCongTime", finalCongTime);
        PlayerPrefs.SetFloat("avgIncongTime", finalIncongTime);
        SceneManager.LoadScene("Flanker Result");
    }
}
