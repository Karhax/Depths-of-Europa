using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonarPulse : MonoBehaviour
{
    [SerializeField, Range(0, 15)] float _speed = 1;
    [SerializeField, Range(0, 5)] float _colorFadeSpeed = 0.15f;

    SpriteRenderer _spriteRenderer;
    Color _currentColor;

    bool _play = false;

    private void Update()
    {
        if (_play)
        {
            transform.localScale += Vector3.one * Time.deltaTime * _speed;

            _currentColor.a -= Time.deltaTime * _colorFadeSpeed;
            _spriteRenderer.color = _currentColor;

            if (_currentColor.a <= 0)
                Destroy(gameObject);
        } 
    }

    public void SetAlpha(float newAlpha)
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _currentColor = _spriteRenderer.color;

        _currentColor.a = newAlpha;
        _play = true;
    }
}
