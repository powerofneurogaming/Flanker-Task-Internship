using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Congrats_Text : MonoBehaviour
{
    public Text congrats;
    string Player;

    // Start is called before the first frame update
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
