using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneSkipScript : MonoBehaviour {

    [SerializeField] private string _skipBoolName = "SkipTriggered";
    private Animator _cutsceneAnimator = null;
    
	void Start () {
        _cutsceneAnimator = gameObject.GetComponent<Animator>();
        if (_cutsceneAnimator == null)
        {
            Debug.LogWarning("The object " + gameObject.ToString() + " has a CutsceneSkipScript but no Animator");
        }
	}
	
	void Update () {
		if (_cutsceneAnimator != null)
        {
            if (Input.GetAxisRaw(Statics.GameInput.SKIP_DIALOG) > 0)
            {
                _cutsceneAnimator.SetBool(_skipBoolName, true);
            }
        }
	}
}
