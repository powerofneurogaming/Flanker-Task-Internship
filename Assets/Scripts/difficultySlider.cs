using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class difficultySlider : MonoBehaviour
{
    int difficulty;
    public Slider difficultySetter;
    public Text difficultyText;
    Scene currentScene;

    private void Start()
    {
        currentScene = SceneManager.GetActiveScene();
        difficulty = 0;
        PlayerPrefs.SetInt("Difficulty", difficulty);

        if(currentScene.name == "Classic Select")
        {
            difficultyText.text = "Easy";
        }
        else if(currentScene.name == "Time Select")
        {
            difficultyText.text = "20 seconds, 10 questions";
        }
    }

    public void setDifficulty()
    {
        difficulty = (int)Mathf.Round(difficultySetter.value);
        if(difficulty == 0)
        {
            if (currentScene.name == "Classic Select")
            {
                difficultyText.text = "Easy";
            }
            else if (currentScene.name == "Time Select")
            {
                difficultyText.text = "20 seconds, 10 questions";
            }
        }
        else if(difficulty == 1)
        {
            if (currentScene.name == "Classic Select")
            {
                difficultyText.text = "Medium";
            }
            else if (currentScene.name == "Time Select")
            {
                difficultyText.text = "40 seconds, 20 questions";
            }
        }
        else
        {
            if (currentScene.name == "Classic Select")
            {
                difficultyText.text = "Hard";
            }
            else if (currentScene.name == "Time Select")
            {
                difficultyText.text = "60 seconds, 30 questions";
            }
        }
        PlayerPrefs.SetInt("Difficulty", difficulty);
    }
}
