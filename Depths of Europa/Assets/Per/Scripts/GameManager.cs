using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public delegate void FadeDelegate(bool startingFade);
    public static event FadeDelegate FadeEvent;
    
    private static GameManager singletonGameManager = null;
    private static FadeHandler _fadeHandler = null;
    private static GameObject _shipObject = null;
    private static GameObject _camera = null;
    private static MainMusicParent _mainMusicParent = null;
    private static GameOverStingerHandler _gameOverStinger = null;

    private static string _nextScene = "Main Menu";

    public static GameObject ShipObject { get { return _shipObject; } }
    public static GameObject CameraObject { get { return _camera; } }

    private void Awake()
    {
        if (singletonGameManager == null)
        {
            singletonGameManager = this;

            _shipObject = GameObject.FindGameObjectWithTag(Statics.Tags.PLAYER_OUTSIDE);
            if (_shipObject == null)
                Debug.LogWarning("Game Manager could not find any object with tag " + Statics.Tags.PLAYER_OUTSIDE);    

            _camera = Camera.main.gameObject;
            if (_camera == null)
                Debug.LogWarning("Game Manager could not find camera!");
        }
        else
        {
            Destroy(this.gameObject);
        }

        _gameOverStinger = _mainMusicParent.gameObject.GetComponentInChildren<GameOverStingerHandler>();
        if (_gameOverStinger == null)
        {
            Debug.LogWarning("The GameManager did not have any GameOverStingerHandler");
        }
    }

	private void Start ()
    {
        _fadeHandler = FindObjectOfType<FadeHandler>();
        if (_fadeHandler == null)
        {
            Debug.LogWarning("The GameManager could not find any object that has a FadeHandler");
        }
        else
        {
            _fadeHandler.FadeEnded += BeginningFadeDone;
            _fadeHandler.StartFadeIn();
            if (FadeEvent != null)
            {
                FadeEvent(true);
            }
        }

        _mainMusicParent = FindObjectOfType<MainMusicParent>();
        if (_mainMusicParent == null)
        {
            Debug.LogWarning("The GameManager could not find any object that has a MainMusicParent");
        }
    }

    public static void DialogStartedReaction()
    {
        if (_mainMusicParent != null)
        {
            _mainMusicParent.ActivateBaseMusic();
        }
    }

    public static void LevelEndReached(string sceneName)
    {
        _nextScene = sceneName;
        _fadeHandler.FadeEnded += EndingFadeOutDone;
        _fadeHandler.FadeEnded -= RestartFadeOutDone;
        _fadeHandler.FadeEnded -= BeginningFadeDone;
        _fadeHandler.StartFadeOut();
        if (FadeEvent != null)
        {
            FadeEvent(true);
        }
        if (_mainMusicParent != null)
        {
            _mainMusicParent.EndBaseMusic();
        }
    }

    private static void EndingFadeOutDone()
    {
        _fadeHandler.FadeEnded -= EndingFadeOutDone;
        SceneManager.LoadScene(_nextScene);
    }

    private static void BeginningFadeDone()
    {
        _fadeHandler.FadeEnded -= BeginningFadeDone;
        if (FadeEvent != null)
        {
            FadeEvent(false);
        }
    }

    public static void PlayerKilledReaction()
    {
        if (_mainMusicParent != null)
        {
            _mainMusicParent.StopAllMusic();
        }
        if (_gameOverStinger != null)
        {
            _gameOverStinger.TriggerGameOverStinger();
        }
    }

    public static void LevelRestartRequested()
    {
        _fadeHandler.FadeEnded += RestartFadeOutDone;
        _fadeHandler.FadeEnded -= BeginningFadeDone;
        _fadeHandler.FadeEnded -= EndingFadeOutDone;
        _fadeHandler.StartFadeOut();
        if (FadeEvent != null)
        {
            FadeEvent(true);
        }
        if (_mainMusicParent != null)
        {
            _mainMusicParent.StopAllMusic();
        }
    }

    private static void RestartFadeOutDone()
    {
        _fadeHandler.FadeEnded -= RestartFadeOutDone;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnDisable()
    {
        if (_fadeHandler != null)
        {
            _fadeHandler.FadeEnded -= BeginningFadeDone;
            _fadeHandler.FadeEnded -= RestartFadeOutDone;
            _fadeHandler.FadeEnded -= EndingFadeOutDone;
        }
    }
}
