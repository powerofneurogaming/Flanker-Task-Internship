using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class achievements : MonoBehaviour
{
    // 'Pseudo-singleton' - uses static instance of GameManager to allow for access to GameManager object
    // !! IS NOT PERSISTENT ACROSS MULTIPLE SCENES !!
    public static achievements Instance { get; private set; }

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

    // COMMENT TEMPLATE
    // ----------------
    // Achievement: 
    // Bronze: 
    // Silver: 
    // Gold: 

    // Achievement: Beat a Classic Mode game with no questions wrong
    // Bronze: 1 game
    // Silver: 2 games
    // Gold: 3 games
    public int noneWrong;

    // Achievement: Get a certain amount of stars
    // Bronze: 50 stars
    // Silver: 100 stars
    // Gold: 200 stars
    public int getStars;

    // Achievement: Complete each difficulty on classic mode
    // Bronze: Complete easy
    // Silver: Complete medium
    // Gold: Complete hard
    public int classicDifficulty;

    // Achievement: Complete each difficulty on time trial mode
    // Bronze: Complete 10q20s
    // Silver: Complete 20q40s
    // Gold: Complete 30q60s
    public int timeDifficulty;

    // Achievement: Complete each difficulty on endless mode
    // Bronze: Complete regenerative
    // Silver: Complete 3 wrong
    // Gold: Complete 1 wrong
    public int endlessDifficulty;

    // Achievement: Clear a Classic Mode game in a certain amount of time
    // Bronze: 10 seconds per question
    // Silver: 5 seconds per question
    // Gold: 2 seconds per question
    public int classicTimed;

    // Achievement: Last for a given number of questions in Endless Mode
    // Bronze: 10 questions
    // Silver: 20 questions
    // Gold: 50 questions
    public int endlessStreak;

    // Achievement: Complete each game mode at least once
    // Bronze: 1 mode
    // Silver: 2 modes
    // Gold: 3 modes
    public int modesComplete;

    // Achievement: Click the wrong hand in the tutorial a given number of times
    // Bronze: 1 time
    // Silver: 2 times
    // Gold: 5 times
    public int dontFollowDirections;

    // Achievement: Finish a game with no questions right
    // Bronze: 1 game
    // Silver: 2 games
    // Gold: 3 games
    public int youreBadAtThis;

    // Achievement: Click on each distraction
    // Bronze: 5 distractions
    // Silver: 10 distractions
    // Gold: 15 distractions
    public int getDistracted;

    // Achievement: Stand Idle at the results screen
    // Bronze: 3 minutes
    // Silver: 5 minutes
    // Gold: 10 minutes
    public int nothingBetterToDo;

    // Achievement: Get all achievements
    // Bronze: All bronze or better
    // Silver: All silver or better
    // Gold: All gold or better
    public int youreGoodAtThis;

    bool classic;
    bool timeTrial;
    bool endless;

    public void loadAchievements()
    {
        noneWrong = PlayerPrefs.GetInt("noneWrong_" + SetPrefabs.name, 0);
        getStars = PlayerPrefs.GetInt("getStars_" + SetPrefabs.name, 0);
        classicDifficulty = PlayerPrefs.GetInt("classicDifficulty_" + SetPrefabs.name, 0);
        timeDifficulty = PlayerPrefs.GetInt("timeDifficulty_" + SetPrefabs.name, 0);
        endlessDifficulty = PlayerPrefs.GetInt("endlessDifficulty_" + SetPrefabs.name, 0);
        classicTimed = PlayerPrefs.GetInt("classicTimed_" + SetPrefabs.name, 0);
        endlessStreak = PlayerPrefs.GetInt("endlessStreak_" + SetPrefabs.name, 0);
        modesComplete = PlayerPrefs.GetInt("modesComplete_" + SetPrefabs.name, 0);
        dontFollowDirections = PlayerPrefs.GetInt("dontFollowDirections_" + SetPrefabs.name, 0);
        youreBadAtThis = PlayerPrefs.GetInt("youreBadAtThis_" + SetPrefabs.name, 0);
        getDistracted = PlayerPrefs.GetInt("getDistracted_" + SetPrefabs.name, 0);
        nothingBetterToDo = PlayerPrefs.GetInt("nothingBetterToDo_" + SetPrefabs.name, 0);
        youreGoodAtThis = PlayerPrefs.GetInt("youreGoodAtThis_" + SetPrefabs.name, 0);

        classic = PlayerPrefs.GetInt("classicAchieve_" + SetPrefabs.name, 0) == 1 ? true : false;
        timeTrial = PlayerPrefs.GetInt("timeTrialAchieve_" + SetPrefabs.name, 0) == 1 ? true : false;
        endless = PlayerPrefs.GetInt("endlessAchieve_" + SetPrefabs.name, 0) == 1 ? true : false;
    }

    public void getAchievement(int achievement, int toAdd, string name)
    {
        string type;

        if(achievement < 3)
        {
            achievement += toAdd;
            if(achievement > 3)
            {
                achievement = 3;
            }
        }

        if (achievement == 1)
        {
            type = "Bronze";
        }
        else if (achievement == 2)
        {
            type = "Silver";
        }
        else
        {
            type = "Gold";
        }

        Debug.Log("You got: " + name + " - " + type);
    }

    public void completeGamemode(int gameMode)
    {
        if (gameMode == 0)
        {
            PlayerPrefs.SetInt("classicAchieve_" + SetPrefabs.name, true ? 1 : 0);
            classic = true;
        }
        else if (gameMode == 1)
        {
            PlayerPrefs.SetInt("timeTrialAchieve_" + SetPrefabs.name, true ? 1 : 0);
            timeTrial = true;
        }
        else if (gameMode == 2)
        {
            PlayerPrefs.SetInt("endlessAchieve_" + SetPrefabs.name, true ? 1 : 0);
            endless = true;
        }
    }
}
