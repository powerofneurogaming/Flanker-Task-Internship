using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Single-fire script for changing the music in the Music Source singleton
public class musicChanger : MonoBehaviour
{
    [SerializeField]
    AudioClip mainBGM;

    void Start()
    {
        Music.Instance.musicSource.clip = mainBGM;
        if(!Music.Instance.musicSource.isPlaying)
        {
            Music.Instance.musicSource.Play(0);
        }
    }
}
