﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Statics;
using UnityEngine.UI;

public class PauseMenuScript : MonoBehaviour {

    bool _isPaused = false;
    [SerializeField] GameObject _pauseMenuBackdrop;

    public delegate void PauseStateHandler(bool pauseState);

    public event PauseStateHandler PauseState;

    void Update () {
		if(Input.GetButtonDown(GameInput.CANCEL))
        {
            SetPause();
        }
	}
    //TODO: Probably better to store the start tiem scale somewhere instead of assuming 1
    
    private void SetPause()
    {
        _isPaused = !_isPaused;
        if(_isPaused)
        {
            if(_pauseMenuBackdrop != null)
                _pauseMenuBackdrop.SetActive(true);

            Time.timeScale = 0;

            if (PauseState != null)
                PauseState(_isPaused);
        }
        else if(!_isPaused)
        {
            if (_pauseMenuBackdrop != null)
                _pauseMenuBackdrop.SetActive(false);

            Time.timeScale = 1;

            if (PauseState != null)
                PauseState(_isPaused);
        }
    }

    public void ResumeFromGUI()
    {
        SetPause();
    }
    //TODO: Impliment setting menu and settings menu trasition
    public void OpenSettings()
    {
        Debug.Log("Settings menu and settings menu opening not implimented");
    }
    //TODO: Impliment main menu and main menu transition
    public void ExitToMenu()
    {
        Debug.Log("Main menu and main menu trasition not implimented");
    }

    public void ExitToDesktop()
    {
        if (Application.isEditor)
        {
            Debug.Log("Not possible to exit to desktop wgile running in editor");
        }
        else
        {
            Application.Quit();
        }
    }
}
