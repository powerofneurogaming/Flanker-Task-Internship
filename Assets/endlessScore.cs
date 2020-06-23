// Unity libraries
using UnityEngine;
using UnityEngine.UI;

// Manages Endless Mode scoreboard. Does nothing else.
public class endlessScore : MonoBehaviour
{
    // Text element for Endless Mode scoreboard
    public Text scoreText;

    // If not Endless Mode, hide scoreboard
    void Start()
    {
        if(GameManager.endlessMode == true)
        {
            scoreText.enabled = true;
        }
        else
        {
            scoreText.enabled = false;
        }
    }

    // Update scoreboard every frame (?!)
    //
    // TODO: REMOVE, THIS IS NOT GOOD
    public void Update()
    {
        if(GameManager.endlessMode == true)
        {
            scoreText.text = "Score: " + GameManager.score;
        }
    }
}
