﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Statics;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour {

    bool _isPaused = false, _isFading = false;
    [SerializeField] GameObject _pauseMenuBackdrop;
    [SerializeField] string _mainMenuName;

    public delegate void PauseStateHandler(bool pauseState);

    public event PauseStateHandler PauseState;

    bool _tutorialPause = false;

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
            if(_pauseMenuBackdrop != null)
                _pauseMenuBackdrop.SetActive(true);

            Time.timeScale = 0;

            if (PauseState != null)
                PauseState.Invoke(_isPaused);
        }
        else if(!_isPaused)
        {
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
