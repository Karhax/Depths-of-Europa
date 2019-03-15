using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverStingerHandler : MonoBehaviour {

    [SerializeField] private bool _useDelay = true;
    [SerializeField] [Range(0.1f, 5f)] private float _delayTime = 0.5f;
    
    private AudioSource _soundPlayer;

	void Awake () {
        _soundPlayer = gameObject.GetComponent<AudioSource>();
        if (_soundPlayer == null)
        {
            Debug.LogWarning("A GameOverStingerHandler did not have any AudioSource");
        }
	}

    public void TriggerGameOverStinger()
    {
        if (_soundPlayer != null)
        {
            if (_useDelay)
            {
                _soundPlayer.PlayDelayed(_delayTime);
            }
            else
            {
                _soundPlayer.Play();
            }
        }
    }
}
