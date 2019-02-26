using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public delegate void StartingFadeDelegate(bool isEnd);
    public static event StartingFadeDelegate StartingFadeEvent;
    
    private static GameManager singletonGameManager = null;
    private static FadeHandler _fadeHandler = null;
    private static GameObject _shipObject = null;
    private static GameObject _camera = null;

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
            if (StartingFadeEvent != null)
            {
                StartingFadeEvent(false);
            }
        }
    }

    public static void LevelEndReached(string sceneName)
    {
        _nextScene = sceneName;
        _fadeHandler.FadeEnded += EndingFadeOutDone;
        _fadeHandler.FadeEnded -= RestartFadeOutDone;
        _fadeHandler.FadeEnded -= BeginningFadeDone;
        _fadeHandler.StartFadeOut();
        if (StartingFadeEvent != null)
        {
            StartingFadeEvent(true);
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
    }

    public static void LevelRestartRequested()
    {
        _fadeHandler.FadeEnded += RestartFadeOutDone;
        _fadeHandler.FadeEnded -= BeginningFadeDone;
        _fadeHandler.FadeEnded -= EndingFadeOutDone;
        _fadeHandler.StartFadeOut();
        if (StartingFadeEvent != null)
        {
            StartingFadeEvent(true);
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
