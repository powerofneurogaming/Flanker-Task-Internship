// System libraries
using System.Collections.Generic;
using System.Linq;

// Unity libraries
using UnityEngine;

// Script to handle state of all achievements
public class AchievementManager : MonoBehaviour
{
    // Returns number of 'true's in boolean array
    public int numTrue(params bool[] bools)
    {
        return bools.Count(n => n);
    }

    // Singleton
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

    // List of all achievements
    public List<Achievement> achievementList;

    // State sentinels for which modes have been played
    bool classic;
    bool timeTrial;
    bool endless;

    public void loadAchievements()
    {
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
        achievementList.Add(new Achievement("noneWrong_", "None Wrong (Classic Mode)"));

        // Achievement: Get a certain amount of stars
        // Bronze: 50 stars
        // Silver: 100 stars
        // Gold: 200 stars
        achievementList.Add(new Achievement("getStars_", "Get Stars"));

        // Achievement: Complete each difficulty on classic mode
        // Bronze: Complete easy
        // Silver: Complete medium
        // Gold: Complete hard
        achievementList.Add(new Achievement("classicDifficulty_", "Cleared Mode (Classic Mode)"));

        // Achievement: Complete each difficulty on time trial mode
        // Bronze: Complete 10q20s
        // Silver: Complete 20q40s
        // Gold: Complete 30q60s
        achievementList.Add(new Achievement("timeDifficulty_", "Cleared Mode (Time Trial Mode)"));

        // Achievement: Complete each difficulty on endless mode
        // Bronze: Complete regenerative
        // Silver: Complete 3 wrong
        // Gold: Complete 1 wrong
        achievementList.Add(new Achievement("endlessDifficulty_", "Cleared Mode (Endless Mode)"));

        // Achievement: Clear a Classic Mode game in a certain amount of time
        // Bronze: 10 seconds per question
        // Silver: 5 seconds per question
        // Gold: 2 seconds per question
        achievementList.Add(new Achievement("classicTimed_", "Timed Clear (Classic Mode)"));

        // Achievement: Last for a given number of questions in Endless Mode
        // Bronze: 10 questions
        // Silver: 20 questions
        // Gold: 50 questions
        achievementList.Add(new Achievement("endlessStreak_", "Streak (Endless Mode)"));

        // Achievement: Complete each game mode at least once
        // Bronze: 1 mode
        // Silver: 2 modes
        // Gold: 3 modes
        achievementList.Add(new Achievement("modesComplete_", "Modes Completed"));

        // Achievement: Click the wrong hand in the tutorial a given number of times
        // Bronze: 1 time
        // Silver: 2 times
        // Gold: 3 times
        achievementList.Add(new Achievement("dontFollowDirections_", "Wrong Hand (Tutorial)"));

        // Achievement: Finish a game with no questions right
        // Gold: 1 game
        achievementList.Add(new Achievement("youreBadAtThis_", "No Questions Right"));

        // Achievement: Stand Idle at the results screen
        // Gold: 3 minutes
        achievementList.Add(new Achievement("nothingBetterToDo_", "Stay at Results Screen"));

        // Achievement: Get all achievements
        // Bronze: All bronze or better
        // Silver: All silver or better
        // Gold: All gold or better
        achievementList.Add(new Achievement("youreGoodAtThis_", "Get Achievements"));

        // Get states for whether gamemodes have been played (for 'Complete each game mode at least once')
        classic = PlayerPrefs.GetInt("classicAchieve_" + stateManager.Instance.playerName, 0) == 1 ? true : false;
        timeTrial = PlayerPrefs.GetInt("timeTrialAchieve_" + stateManager.Instance.playerName, 0) == 1 ? true : false;
        endless = PlayerPrefs.GetInt("endlessAchieve_" + stateManager.Instance.playerName, 0) == 1 ? true : false;
    }

    // Unlocks/upgrades an achievement by a given number of levels
    public void getAchievement(Achievement achievement, int toAdd)
    {
        // If not upgrading the achievement or it is already at gold level, do nothing
        if (achievement.state >= 3 || toAdd == 0)
        {
            return;
        }

        // Level of achievement unlocked, for debug output
        string type;

        // Upgrade/unlock achievement
        achievement.state += toAdd;

        // Force achievement down to gold level if somehow past
        if (achievement.state > 3)
        {
            achievement.state = 3;
        }

        // Set debug output to proper achievement type
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

        // Save achievement
        PlayerPrefs.SetInt(achievement.privateName + stateManager.Instance.playerName, achievement.state);

        // Debug output
        Debug.Log("You got: " + achievement.friendlyName + " - " + type);
    }

