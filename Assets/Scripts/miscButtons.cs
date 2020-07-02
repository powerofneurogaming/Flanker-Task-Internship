using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class miscButtons : MonoBehaviour
{
    public AudioSource source;

    public void toIntro()
    {
        source.Stop();
        SceneManager.LoadScene("Intro");
    }

    public void toAbout()
    {
        source.Stop();
        SceneManager.LoadScene("How to Play");
    }

    public void toSettings()
    {
        source.Stop();
        SceneManager.LoadScene("Settings");
    }

    public void toMain()
    {
        source.Stop();
        SceneManager.LoadScene("Flanker Main");
    }

    public void backToTitle()
    {
        source.Stop();
        SceneManager.LoadScene("Title");
    }

    public void settingsBackToTitle()
    {
        // For now this is identical to backToTitle()
        // Eventually it will contain getters and setters for saving settings.
        // TODO: actually make the settings menu
        source.Stop();
        SceneManager.LoadScene("Title");
    }
}
