using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorialGate : MonoBehaviour
{
    public static tutorialGate Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool hasPlayedTutorial;

    public void setTrue()
    {
        PlayerPrefs.SetInt("HasPlayedTutorial_" + SetPrefabs.name, true ? 1 : 0);
        hasPlayedTutorial = true;
    }

    public void setFalse()
    {
        PlayerPrefs.SetInt("HasPlayedTutorial_" + SetPrefabs.name, false ? 1 : 0);
        hasPlayedTutorial = false;
    }

    public void getPlayed()
    {
        hasPlayedTutorial = PlayerPrefs.GetInt("HasPlayedTutorial_" + SetPrefabs.name, 0) == 1 ? true : false;
    }
}
