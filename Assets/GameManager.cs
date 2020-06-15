using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public Question[] questions;
    public Question[] allTrailQuestions;
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
        allTrailQuestions = new Question[10];
        LoadTrails();
        foreach (Question question in allTrailQuestions)
        {
            Debug.Log(question.flankerArrows);
        }
        //StartFlankerTask();

    }

    

    //public void StartFlankerTask()
    //{

    //    randQuestionIndex = Random.Range(0, questions.Length);
    //    int currentlevel = PlayerPrefs.GetInt("PlayerLevel");
        
    //    currentQuestion = questions[randQuestionIndex];

    //    Debug.Log(currentQuestion.flankerArrows);
    //    Debug.Log(currentlevel.ToString());

    //    if (previousQuestion == null)
    //    {
    //        arrows.GetComponent<Text>().text = currentQuestion.flankerArrows;

    //    } else {
    //        randQuestionIndex = Random.Range(0, questions.Length);
    //        currentQuestion = questions[randQuestionIndex];

    //        arrows.GetComponent<Text>().text = currentQuestion.flankerArrows;
    //    }
    //    currentlevel++;
    //    PlayerPrefs.SetInt("PlayerLevel", currentlevel);
    //    previousQuestion = currentQuestion;
    //}
    void LoadTrails()
    {
        for (int i = 0; i < allTrailQuestions.Length; i++)
        {
            randQuestionIndex = Random.Range(0, questions.Length);
            currentQuestion = questions[randQuestionIndex];

            //Debug.Log(currentQuestion.flankerArrows);
            //Debug.Log(currentlevel.ToString());

            if (previousQuestion == null)
            {
                allTrailQuestions[i] = currentQuestion;
            }
            else
            {
                while (previousQuestion.flankerArrows == currentQuestion.flankerArrows)
                {
                    randQuestionIndex = Random.Range(0, questions.Length);
                    currentQuestion = questions[randQuestionIndex];

                }
                allTrailQuestions[i] = currentQuestion;

            }
            previousQuestion = currentQuestion;
        }

    }
    public void startTrail()
    {
        arrows.SetActive(true);

        Question trail = allTrailQuestions[globalIndex];
        StartCoroutine(displayTrail(trail.flankerArrows));
        globalIndex++;
    }
    IEnumerator displayTrail(string trail)
    {
        arrows.GetComponent<Text>().text = "+";
        yield return new WaitForSeconds(.5f);
        arrows.GetComponent<Text>().text = trail;

    }
    //IEnumerator TransitionToNextQuestion()
    //{
    //    int currentLevel = PlayerPrefs.GetInt("PlayerLevel");
    //    yield return new WaitForSeconds(questionTransitionTime);
    //    if (currentLevel > totalQuestions)
    //    {
    //        GameObject[] buttons = GameObject.FindGameObjectsWithTag("button");

    //        foreach (GameObject obj in buttons)
    //        {
    //            obj.active = false;
    //        }

    //        string playerScore = PlayerPrefs.GetInt("PlayerScore").ToString();
    //        arrows.GetComponent<Text>().text = "You scored " + playerScore;

    //    } else {
    //        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    //    }
    //}

    
    public void userSelectRight()
    {
        GameObject[] buttons = GameObject.FindGameObjectsWithTag("button");

        if (!allTrailQuestions[globalIndex].isLeft)
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

        if (allTrailQuestions[globalIndex].isLeft)
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
