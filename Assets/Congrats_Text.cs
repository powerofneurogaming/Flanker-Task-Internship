// Unity libraries
using UnityEngine;
using UnityEngine.UI;

public class Congrats_Text : MonoBehaviour
{
    // Congratulations text element
    public Text congrats;

    // Player name
    public static string Player;

    // Setter for Congratulations text. TODO: MOST LIKELY UNNECESSARY. REMOVE?
    void Start()
    {
        Player = PlayerPrefs.GetString("PlayerName");
        if(Player == "NoName")
        {
            congrats.text = "Congratulations!";
        }
        else
        {
            congrats.text = "Congratulations, " + Player + "!"; 
        }
    }
}
