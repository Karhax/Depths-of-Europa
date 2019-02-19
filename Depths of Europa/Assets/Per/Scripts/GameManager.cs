using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    
    private static GameManager singletonGameManager = null;
    private FadeHandler _fadeHandler = null;
    private LevelEndingScript _levelEnder = null;
    private DamageShip _damageShip = null;
    private static GameObject _shipObject = null;
    private static GameObject _camera = null;

    private string _nextScene = "Main Menu";

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
            else
            {
                _damageShip = _shipObject.GetComponent<DamageShip>();
                _damageShip.DieEvent += LevelRestartRequested;
            }     

            _camera = Camera.main.gameObject;
            if (_camera == null)
                Debug.LogWarning("Game Manager could not find camera!");
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void OnEnable()
    {
        if (_levelEnder != null)
        {
            _levelEnder.LevelEndingDetected += LevelEndReached;
        }
        if (_damageShip != null)
        {
            _damageShip.DieEvent += LevelRestartRequested;
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
        }

        _levelEnder = FindObjectOfType<LevelEndingScript>();
        if (_levelEnder == null)
        {
            Debug.LogWarning("The GameManager could not find any object that has a LevelEndingScript");
        }
        else
        {
            _levelEnder.LevelEndingDetected += LevelEndReached;
        }

    }

    public void LevelEndReached(string sceneName)
    {
        _nextScene = sceneName;
        _fadeHandler.FadeEnded += EndingFadeOutDone;
        _fadeHandler.FadeEnded -= RestartFadeOutDone;
        _fadeHandler.FadeEnded -= BeginningFadeDone;
        _fadeHandler.StartFadeOut();
    }

    public void EndingFadeOutDone()
    {
        _fadeHandler.FadeEnded -= EndingFadeOutDone;
        SceneManager.LoadScene(_nextScene);
    }

    public void BeginningFadeDone()
    {
        _fadeHandler.FadeEnded -= BeginningFadeDone;
    }

    public void LevelRestartRequested()
    {
        _fadeHandler.FadeEnded += RestartFadeOutDone;
        _fadeHandler.FadeEnded -= BeginningFadeDone;
        _fadeHandler.FadeEnded -= EndingFadeOutDone;
        _fadeHandler.StartFadeOut();
    }

    public void RestartFadeOutDone()
    {
        _fadeHandler.FadeEnded -= RestartFadeOutDone;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnDisable()
    {
        if (_levelEnder != null)
        {
            _levelEnder.LevelEndingDetected -= LevelEndReached;
        }
        if (_fadeHandler != null)
        {
            _fadeHandler.FadeEnded -= BeginningFadeDone;
            _fadeHandler.FadeEnded -= RestartFadeOutDone;
            _fadeHandler.FadeEnded -= EndingFadeOutDone;
        }
        if (_damageShip != null)
        {
            _damageShip.DieEvent -= LevelRestartRequested;
        }
    }
}
