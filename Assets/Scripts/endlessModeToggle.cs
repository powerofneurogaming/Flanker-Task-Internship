using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// THIS FILE IS TEMPORARY UNTIL THE ENDLESS MODE SETUP SCENE IS IMPLEMENTED
public class endlessModeToggle : MonoBehaviour
{
    public Toggle endlessToggle;
    public InputField levelSelect;

    public static bool endlessMode;

    void Start()
    {
    }

    // Toggles Endless Mode on or off based on checkbox
    public void onToggle()
    {
        if (endlessToggle.isOn)
        {
            endlessMode = true;
            levelSelect.interactable = false;
        }
        else
        {
            endlessMode = false;
            levelSelect.interactable = true;
        }
    }
}
