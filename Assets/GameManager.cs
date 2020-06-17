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

    //[SerializeField]
    //private int numQuestions;

    [SerializeField]
    private float questionTransitionTime = 1f;

    public Button plusButton;
    public GameObject arrows;

    private void Start()
    {
        int score = PlayerPrefs.GetInt("PlayerScore");
        score = 0;
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
        Debug.Log("github test");
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
        arrows.GetComponent<Text>().text = trial;
    }



    public void userSelectRight()
    {
        arrows.GetComponent<Text>().text = "+";
        GameObject[] buttons = GameObject.FindGameObjectsWithTag("button");

        if (!allTrialQuestions[globalIndex].isLeft)
        {
            answerCorrect();
            Debug.Log("Correct, Score: " + PlayerPrefs.GetInt("PlayerScore"));
        } else {
            Debug.Log("Incorrect, Score: " + PlayerPrefs.GetInt("PlayerScore"));
        }
        globalIndex++;
        foreach (GameObject obj in buttons)
        {
            obj.SetActive(false);
        }

        // plusButton.interactable = true;

        if (globalIndex == givenQuestions)
        {
            SceneManager.LoadScene("Flanker Result");
        }
    }
    public void userSelectLeft()
    {
        arrows.GetComponent<Text>().text = "+";
        GameObject[] buttons = GameObject.FindGameObjectsWithTag("button");

        if (allTrialQuestions[globalIndex].isLeft)
        {
            answerCorrect();
            Debug.Log("Correct, Score: " + PlayerPrefs.GetInt("PlayerScore"));
        }
        else {
            Debug.Log("Incorrect, Score: " + PlayerPrefs.GetInt("PlayerScore"));
        }
        globalIndex++;
        foreach (GameObject obj in buttons)
        {
            obj.SetActive(false);
        }

        // plusButton.interactable = true;

        if (globalIndex == givenQuestions)
        {
            SceneManager.LoadScene("Flanker Result");
        }
    }
    
    public void answerCorrect()
    {
        int score = PlayerPrefs.GetInt("PlayerScore");
        score++;
        PlayerPrefs.SetInt("PlayerScore", score);
    }
}
