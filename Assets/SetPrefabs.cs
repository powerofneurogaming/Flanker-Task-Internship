using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SetPrefabs : MonoBehaviour
{
    public GameObject playerName;

    public void setupPrefabs()
    {
        PlayerPrefs.SetInt("PlayerScore", 0);
        PlayerPrefs.SetInt("PlayerLevel", 0);
        PlayerPrefs.SetFloat("avgTime", 0.0f);
        PlayerPrefs.SetFloat("avgCongTime", 0.0f);
        PlayerPrefs.SetFloat("avgIncongTime", 0.0f);

        string name = playerName.GetComponent<Text>().text;

        if (name.Length <= 0)
        {
            PlayerPrefs.SetString("PlayerName", "NoName");
        }
        else
        {
            PlayerPrefs.SetString("PlayerName", name);
        }

        SceneManager.LoadScene("Select Screen");
    }

    public void setLevel()
    {
        string level = playerName.GetComponent<Text>().text;
        int.TryParse(level, out int level_int);
        PlayerPrefs.SetInt("PlayerLevel", level_int);
        SceneManager.LoadScene("Flanker Main");
    }
}
   
