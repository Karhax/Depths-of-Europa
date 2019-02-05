using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Statics;
using UnityEngine.UI;

public class PauseMenuScript : MonoBehaviour {

    [SerializeField] bool _isPaused = false;
    [SerializeField] GameObject _pauseMenuBackdrop;

	
	void Update () {
		if(Input.GetButtonDown(GameInput.Cancel))
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
        }
        else if(!_isPaused)
        {
            if (_pauseMenuBackdrop != null)
                _pauseMenuBackdrop.SetActive(false);
            Time.timeScale = 1;
        }
    }
}
