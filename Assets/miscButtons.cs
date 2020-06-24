﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class miscButtons : MonoBehaviour
{
    public Button backButton;

    public void toIntro()
    {
        SceneManager.LoadScene("Intro");
    }

    public void toAbout()
    {
        SceneManager.LoadScene("About");
    }

    public void toSettings()
    {
        SceneManager.LoadScene("Settings");
    }

    public void backToTitle()
    {
        SceneManager.LoadScene("Title");
    }

    public void settingsBackToTitle()
    {
        // For now this is identical to backToTitle()
        // Eventually it will contain getters and setters for saving settings.
        // TODO: actually make the settings menu
        SceneManager.LoadScene("Title");
    }
}