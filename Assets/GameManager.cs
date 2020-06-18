using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public Question[] questions;
    public Question[] allTrialQuestions;

    [SerializeField]
    private int givenQuestions = 10;

    private Question previousQuestion;
    private Question currentQuestion;
    private int randQuestionIndex;
    public int globalIndex;
    public int score;

    float timer;
    float time;

    float congruentTime;
    int congruentQuestions;

    float incongruentTime;
    int incongruentQuestions;

    bool timerStart;
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
        timer = 0.0f;
        time = 0.0f;

        congruentTime = 0.0f;
        congruentQuestions = 0;

        incongruentTime = 0.0f;
        incongruentQuestions = 0;

        timerStart = false;
        isAnswered = true;
        PlayerPrefs.SetInt("PlayerScore", score);

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
        if(timerStart == true)
        {
            timer += Time.deltaTime;
            Debug.Log("Time: " + timer);
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
        timerStart = true;
        arrows.GetComponent<Text>().text = trial;
    }



    public void userSelectRight()
    {
        timerStart = false;
        arrows.GetComponent<Text>().text = "+";
        GameObject[] buttons = GameObject.FindGameObjectsWithTag("button");

        if (!allTrialQuestions[globalIndex].isLeft)
        {
            answerCorrect();
            Debug.Log("Correct, Score: " + PlayerPrefs.GetInt("PlayerScore") + ", Time: " + timer);
        } else {
            Debug.Log("Incorrect, Score: " + PlayerPrefs.GetInt("PlayerScore") + ", Time: " + timer);
        }

        resetTimer();
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
        timerStart = false;
        arrows.GetComponent<Text>().text = "+";
        GameObject[] buttons = GameObject.FindGameObjectsWithTag("button");

        if (allTrialQuestions[globalIndex].isLeft)
        {
            answerCorrect();
            Debug.Log("Correct, Score: " + PlayerPrefs.GetInt("PlayerScore") + ", Time: " + timer);
        }
        else {
            Debug.Log("Incorrect, Score: " + PlayerPrefs.GetInt("PlayerScore") + ", Time: " + timer);
        }

        resetTimer();
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

    public void resetTimer()
    {
        time += timer;
        if(allTrialQuestions[globalIndex].isCongruent == true)
        {
            congruentTime += timer;
            congruentQuestions++;
        }
        else
        {
            incongruentTime += timer;
            incongruentQuestions++;
        }
        timer = 0.0f;
    }

    public void moveToResults()
    {
        time = time / givenQuestions;
        congruentTime = congruentTime / congruentQuestions;
        incongruentTime = incongruentTime / incongruentQuestions;

        PlayerPrefs.SetFloat("avgTime", time);
        PlayerPrefs.SetFloat("avgCongTime", congruentTime);
        PlayerPrefs.SetFloat("avgIncongTime", incongruentTime);
        SceneManager.LoadScene("Flanker Result");
    }
}
