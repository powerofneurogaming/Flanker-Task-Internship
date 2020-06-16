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
    public int totalQuestions;

    private Question previousQuestion;
    private Question currentQuestion;
    private int randQuestionIndex;
    public int globalIndex = 0;

    [SerializeField]
    private int numQuestions;

    [SerializeField]
    public GameObject arrows;

    [SerializeField]
    private float questionTransitionTime = 1f;

    private void Start()
    {
        arrows.SetActive(false);
        allTrialQuestions = new Question[10];
        LoadTrials();
        foreach (Question question in allTrialQuestions)
        {
            Debug.Log(question.flankerArrows);
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

        Question trial = allTrialQuestions[globalIndex];
        StartCoroutine(displayTrial(trial.flankerArrows));
        globalIndex++;
    }
    IEnumerator displayTrial(string trail)
    {
        arrows.GetComponent<Text>().text = "+";
        yield return new WaitForSeconds(.5f);
        arrows.GetComponent<Text>().text = trail;

    }
   

    
    public void userSelectRight()
    {
        GameObject[] buttons = GameObject.FindGameObjectsWithTag("button");

        if (!allTrialQuestions[globalIndex].isLeft)
        {
            Debug.Log("Correct");
            answerCorrect();
        } else {
            Debug.Log("Incorrect");
        }

        foreach (GameObject obj in buttons)
        {
            obj.active = false;
        }
    }
    public void userSelectLeft()
    {
        GameObject[] buttons = GameObject.FindGameObjectsWithTag("button");

        if (allTrialQuestions[globalIndex].isLeft)
        {
            Debug.Log("Correct");
            answerCorrect();
        }
        else {
            Debug.Log("Incorrect");
        }

        foreach (GameObject obj in buttons)
        {
            obj.active = false;
        }
    }
    
    public void answerCorrect()
    {
        int score = PlayerPrefs.GetInt("PlayerScore");
        score++;
        PlayerPrefs.SetInt("PlayerScore", score);
    }
}
