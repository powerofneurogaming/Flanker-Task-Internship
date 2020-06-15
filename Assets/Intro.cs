using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject playerName;
    public GameObject placeholder;
    void Start()
    {
        PlayerPrefs.SetInt("PlayerScore", 0);
    }
    public void startGame()
    {
        string name = playerName.GetComponent<Text>().text;
        if (name.Length <= 0)
        {
            PlayerPrefs.SetString("PlayerName", "NoName");
            SceneManager.LoadScene("Flanker Main");


        }
        else
        {
            PlayerPrefs.SetString("PlayerName", name);
            SceneManager.LoadScene("Flanker Main");

        }


    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
