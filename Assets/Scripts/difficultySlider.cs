// Unity libraries
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Difficulty slider logic
public class difficultySlider : MonoBehaviour
{
    int difficulty;
    public Slider difficultySetter;
    public Text difficultyText;
    Scene currentScene;

    // Set difficulty sider to the easiest mode on start and display correct text
    private void Start()
    {
        currentScene = SceneManager.GetActiveScene();
        difficulty = 0;
        stateManager.Instance.difficulty = difficulty;

        // Classic Mode
        if(currentScene.name == "Classic Select")
        {
            difficultyText.text = "Easy";
        }
        // Time Trial Mode
        else if(currentScene.name == "Time Select")
        {
            difficultyText.text = "20 seconds, 10 questions";
        }
        else
        {
            difficultyText.text = "3 Wrong - Regenerative";
        }
    }

    // Logic for changing difficulty slider text
    public void setDifficulty()
    {
        // Get output from slider
        difficulty = (int)Mathf.Round(difficultySetter.value);

        // If easy
        if(difficulty == 0)
        {
            // If Classic Mode
            if (currentScene.name == "Classic Select")
            {
                difficultyText.text = "Easy";
            }
            // If Time Trial Mode
            else if (currentScene.name == "Time Select")
            {
                difficultyText.text = "20 seconds, 10 questions";
            }
            else
            {
                difficultyText.text = "3 Wrong - Regenerative";
            }

        }
        // If medium
        else if(difficulty == 1)
        {
            // If Classic Mode
            if (currentScene.name == "Classic Select")
            {
                difficultyText.text = "Medium";
            }
            // If Time Trial Mode
            else if (currentScene.name == "Time Select")
            {
                difficultyText.text = "40 seconds, 20 questions";
            }
            else
            {
                difficultyText.text = "3 Wrong";
            }
        }
        // If hard
        else
        {
            // If Classic Mode
            if (currentScene.name == "Classic Select")
            {
                difficultyText.text = "Hard";
            }
            // If Time Trial Mode
            else if (currentScene.name == "Time Select")
            {
                difficultyText.text = "60 seconds, 30 questions";
            }
            else
            {
                difficultyText.text = "1 Wrong";
            }
        }

        // Save difficulty so it can be accessed by the main game
        stateManager.Instance.difficulty = difficulty;
    }
}
