using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SetPrefabs : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject playerName;
    void Start()
    {

    }
    public void setupPrefabs()
    {
        PlayerPrefs.SetInt("PlayerScore", 0);
        PlayerPrefs.SetInt("PlayerLevel", 0);

        string name = playerName.GetComponent<Text>().text;

        if (name.Length <= 0)
        {
            PlayerPrefs.SetString("PlayerName", "NoName");
        }
        else
        {
            PlayerPrefs.SetString("PlayerName", name);
        }

        SceneManager.LoadScene("Flanker Main");

    }
}
   
