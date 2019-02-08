﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    
    private static GameManager singletonGameManager = null;
    private FadeHandler _fadeHandler = null;
    private LevelEndingScript _levelEnder = null;
    private MoveShip _shipMovement = null;

    private string _nextScene = "Main Menu";

    private void Awake()
    {
        if (singletonGameManager == null)
        {
            singletonGameManager = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

	private void Start ()
    {
        _fadeHandler = FindObjectOfType<FadeHandler>();
        if (_fadeHandler == null)
        {
            throw new System.Exception("The GameManager could not find any object that has a FadeHandler");
        }
        _fadeHandler.FadeEnded += BeginningFadeDone;
        _fadeHandler.StartFadeIn();

        _levelEnder = FindObjectOfType<LevelEndingScript>();
        if (_levelEnder == null)
        {
            throw new System.Exception("The GameManager could not find any object that has a LevelEndingScript");
        }
        _levelEnder.LevelEndingDetected += LevelEndReached;

        _shipMovement = FindObjectOfType<MoveShip>();
        if(_shipMovement == null)
        {
            throw new System.Exception("The GameManager could not find any object that has a MoveShip component");
        }
	}

    public void LevelEndReached(string sceneName)
    {
        _nextScene = sceneName;
        _fadeHandler.FadeEnded += FadeOutDone;
        _fadeHandler.StartFadeOut();
    }

    public void FadeOutDone()
    {
        _fadeHandler.FadeEnded -= FadeOutDone;
        SceneManager.LoadScene(_nextScene);
    }

    public void BeginningFadeDone()
    {
        _fadeHandler.FadeEnded -= BeginningFadeDone;
        // _shipMovement.EnableInput(); NOT IMPLEMENTED
    }

    private void OnDestroy()
    {
        _levelEnder.LevelEndingDetected -= LevelEndReached;
    }
}
