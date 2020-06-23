using UnityEngine;

public class Intro : MonoBehaviour
{
    
    public GameObject playerName;
    public GameObject placeholder;
    void Start()
    {
        PlayerPrefs.SetInt("PlayerScore", 0);
        PlayerPrefs.SetFloat("avgTime", 0.0f);
        PlayerPrefs.SetFloat("avgCongTime", 0.0f);
        PlayerPrefs.SetFloat("avgIncongTime", 0.0f);
    }
}
