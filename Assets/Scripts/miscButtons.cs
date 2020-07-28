using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditorInternal;

// UI button interactions
public class miscButtons : MonoBehaviour
{
    // Sound sources
    AudioSource musicSource;
    AudioSource sfxSource;

    // Typewriter sound
    public AudioClip typekey;

    // SFX volume
    public float volume;

    // Assign Music and SFX objects to AudioSources
    private void Start()
    {
        musicSource = Music.Instance.musicSource;
        sfxSource = SoundManager.Instance.audioSource;
    }

    // Transition to Intro scene, for changing users
    public void toIntro()
    {
        if (!sfxSource.isPlaying)
        {
            sfxSource.PlayOneShot(typekey, volume);
        }
        musicSource.Pause();
        SceneManager.LoadScene("Intro");
    }

    // Transition to Classic Mode setup screen
    public void toClassic()
    {
        if (!sfxSource.isPlaying)
        {
            sfxSource.PlayOneShot(typekey, volume);
        }
        SceneManager.LoadScene("Classic Select");
    }

    // Transition to Time Trial Mode setup screen
    public void toTimeTrial()
    {
        if (!sfxSource.isPlaying)
        {
            sfxSource.PlayOneShot(typekey, volume);
        }
        SceneManager.LoadScene("Time Select");
    }

    // Transition to Endless Mode setup screen
    public void toEndless()
    {
        if (!sfxSource.isPlaying)
        {
            sfxSource.PlayOneShot(typekey, volume);
        }
        SceneManager.LoadScene("Endless Select");
    }

    // Transition to Tutorial
    public void toTutorial()
    {
        if (!sfxSource.isPlaying)
        {
            sfxSource.PlayOneShot(typekey, volume);
        }
        SceneManager.LoadScene("Tutorial");
    }

    // Transition to Settings screen
    public void toSettings()
    {
        if (!sfxSource.isPlaying)
        {
            sfxSource.PlayOneShot(typekey, volume);
        }
        SceneManager.LoadScene("Settings");
    }


    // Transition to Achievements screen
    public void toAchievements()
    {
        if (!sfxSource.isPlaying)
        {
            sfxSource.PlayOneShot(typekey, volume);
        }
        SceneManager.LoadScene("Achievements");
    }

    public void toShop()
    {
        if (!sfxSource.isPlaying)
        {
            sfxSource.PlayOneShot(typekey, volume);
        }
        SceneManager.LoadScene("Shop");
    }

    // Transition to Achievements screen
    public void toAchieveP2()
    {
        if (!sfxSource.isPlaying)
        {
            sfxSource.PlayOneShot(typekey, volume);
        }
        SceneManager.LoadScene("Achieve P2");
    }

    // Transition to About screen
    public void toAbout()
    {
        if (!sfxSource.isPlaying)
        {
            sfxSource.PlayOneShot(typekey, volume);
        }
        SceneManager.LoadScene("About");
    }

    // Transition to game
    public void toMain()
    {
        if (!sfxSource.isPlaying)
        {
            sfxSource.PlayOneShot(typekey, volume);
        }
        musicSource.Pause();
        SceneManager.LoadScene("Flanker Main");
    }

    // Transition to title screen
    public void backToTitle()
    {
        if (!sfxSource.isPlaying)
        {
            sfxSource.PlayOneShot(typekey, volume);
        }
        if (musicSource.isPlaying == false)
        {
            musicSource.UnPause();
        }
        SceneManager.LoadScene("Title");
    }

    public void resetUser()
    {
        AchievementManager.Instance.resetAchievements();
        PlayerPrefs.SetInt("starScore_" + stateManager.Instance.playerName, 0);

        PlayerPrefs.SetFloat("allBestTime_" + stateManager.Instance.playerName, float.NaN);
        PlayerPrefs.SetFloat("allWorstTime_" + stateManager.Instance.playerName, float.NaN);
        PlayerPrefs.SetFloat("allBestAvg_" + stateManager.Instance.playerName, float.NaN);
        PlayerPrefs.SetFloat("allWorstAvg_" + stateManager.Instance.playerName, float.NaN);
        PlayerPrefs.SetFloat("allBestCongAvg_" + stateManager.Instance.playerName, float.NaN);
        PlayerPrefs.SetFloat("allBestIncongAvg_" + stateManager.Instance.playerName, float.NaN);
        PlayerPrefs.SetFloat("allWorstCongAvg_" + stateManager.Instance.playerName, float.NaN);
        PlayerPrefs.SetFloat("allWorstIncongAvg_" + stateManager.Instance.playerName, float.NaN);
        PlayerPrefs.SetFloat("allBestCong_" + stateManager.Instance.playerName, float.NaN);
        PlayerPrefs.SetFloat("allBestIncong_" + stateManager.Instance.playerName, float.NaN);
        PlayerPrefs.SetFloat("allWorstCong_" + stateManager.Instance.playerName, float.NaN);
        PlayerPrefs.SetFloat("allWorstIncong_" + stateManager.Instance.playerName, float.NaN);
        PlayerPrefs.SetFloat("allBestFlanker_" + stateManager.Instance.playerName, float.NaN);
        PlayerPrefs.SetFloat("allWorstFlanker_" + stateManager.Instance.playerName, float.NaN);

        AchievementManager.Instance.loadAchievements();

        tutorialGate.Instance.setFalse();

        SceneManager.LoadScene("Title");
    }
}
