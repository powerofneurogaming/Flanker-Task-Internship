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
    bool timer_start;

    //[SerializeField]
    //private int numQuestions;

    [SerializeField]
    private float questionTransitionTime = 1f;

    public Button plusButton;
    public GameObject arrows;

    private void Start()
    {
        score = 0;
        time = 0.0f;
        timer = 0.0f;
        timer_start = false;
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
        if(timer_start == true)
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
        arrows.SetActive(true);

        // plusButton.interactable = false;

        Question trial = allTrialQuestions[globalIndex];
        StartCoroutine(displayTrial(trial.flankerArrows));

    }
    IEnumerator displayTrial(string trial)
    {
        arrows.GetComponent<Text>().text = "+";
        yield return new WaitForSeconds(.5f);
        timer_start = true;
        arrows.GetComponent<Text>().text = trial;
    }



    public void userSelectRight()
    {
        timer_start = false;
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

        // plusButton.interactable = true;

        if (globalIndex == givenQuestions)
        {
            moveToResults();
        }
    }
    public void userSelectLeft()
    {
        timer_start = false;
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

        // plusButton.interactable = true;

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
        timer = 0.0f;
    }

    public void moveToResults()
    {
        time = time / givenQuestions;
        PlayerPrefs.SetFloat("avgTime", time);
        SceneManager.LoadScene("Flanker Result");
    }
}
