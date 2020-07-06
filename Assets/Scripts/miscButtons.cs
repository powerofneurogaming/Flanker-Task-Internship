using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class miscButtons : MonoBehaviour
{
    AudioSource source;
    AudioSource sfxSource;

    public AudioClip typekey;
    public float volume;

    private void Start()
    {
        source = Music.Instance.musicSource;
        sfxSource = SoundManager.Instance.audioSource;
    }

    public void toIntro()
    {
        if (!sfxSource.isPlaying)
        {
            sfxSource.PlayOneShot(typekey, volume);
        }
        source.Pause();
        SceneManager.LoadScene("Intro");
    }

    public void toClassic()
    {
        if (!sfxSource.isPlaying)
        {
            sfxSource.PlayOneShot(typekey, volume);
        }
        source.Pause();
        SceneManager.LoadScene("Classic Select");
    }

    public void toTimeTrial()
    {
        if (!sfxSource.isPlaying)
        {
            sfxSource.PlayOneShot(typekey, volume);
        }
        source.Pause();
        SceneManager.LoadScene("Time Select");
    }

    public void toTutorial()
    {
        if (!sfxSource.isPlaying)
        {
            sfxSource.PlayOneShot(typekey, volume);
        }
        source.Pause();
        SceneManager.LoadScene("Tutorial");
    }

    public void toSettings()
    {
        if (!sfxSource.isPlaying)
        {
            sfxSource.PlayOneShot(typekey, volume);
        }
        SceneManager.LoadScene("Settings");
    }

    public void toAbout()
    {
        if (!sfxSource.isPlaying)
        {
            sfxSource.PlayOneShot(typekey, volume);
        }
        SceneManager.LoadScene("About");
    }

    public void toMain()
    {
        if (!sfxSource.isPlaying)
        {
            sfxSource.PlayOneShot(typekey, volume);
        }
        source.Pause();
        SceneManager.LoadScene("Flanker Main");
    }

    public void backToTitle()
    {
        if (!sfxSource.isPlaying)
        {
            sfxSource.PlayOneShot(typekey, volume);
        }
        if (source.isPlaying == false)
        {
            source.UnPause();
        }
        SceneManager.LoadScene("Title");
    }

    public void settingsBackToTitle()
    {
        // For now this is identical to backToTitle()
        // Eventually it will contain getters and setters for saving settings.
        // TODO: actually make the settings menu
        if (!sfxSource.isPlaying)
        {
            sfxSource.PlayOneShot(typekey, volume);
        }
        SceneManager.LoadScene("Title");
    }
}
