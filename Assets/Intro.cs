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
        PlayerPrefs.SetFloat("avgTime", 0.0f);
        PlayerPrefs.SetFloat("avgCongTime", 0.0f);
        PlayerPrefs.SetFloat("avgIncongTime", 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
