using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroTextAlignment : MonoBehaviour {

    private RectTransform _thisTransform = null;
    
	void Start () {
        _thisTransform = gameObject.GetComponent<RectTransform>();

	    if (_thisTransform == null)
        {
            Debug.LogWarning("The object " + gameObject.ToString() + " does not have a RectTransform. IntroTextAlignment will not work.");
        }
        else
        {
            if ((1920f / 1080f) < Camera.main.aspect)
            {
                float targetWidth = (1920f / 1080f) * Camera.main.pixelHeight;
                float move = (Camera.main.pixelWidth - targetWidth) / 2;
                _thisTransform.Translate(move, 0, 0);
            }
        }
	}
}
