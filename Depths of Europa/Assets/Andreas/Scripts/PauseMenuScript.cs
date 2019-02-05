using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Statics;

public class PauseMenuScript : MonoBehaviour {


    [SerializeField] bool _isPaused = false;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
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
            Time.timeScale = 0;
        }
        else if(!_isPaused)
        {
            Time.timeScale = 1;
        }
    }
}
