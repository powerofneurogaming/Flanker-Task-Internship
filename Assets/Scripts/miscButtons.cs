using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class miscButtons : MonoBehaviour
{
    AudioSource source = Music.Instance.musicSource;
    AudioSource sfxSource = SoundManager.Instance.audioSource;

    public AudioClip typekey;
    public float volume;

    public void toIntro()
    {
        source.Stop();
        SceneManager.LoadScene("Intro");
    }

    public void toAbout()
    {
        sfxSource.PlayOneShot(typekey, volume);
        source.Stop();
        SceneManager.LoadScene("How to Play");
    }

    public void toSettings()
    {
        SceneManager.LoadScene("Settings");
    }

    public void toMain()
    {
        source.Stop();
        SceneManager.LoadScene("Flanker Main");
    }

    public void backToTitle()
    {
        if(source.isPlaying == false)
        {
            source.Play();
        }
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
