using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class starManager : MonoBehaviour
{
    public static starManager Instance { get; private set; }

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

    int starScore;

    // Start is called before the first frame update
    public void loadStars()
    {
        starScore = PlayerPrefs.GetInt("starScore_" + stateManager.Instance.playerName, 0);
    }

    public int getStars()
    {
        return starScore;
    }

    public void addStars(int stars)
    {
        starScore += stars;
    }

    public void saveStars()
    {
        PlayerPrefs.SetInt("starScore_" + stateManager.Instance.playerName, starScore);
    }
}
