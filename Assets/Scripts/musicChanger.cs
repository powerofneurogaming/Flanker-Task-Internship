// Unity libraries
using UnityEngine;

// Single-fire script for changing the music in the Music Source singleton
public class musicChanger : MonoBehaviour
{
    // Music to be played
    [SerializeField]
    AudioClip mainBGM;

    // On-start function to change and start music
    void Start()
    {
        Music.Instance.musicSource.clip = mainBGM;
        if(!Music.Instance.musicSource.isPlaying)
        {
            Music.Instance.musicSource.Play(0);
        }
    }
}
