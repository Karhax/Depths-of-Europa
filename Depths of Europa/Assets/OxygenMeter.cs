using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OxygenMeter : MonoBehaviour
{
    [SerializeField] bool _use = false;
    [SerializeField, Range(0, 2000)] int _length = 200;
    [SerializeField] Image _meter;
    [SerializeField] Text _text;
    [SerializeField] Gradient _meterGradient;
    [SerializeField] Gradient _textGradient;

    float _currentTime;

    private void Awake()
    {
        if (!_use)
            Destroy(gameObject);
        else
            _currentTime = _length;
    }

    private void Update()
    {
        _currentTime -= Time.deltaTime;

        _meter.fillAmount = _currentTime / _length;
        _text.text = ((int)_currentTime + 1).ToString();

        _meter.color = _meterGradient.Evaluate(1 - (_currentTime / _length));
        _text.color = _textGradient.Evaluate(1 - (_currentTime / _length));

        if (_currentTime <= 0)
        {
            GameManager.LevelRestartRequested();
            Destroy(gameObject);
        }
    }
}
