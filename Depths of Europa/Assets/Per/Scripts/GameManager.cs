using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    [SerializeField] private string _nextScene;

    private static GameManager singletonGameManager = null;
    private FadeHandler _fadeHandler = null;
    private LevelEndingScript _levelEnder = null;
    private MoveShip _shipMovement = null;

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
        // subscribe the function BeginningFadeDone to the fade end event in _fadeHandler
        _fadeHandler.StartFadeIn();

        _levelEnder = FindObjectOfType<LevelEndingScript>();
        if (_levelEnder == null)
        {
            throw new System.Exception("The GameManager could not find any object that has a LevelEndingScript");
        }
        // subscribe the function LevelEndReached to the Level End Reached event in _levelEnder

        _shipMovement = FindObjectOfType<MoveShip>();
        if(_shipMovement == null)
        {
            throw new System.Exception("The GameManager could not find any object that has a MoveShip component");
        }
	}

    public void LevelEndReached()
    {
        _fadeHandler.StartFadeOut();
    }

    public void FadeOutDone()
    {
        SceneManager.LoadScene(_nextScene);
    }

    public void BeginningFadeDone()
    {
        // This function is called from the _fadeHandler when the fade-in that beginns a level has finnished.
        // _shipMovement.EnableInput(); NOT IMPLEMENTED
    }
}
