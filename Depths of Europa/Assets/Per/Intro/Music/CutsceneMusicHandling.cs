using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneMusicHandling : MonoBehaviour {
    
    private AudioSource _musicPlayer = null;
    private bool _fadeOutActive = false;
    private float _changePerSecond = 0.2f;

	private void Awake () {
        _musicPlayer = gameObject.GetComponent<AudioSource>();
        if (_musicPlayer == null)
        {
            Debug.LogWarning("The object " + gameObject.ToString() + " has a CutsceneMusicHandling script but no AudioSource");
        }
	}
	
	private void Update () {
		if (_fadeOutActive && _musicPlayer != null)
        {
            _musicPlayer.volume -= _changePerSecond * Time.deltaTime;
            if (_musicPlayer.volume <= 0)
            {
                _musicPlayer.volume = 0;
                _fadeOutActive = false;
            }
        }
	}

    public void StartMusic()
    {
        if (_musicPlayer != null)
        {
            _musicPlayer.Play();
        }
    }

    public void StartFadeOut(float fadeDuration = 1)
    {
        if (fadeDuration <= 0)
        {
            _changePerSecond = 1f;
        }
        else if (fadeDuration >= 10)
        {
            _changePerSecond = 0.1f;
        }
        else
        {
            _changePerSecond = 1.0f / fadeDuration;
        }

        _fadeOutActive = true;
    }
}
