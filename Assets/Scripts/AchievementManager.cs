using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    public int numTrue(params bool[] bools)
    {
        return bools.Count(n => n);
    }

    public static AchievementManager Instance { get; private set; }

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
    public Achievement noneWrong;

    // Achievement: Get a certain amount of stars
    // Bronze: 50 stars
    // Silver: 100 stars
    // Gold: 200 stars
    public Achievement getStars;

    // Achievement: Complete each difficulty on classic mode
    // Bronze: Complete easy
    // Silver: Complete medium
    // Gold: Complete hard
    public Achievement classicDifficulty;

    // Achievement: Complete each difficulty on time trial mode
    // Bronze: Complete 10q20s
    // Silver: Complete 20q40s
    // Gold: Complete 30q60s
    public Achievement timeDifficulty;

    // Achievement: Complete each difficulty on endless mode
    // Bronze: Complete regenerative
    // Silver: Complete 3 wrong
    // Gold: Complete 1 wrong
    public Achievement endlessDifficulty;

    // Achievement: Clear a Classic Mode game in a certain amount of time
    // Bronze: 10 seconds per question
    // Silver: 5 seconds per question
    // Gold: 2 seconds per question
    public Achievement classicTimed;

    // Achievement: Last for a given number of questions in Endless Mode
    // Bronze: 10 questions
    // Silver: 20 questions
    // Gold: 50 questions
    public Achievement endlessStreak;

    // Achievement: Complete each game mode at least once
    // Bronze: 1 mode
    // Silver: 2 modes
    // Gold: 3 modes
    public Achievement modesComplete;

    // Achievement: Click the wrong hand in the tutorial a given number of times
    // Bronze: 1 time
    // Silver: 2 times
    // Gold: 3 times
    public Achievement dontFollowDirections;

    // Achievement: Finish a game with no questions right
    // Bronze: 1 game
    // Silver: 2 games
    // Gold: 3 games
    public Achievement youreBadAtThis;

    // Achievement: Click on each distraction
    // Bronze: 5 distractions
    // Silver: 10 distractions
    // Gold: 15 distractions
    public Achievement getDistracted;

    // Achievement: Stand Idle at the results screen
    // Bronze: 3 minutes
    // Silver: 5 minutes
    // Gold: 10 minutes
    public Achievement nothingBetterToDo;

    // Achievement: Get all achievements
    // Bronze: All bronze or better
    // Silver: All silver or better
    // Gold: All gold or better
    public Achievement youreGoodAtThis;

    bool classic;
    bool timeTrial;
    bool endless;

    public void loadAchievements()
    {
        noneWrong.privateName = "noneWrong_";
        noneWrong.friendlyName = "None Wrong (Classic Mode)";
        noneWrong.state = PlayerPrefs.GetInt(noneWrong.privateName + SetPrefabs.name, 0);

        getStars.privateName = "getStars_";
        getStars.friendlyName = "Get Stars";
        getStars.state = PlayerPrefs.GetInt(getStars.privateName + SetPrefabs.name, 0);

        classicDifficulty.privateName = "classicDifficulty_";
        classicDifficulty.friendlyName = "Cleared Mode (Classic Mode)";
        classicDifficulty.state = PlayerPrefs.GetInt(classicDifficulty.privateName + SetPrefabs.name, 0);

        timeDifficulty.privateName = "timeDifficulty_";
        timeDifficulty.friendlyName = "Cleared Mode (Time Trial Mode)";
        timeDifficulty.state = PlayerPrefs.GetInt(timeDifficulty.privateName + SetPrefabs.name, 0);

        endlessDifficulty.privateName = "endlessDifficulty_";
        endlessDifficulty.friendlyName = "Cleared Mode (Endless Mode)";
        endlessDifficulty.state = PlayerPrefs.GetInt(endlessDifficulty.privateName + SetPrefabs.name, 0);

        classicTimed.privateName = "classicTimed_";
        classicTimed.friendlyName = "Timed Clear (Classic Mode)";
        classicTimed.state = PlayerPrefs.GetInt(classicTimed.privateName + SetPrefabs.name, 0);

        endlessStreak.privateName = "endlessStreak_";
        endlessStreak.friendlyName = "Streak (Endless Mode)";
        endlessStreak.state = PlayerPrefs.GetInt(endlessStreak.privateName + SetPrefabs.name, 0);

        modesComplete.privateName = "modesComplete_";
        modesComplete.friendlyName = "Modes Completed";
        modesComplete.state = PlayerPrefs.GetInt(modesComplete.privateName + SetPrefabs.name, 0);

        dontFollowDirections.privateName = "dontFollowDirections_";
        dontFollowDirections.friendlyName = "Wrong Hand (Tutorial)";
        dontFollowDirections.state = PlayerPrefs.GetInt(dontFollowDirections.privateName + SetPrefabs.name, 0);

        youreBadAtThis.privateName = "youreBadAtThis_";
        youreBadAtThis.friendlyName = "No Questions Right";
        youreBadAtThis.state = PlayerPrefs.GetInt(youreBadAtThis.privateName + SetPrefabs.name, 0);

        getDistracted.privateName = "getDistracted_";
        getDistracted.friendlyName = "Clicked Distractions";
        getDistracted.state = PlayerPrefs.GetInt(getDistracted.privateName + SetPrefabs.name, 0);

        nothingBetterToDo.privateName = "nothingBetterToDo_";
        nothingBetterToDo.friendlyName = "Stay at Results Screen";
        nothingBetterToDo.state = PlayerPrefs.GetInt(nothingBetterToDo.privateName + SetPrefabs.name, 0);

        youreGoodAtThis.privateName = "youreGoodAtThis_";
        youreGoodAtThis.friendlyName = "Get Achievements";
        youreGoodAtThis.state = PlayerPrefs.GetInt(youreGoodAtThis.privateName + SetPrefabs.name, 0);

        classic = PlayerPrefs.GetInt("classicAchieve_" + SetPrefabs.name, 0) == 1 ? true : false;
        timeTrial = PlayerPrefs.GetInt("timeTrialAchieve_" + SetPrefabs.name, 0) == 1 ? true : false;
        endless = PlayerPrefs.GetInt("endlessAchieve_" + SetPrefabs.name, 0) == 1 ? true : false;
    }

    public void getAchievement(ref Achievement achievement, int toAdd)
    {
        if (achievement.state >= 3 || toAdd == 0)
        {
            return;
        }

        string type;

        achievement.state += toAdd;
        if (achievement.state > 3)
        {
            achievement.state = 3;
        }

        if (achievement.state == 1)
        {
            type = "Bronze";
        }
        else if (achievement.state == 2)
        {
            type = "Silver";
        }
        else
        {
            type = "Gold";
        }

        PlayerPrefs.SetInt(achievement.privateName + SetPrefabs.name, achievement.state);

        Debug.Log("You got: " + achievement.friendlyName + " - " + type);
    }

    public void completeGamemode(int gameMode)
    {
        int alreadyCleared = numTrue(classic, timeTrial, endless);

        if (alreadyCleared == 3)
        {
            return;
        }

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

        int newCleared = numTrue(classic, timeTrial, endless);

        if (alreadyCleared == newCleared)
        {
            return;
        }

        getAchievement(ref modesComplete, newCleared - alreadyCleared);
    }

    public void achievementsAchievement()
    {
        if (youreGoodAtThis.state == 3)
        {
            return;
        }

        int[] achieveList = { noneWrong.state , getStars.state , classicDifficulty.state , timeDifficulty.state , endlessDifficulty.state ,
                              classicTimed.state , endlessStreak.state , modesComplete.state , dontFollowDirections.state ,
                              youreBadAtThis.state , getDistracted.state , nothingBetterToDo.state };

        int max = 4;

        for (int i = 0; i < 12; i++)
        {
            if (achieveList[i] < max)
            {
                max = achieveList[i];
            }
        }

        getAchievement(ref youreGoodAtThis, max - youreGoodAtThis.state);
    }

    public void resetAchievements()
    {
        noneWrong.state = 0;
        PlayerPrefs.SetInt(noneWrong.privateName + SetPrefabs.name, 0);
        getStars.state = 0;
        PlayerPrefs.SetInt(getStars.privateName + SetPrefabs.name, 0);
        classicDifficulty.state = 0;
        PlayerPrefs.SetInt(classicDifficulty.privateName + SetPrefabs.name, 0);
        timeDifficulty.state = 0;
        PlayerPrefs.SetInt(timeDifficulty.privateName + SetPrefabs.name, 0);
        endlessDifficulty.state = 0;
        PlayerPrefs.SetInt(endlessDifficulty.privateName + SetPrefabs.name, 0);
        classicTimed.state = 0;
        PlayerPrefs.SetInt(classicTimed.privateName + SetPrefabs.name, 0);
        endlessStreak.state = 0;
        PlayerPrefs.SetInt(endlessStreak.privateName + SetPrefabs.name, 0);
        modesComplete.state = 0;
        PlayerPrefs.SetInt(modesComplete.privateName + SetPrefabs.name, 0);
        dontFollowDirections.state = 0;
        PlayerPrefs.SetInt(dontFollowDirections.privateName + SetPrefabs.name, 0);
        youreBadAtThis.state = 0;
        PlayerPrefs.SetInt(youreBadAtThis.privateName + SetPrefabs.name, 0);
        getDistracted.state = 0;
        PlayerPrefs.SetInt(getDistracted.privateName + SetPrefabs.name, 0);
        nothingBetterToDo.state = 0;
        PlayerPrefs.SetInt(nothingBetterToDo.privateName + SetPrefabs.name, 0);
        youreGoodAtThis.state = 0;
        PlayerPrefs.SetInt(youreGoodAtThis.privateName + SetPrefabs.name, 0);

        PlayerPrefs.SetInt("classicAchieve_" + SetPrefabs.name, false ? 1 : 0);
        classic = false;

        PlayerPrefs.SetInt("timeTrialAchieve_" + SetPrefabs.name, false ? 1 : 0);
        timeTrial = false;

        PlayerPrefs.SetInt("endlessAchieve_" + SetPrefabs.name, false ? 1 : 0);
        endless = false;

        Instance = null;
        Destroy(this);
    }
}
