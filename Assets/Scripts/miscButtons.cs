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
        source.Stop();
        SceneManager.LoadScene("Intro");
    }

    public void toAbout()
    {
        if (!sfxSource.isPlaying)
        {
            sfxSource.PlayOneShot(typekey, volume);
        }
        source.Stop();
        SceneManager.LoadScene("How to Play");
    }

    public void toSettings()
    {
        if (!sfxSource.isPlaying)
        {
            sfxSource.PlayOneShot(typekey, volume);
        }
        SceneManager.LoadScene("Settings");
    }

    public void toMain()
    {
        if (!sfxSource.isPlaying)
        {
            sfxSource.PlayOneShot(typekey, volume);
        }
        source.Stop();
        SceneManager.LoadScene("Flanker Main");
    }

    public void backToTitle()
    {
            sfxSource.PlayOneShot(typekey, volume);
        if (source.isPlaying == false)
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
        if (!sfxSource.isPlaying)
        {
            sfxSource.PlayOneShot(typekey, volume);
        }
        SceneManager.LoadScene("Title");
    }
}
