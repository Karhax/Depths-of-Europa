using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneEventHandler : MonoBehaviour {

    [SerializeField] private string _nextSceneName = "Level_1";
    [SerializeField] private string _skipBoolName = "SkipTriggered";
    [SerializeField] private string _skipRequestBoolName = "SkipRequested";
    [SerializeField] private CutsceneMusicHandling _musicHandler = null;
    [SerializeField] [Range(0.1f, 5f)] private float _skipMusicFadeDuration = 2f;

    private Animator _cutsceneAnimator = null;
    
    void Awake ()
    {
        Cursor.visible = false;

        _cutsceneAnimator = gameObject.GetComponent<Animator>();
        if (_cutsceneAnimator == null)
        {
            Debug.LogWarning("The object " + gameObject.ToString() + " has a CutsceneSkipScript but no Animator");
        }

        if (_musicHandler == null)
        {
            Debug.LogWarning("The CutsceneSkipScript in object " + gameObject.ToString() + " does not have any music handler");
        }
    }
	
	void Update ()
    {
        if (_cutsceneAnimator != null)
        {
            // If the skip question is active, see if the player confirms the skip
            if (_cutsceneAnimator.GetBool(_skipRequestBoolName) == true)
            {
                if (Input.GetButtonDown(Statics.GameInput.SKIP_DIALOG))
                {
                    _cutsceneAnimator.SetBool(_skipBoolName, true);
                    if (_musicHandler != null)
                    {
                        _musicHandler.StartFadeOut(_skipMusicFadeDuration);
                    }
                }
            }
            // Else, see if the skip question should be brought up
            else if (Input.GetButtonDown(Statics.GameInput.SKIP_DIALOG))
            {
                _cutsceneAnimator.SetBool(_skipRequestBoolName, true);
            }
        }
    }

    public void CutsceneStartReaction()
    {
        if (_musicHandler != null)
        {
            _musicHandler.StartMusic();
        }
    }
    
    public void DetectEndOfQuestion()
    {
        _cutsceneAnimator.SetBool(_skipRequestBoolName, false);
    }

    public void CutsceneEndReaction()
    {
        Cursor.visible = true;
        SceneManager.LoadScene(_nextSceneName);
    }
}
