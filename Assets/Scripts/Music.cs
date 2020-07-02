using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
	public AudioClip music;
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.loop = true;
		SoundManager.Instance.audioSource.Play(music, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
