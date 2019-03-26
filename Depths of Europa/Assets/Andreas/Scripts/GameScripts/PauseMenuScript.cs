using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Statics;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class PauseMenuScript : MonoBehaviour {

    bool _isPaused = false, _isFading = false;
    [SerializeField] GameObject _pauseMenuBackdrop;
    [SerializeField] string _mainMenuName;
    [SerializeField] string _soundAudioEffectName = "Sound_Effect";
    [SerializeField] string _musicAudioEffectName = "Music_Effect";
    [SerializeField] AudioMixer _audioMixer;
    [SerializeField, Range(0, 2000)] float _soundCutOffFreqPause = 275f;
    [SerializeField, Range(0, 2000)] float _musicCutOffFreqPause = 350f;

    readonly float NORMAL_CUTOFF_FREQ_PAUSE = 5000f;

    public delegate void PauseStateHandler(bool pauseState);

    public event PauseStateHandler PauseState;

    bool _tutorialPause = false;
    bool _cursorShouldBeOn = true;

    private void Awake()
    {
        _pauseMenuBackdrop.SetActive(false);
        GameManager.FadeEvent += OnFade;
        Time.timeScale = 1;
    }

    void Update () {
		if(Input.GetButtonDown(GameInput.CANCEL) && !_isFading && !_tutorialPause)
        {
            SetPause();
        }
	}
    //TODO: Probably better to store the start tiem scale somewhere instead of assuming 1
    

    private void OnFade(bool fadeState)
    {
        _isFading = fadeState;
    }

    public void TutorialStart()
    {
        _tutorialPause = true;
        _isPaused = false;
        SetPause();
        _pauseMenuBackdrop.SetActive(false);
    }

    public void TutorialStop()
    {
        _tutorialPause = false;
        _isPaused = true;
        SetPause();
    }

        /// <summary>
        /// Sets the game to paused
        /// </summary>
    private void SetPause()
    {
        _isPaused = !_isPaused;
        if(_isPaused)
        {
            _cursorShouldBeOn = Cursor.visible;
            Cursor.visible = true;

            _audioMixer.SetFloat(_soundAudioEffectName, _soundCutOffFreqPause);
            _audioMixer.SetFloat(_musicAudioEffectName, _musicCutOffFreqPause);

            if (_pauseMenuBackdrop != null)
                _pauseMenuBackdrop.SetActive(true);

            Time.timeScale = 0;

            if (PauseState != null)
                PauseState.Invoke(_isPaused);
        }
        else if(!_isPaused)
        {
            Cursor.visible = _cursorShouldBeOn;

            _audioMixer.SetFloat(_soundAudioEffectName, NORMAL_CUTOFF_FREQ_PAUSE);
            _audioMixer.SetFloat(_musicAudioEffectName, NORMAL_CUTOFF_FREQ_PAUSE);

            if (_pauseMenuBackdrop != null)
                _pauseMenuBackdrop.SetActive(false);

            Time.timeScale = 1;

            if (PauseState != null)
                PauseState.Invoke(_isPaused);
        }
    }
    /// <summary>
    /// Function for resuming the game via the UI
    /// </summary>
    public void ResumeFromGUI()
    {
        SetPause();
    }
    //TODO: Impliment setting menu and settings menu trasition

        /// <summary>
        /// Open settings
        /// </summary>
    public void OpenSettings()
    {
        Debug.Log("Settings menu and settings menu opening not implimented");
    }
    //TODO: Impliment main menu and main menu transition

        /// <summary>
        /// Exiy to menu
        /// </summary>
    public void ExitToMenu()
    {
        GameManager.LevelEndReached(_mainMenuName);
        SetPause();
    }

    public void ExitToDesktop()
    {
        if (Application.isEditor)
        {
            Debug.Log("Not possible to exit to desktop while running in editor");
        }
        else
        {
            Application.Quit();
        }
    }
    public void RestartLevel()
    {
        ResumeFromGUI();
        GameManager.LevelRestartRequested();
    }
}
