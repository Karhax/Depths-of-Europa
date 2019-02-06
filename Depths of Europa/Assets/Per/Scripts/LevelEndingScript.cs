using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEndingScript : MonoBehaviour {

    [SerializeField] private GameObject _goalBase = null;

    [SerializeField] private GameObject _fadeObject = null;
    [SerializeField] private float _fadeDuration = 1f;
    [SerializeField] [Range(0.1f, 20f)] private float _fadeDelay = 0.5f;
    [SerializeField] private bool _useFadeDelayTimer = true;

    // [SerializeField] private DialogSystem _dialogToUse = null;
    // [SerializeField] private SoundObject _goalReachedSound = null;

    private Timer _timerFadeDelay;
    private SpriteRenderer _fadeSprite;
    private bool _playerDetected = false;
    private bool _fadeStarted = false;
    
	private void Start () {
        _timerFadeDelay = new Timer(_fadeDelay);
        if (_fadeObject == null)
        {
            throw new System.Exception("A LevelEndHandler could not find any FadeObject");
        }
        else
        {
            _fadeSprite = _fadeObject.GetComponent<SpriteRenderer>();
            if (_fadeSprite == null)
            {
                throw new System.Exception("The FadeObject does not have a SpriteRenderer");
            }
        }
	}
	
	private void Update () {
        if (_playerDetected)
        {
            if (_useFadeDelayTimer && !_fadeStarted)
            {
                _timerFadeDelay.Time += Time.deltaTime;
                if (_timerFadeDelay.Expired())
                {
                    _fadeStarted = true;
                }
            }
            else
            {
                if (_fadeDuration <= 0)
                {
                    _fadeObject.SetActive(true);
                    _fadeSprite.color = new Color(255, 255, 255, 255);
                }
            }
        }
	}
}
