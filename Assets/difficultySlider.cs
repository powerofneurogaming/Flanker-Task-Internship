using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class difficultySlider : MonoBehaviour
{
    int difficulty;
    public Slider difficultySetter;
    public Text difficultyText;

    private void Start()
    {
        difficulty = 0;
        PlayerPrefs.SetInt("Difficulty", difficulty);
    }

    public void setDifficulty()
    {
        difficulty = (int)Mathf.Round(difficultySetter.value);
        if(difficulty == 0)
        {
            difficultyText.text = "Easy";
        }
        else if(difficulty == 1)
        {
            difficultyText.text = "Medium";
        }
        else
        {
            difficultyText.text = "Hard";
        }
        PlayerPrefs.SetInt("Difficulty", difficulty);
    }
}
