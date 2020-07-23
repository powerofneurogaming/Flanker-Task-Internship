using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class settings : MonoBehaviour
{
    [SerializeField]
    Slider musicSetter;
    [SerializeField]
    Text musicText;

    [SerializeField]
    Slider sfxSetter;
    [SerializeField]
    Text sfxText;

    // Start is called before the first frame update
    void Start()
    {
        musicSetter.value = stateManager.Instance.music_volume * 100;
        musicText.text = "Music: " + musicSetter.value + "%";

        sfxSetter.value = stateManager.Instance.sfx_volume * 100;
        sfxText.text = "Sound: " + sfxSetter.value + "%";
    }

    // Update is called once per frame
    public void setMusicVol()
    {
        stateManager.Instance.music_volume = musicSetter.value / 100;
        Music.Instance.setVolume(musicSetter.value / 100);
        musicText.text = "Music: " + musicSetter.value + "%";
    }

    // Update is called once per frame
    public void setSFXVol()
    {
        stateManager.Instance.sfx_volume = sfxSetter.value / 100;
        SoundManager.Instance.setVolume(sfxSetter.value / 100);
        sfxText.text = "Sound: " + sfxSetter.value + "%";
    }

    public void saveSettings()
    {
        PlayerPrefs.SetFloat("musicVol", musicSetter.value / 100);
        PlayerPrefs.SetFloat("sfxVol", sfxSetter.value / 100);
    }
}
