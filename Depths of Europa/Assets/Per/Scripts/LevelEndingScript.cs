using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEndingScript : MonoBehaviour {

    [SerializeField] private GameObject _goalBase = null;
    [SerializeField] private string _nextScene;
 
    [SerializeField] private GameObject _fadeObject = null;
    [SerializeField] private float _fadeDuration = 1f;
    [SerializeField] [Range(0.1f, 20f)] private float _fadeDelay = 0.5f;
    [SerializeField] private bool _useFadeDelayTimer = true;

    // [SerializeField] private DialogSystem _dialogToUse = null;
    // [SerializeField] private SoundObject _goalReachedSound = null;

    private Timer _timerFadeDelay;
    private SpriteRenderer _fadeSpriteRenderer;
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
            _fadeSpriteRenderer = _fadeObject.GetComponent<SpriteRenderer>();
            if (_fadeSpriteRenderer == null)
            {
                throw new System.Exception("The FadeObject does not have an Image component");
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
                    _fadeSpriteRenderer.color = new Color(1, 1, 1, 1);
                }
                else
                {
                    float nextAlpha = _fadeSpriteRenderer.color.a + (1 / _fadeDuration * Time.deltaTime);
                    _fadeSpriteRenderer.color = new Color(1, 1, 1, nextAlpha);
                }
            }
            if(_fadeSpriteRenderer.color.a >= 1)
            {
                _playerDetected = false;
                BeginEndingDialog();
            }
        }
	}

    private void BeginEndingDialog()
    {
        _playerDetected = false;
        // Call the beginning of the connected dialog system
        // For testing purposes:
        EndLevel();
    }
    public void EndLevel()
    {
        // Function that the connected dialog system should call to end the level.
        // Probably need to send a reference to this function when calling the dialog system.
        SceneManager.LoadScene(_nextScene);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(Statics.Tags.PLAYER_OUTSIDE))
        {
            _playerDetected = true;

            // Call function in the player object that stops movement and disables input

            // Call function in the base object that plays the docking animation

            // Play sound here, or perhaps in the function EndLevel()
        }
    }
}
