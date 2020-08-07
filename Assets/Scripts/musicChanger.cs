// Unity libraries
using UnityEngine;
using UnityEngine.SceneManagement;

// Single-fire script for changing the music in the Music Source singleton
public class musicChanger : MonoBehaviour
{
    // On-start function to change and start music
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Intro" || SceneManager.GetActiveScene().name == "Title")
        {
            Music.Instance.musicSource.clip = Music.Instance.twelveSt;
        }
        if (SceneManager.GetActiveScene().name == "Flanker Main")
        {
            Music.Instance.musicSource.clip = Music.Instance.mapleLeaf;
        }
        if (SceneManager.GetActiveScene().name == "Flanker Result")
        {
            Music.Instance.musicSource.clip = Music.Instance.stormyWeather;
        }

        if (!Music.Instance.musicSource.isPlaying)
        {
            Music.Instance.musicSource.Play(0);
        }
    }
}
