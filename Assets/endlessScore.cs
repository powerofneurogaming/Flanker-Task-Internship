using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class endlessScore : MonoBehaviour
{
    public Text scoreText;

    // Start is called before the first frame update
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

    // Update is called once per frame
    public void Update()
    {
        scoreText.text = "Score: " + GameManager.score;
    }
}
