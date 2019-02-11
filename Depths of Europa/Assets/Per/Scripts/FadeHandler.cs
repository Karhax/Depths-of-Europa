using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeHandler : MonoBehaviour {

    public delegate void DelegateFadeEnded();
    public event DelegateFadeEnded FadeEnded;

    [SerializeField] private GameObject _fadeObject = null;
    [SerializeField] private float _fadeDuration = 1f;

    private SpriteRenderer _fadeSpriteRenderer;

    private bool _fadeStarted = false;
    private int _directionMultiplier = 1;

    private void Awake ()
    {
        if (_fadeObject == null)
        {
            throw new System.Exception("The FadeHandler could not find any FadeObject");
        }
        else
        {
            _fadeSpriteRenderer = _fadeObject.GetComponent<SpriteRenderer>();
            if (_fadeSpriteRenderer == null)
            {
                throw new System.Exception("The FadeObject does not have a SpriteRenderer component");
            }
        }
    }

	private void Update ()
    {
        if (_fadeStarted)
        {
            if (_fadeDuration <= 0)
            {
                _fadeSpriteRenderer.color = new Color(1, 1, 1, _directionMultiplier);
            }
            else
            {
                float nextAlpha = _fadeSpriteRenderer.color.a + (_directionMultiplier / _fadeDuration * Time.deltaTime);
                _fadeSpriteRenderer.color = new Color(1, 1, 1, nextAlpha);
            }

            if (_fadeSpriteRenderer.color.a >= 1 && _directionMultiplier > 0)
            {
                _fadeStarted = false;
                if (FadeEnded != null)
                {
                    FadeEnded();
                }
            }
            else if (_fadeSpriteRenderer.color.a <= 0 && _directionMultiplier < 0)
            {
                _fadeStarted = false;
                if (FadeEnded != null)
                {
                    FadeEnded();
                }
            }
        }
    }

    public void StartFadeIn()
    {
        _fadeStarted = true;
        _directionMultiplier = -1;
        _fadeSpriteRenderer.color = new Color(1, 1, 1, 1);
    }
    public void StartFadeOut()
    {
        _fadeStarted = true;
        _directionMultiplier = 1;
        _fadeSpriteRenderer.color = new Color(1, 1, 1, 0);
    }
}
