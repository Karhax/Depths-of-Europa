using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicHandler : MonoBehaviour {

    private static MusicHandler singletonMusicHandler = null;
    private AudioSource _audioSource = null;

    private void Awake()
    {
        if (singletonMusicHandler == null)
        {
            singletonMusicHandler = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

	void Start () {
        _audioSource = gameObject.GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            throw new System.Exception("The Music Handler did not have any Audio Source to play music through");
        }
	}
}