    // getAchievement wrapper for 'Complete each game mode at least once'
    public void completeGamemode(int gameMode)
    {
        // get the number of gamemodes already cleared
        int alreadyCleared = numTrue(classic, timeTrial, endless);

        // If all gamemodes have already been cleared, player already has the achievement; return
        if (alreadyCleared == 3)
        {
            return;
        }

        // Set current gamemode flag to true
        if (gameMode == 0)
        {
            PlayerPrefs.SetInt("classicAchieve_" + stateManager.Instance.playerName, true ? 1 : 0);
            classic = true;
        }
        else if (gameMode == 1)
        {
            PlayerPrefs.SetInt("timeTrialAchieve_" + stateManager.Instance.playerName, true ? 1 : 0);
            timeTrial = true;
        }
        else if (gameMode == 2)
        {
            PlayerPrefs.SetInt("endlessAchieve_" + stateManager.Instance.playerName, true ? 1 : 0);
            endless = true;
        }

        // Get the new number of cleared gamemodes
        int newCleared = numTrue(classic, timeTrial, endless);

        // If both numbers are the same, no new achievement unlocked; return
        if (alreadyCleared == newCleared)
        {
            return;
        }

        // Else, upgrade achievement
        getAchievement(achievementList[7], 1);
    }

    // getAchievement wrapper for 'Get all achievements'
    public void achievementsAchievement()
    {
        // If already at gold level, return
        if (achievementList[11].state == 3)
        {
            return;
        }

        // create array of all other achievement states
        int[] achieveList = { achievementList[0].state , achievementList[1].state , achievementList[2].state , achievementList[3].state , achievementList[4].state ,
                              achievementList[5].state , achievementList[6].state , achievementList[7].state , achievementList[8].state ,
                              achievementList[9].state , achievementList[10].state };

        // flag for lowest achievement level
        int max = 4;

        // set max to the lowest level of all other achievements
        for (int i = 0; i < 11; i++)
        {
            if (achieveList[i] < max)
            {
                max = achieveList[i];
            }
        }

        // if max and 'Get all achievements' state are the same, no new levels; return
        if (max == achievementList[11].state)
        {
            return;
        }

        // upgrade 'Get all achievements' by the difference between the current level and the lowest achievement level
        getAchievement(achievementList[11], max - achievementList[11].state);
    }

    // Resets all achievements and state variables as part of reset user function
    public void resetAchievements()
    {
        // Reset achievements
        achievementList[0].state = 0;
        PlayerPrefs.SetInt(achievementList[0].privateName + stateManager.Instance.playerName, 0);
        achievementList[1].state = 0;
        PlayerPrefs.SetInt(achievementList[1].privateName + stateManager.Instance.playerName, 0);
        achievementList[2].state = 0;
        PlayerPrefs.SetInt(achievementList[2].privateName + stateManager.Instance.playerName, 0);
        achievementList[3].state = 0;
        PlayerPrefs.SetInt(achievementList[3].privateName + stateManager.Instance.playerName, 0);
        achievementList[4].state = 0;
        PlayerPrefs.SetInt(achievementList[4].privateName + stateManager.Instance.playerName, 0);
        achievementList[5].state = 0;
        PlayerPrefs.SetInt(achievementList[5].privateName + stateManager.Instance.playerName, 0);
        achievementList[6].state = 0;
        PlayerPrefs.SetInt(achievementList[6].privateName + stateManager.Instance.playerName, 0);
        achievementList[7].state = 0;
        PlayerPrefs.SetInt(achievementList[7].privateName + stateManager.Instance.playerName, 0);
        achievementList[8].state = 0;
        PlayerPrefs.SetInt(achievementList[8].privateName + stateManager.Instance.playerName, 0);
        achievementList[9].state = 0;
        PlayerPrefs.SetInt(achievementList[9].privateName + stateManager.Instance.playerName, 0);
        achievementList[10].state = 0;
        PlayerPrefs.SetInt(achievementList[10].privateName + stateManager.Instance.playerName, 0);
        achievementList[11].state = 0;
        PlayerPrefs.SetInt(achievementList[11].privateName + stateManager.Instance.playerName, 0);

        // Reset flags for played gamemodes
        PlayerPrefs.SetInt("classicAchieve_" + stateManager.Instance.playerName, false ? 1 : 0);
        classic = false;
        PlayerPrefs.SetInt("timeTrialAchieve_" + stateManager.Instance.playerName, false ? 1 : 0);
        timeTrial = false;
        PlayerPrefs.SetInt("endlessAchieve_" + stateManager.Instance.playerName, false ? 1 : 0);
        endless = false;
    }
}
