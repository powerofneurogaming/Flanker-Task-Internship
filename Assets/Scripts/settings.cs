// Unity libraries
using UnityEngine;
using UnityEngine.UI;

// Settings screen logic
public class settings : MonoBehaviour
{
    // Slider for music volume
    [SerializeField]
    Slider musicSetter;
    [SerializeField]
    Text musicText;

    // Slider for SFX volume
    [SerializeField]
    Slider sfxSetter;
    [SerializeField]
    Text sfxText;

    // Toggle for Nighttime BG
    [SerializeField]
    GameObject nightMode;

    // Get existing settings state and set up interface to match
    void Start()
    {
        musicSetter.value = stateManager.Instance.music_volume * 100;
        musicText.text = "Music: " + musicSetter.value + "%";

        sfxSetter.value = stateManager.Instance.sfx_volume * 100;
        sfxText.text = "Sound: " + sfxSetter.value + "%";

        if(stateManager.Instance.nightPurchased == false)
        {
            nightMode.GetComponent<Toggle>().interactable = false;
            nightMode.GetComponentInChildren<Text>().text = "Night Mode\n(LOCKED)";
        }
        nightMode.GetComponent<Toggle>().isOn = stateManager.Instance.nightMode;
    }

    // Music volume setter
    public void setMusicVol()
    {
        stateManager.Instance.music_volume = musicSetter.value / 100;
        Music.Instance.setVolume(musicSetter.value / 100);
        musicText.text = "Music: " + musicSetter.value + "%";
    }

    // SFX volume setter
    public void setSFXVol()
    {
        stateManager.Instance.sfx_volume = sfxSetter.value / 100;
        SoundManager.Instance.setVolume(sfxSetter.value / 100);
        sfxText.text = "Sound: " + sfxSetter.value + "%";
    }

    // Night Mode toggle
    public void setNightMode()
    {
        stateManager.Instance.nightMode = nightMode.GetComponent<Toggle>().isOn;
    }

    // Save settings on menu exit
    public void saveSettings()
    {
        PlayerPrefs.SetFloat("musicVol", musicSetter.value / 100);
        PlayerPrefs.SetFloat("sfxVol", sfxSetter.value / 100);
        PlayerPrefs.SetInt("nightMode_" + stateManager.Instance.nightMode, true ? 1 : 0);
    }
}
