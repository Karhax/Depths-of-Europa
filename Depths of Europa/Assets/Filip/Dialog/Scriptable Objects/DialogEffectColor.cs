using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New DialogEffectColor", menuName = "Dialog/Effect/Color")]
public class DialogEffectColor : DialogEffectBase
{
    [SerializeField] AudioClip _audio;
    [SerializeField] Gradient _gradient;
    [SerializeField] float _changeColorSpeed = 5;

    AudioSource _thisAudioSource;
    Color _startColor;
    Image _backgroundImage;
    bool _firstLerp = true;
    float _placeInGradient = 0;

    bool _play = true;
    readonly float MAKE_COLOR_CHANGE_SPEED_BIGGER_THAN_1 = 10;

    public override void SetUpEffect(Dialog dialogScript)
    {
        _thisAudioSource = dialogScript.EffectAudioSource;

        if (_thisAudioSource != null)
        {
            _thisAudioSource.clip = _audio;
            _thisAudioSource.Play();
        }

        _changeColorSpeed /= MAKE_COLOR_CHANGE_SPEED_BIGGER_THAN_1;
        _backgroundImage = dialogScript.BackgroundImage;

        if (_backgroundImage == null)
        {
            Debug.LogWarning("This Dialog player cannot have this effect!", dialogScript);
            _play = false;
        }
        else
            _startColor = _backgroundImage.color;
    }

    public override void UpdateEffect()
    {
        if (_play)
        {
            _backgroundImage.color = _gradient.Evaluate(_placeInGradient);

            if (_firstLerp)
                _placeInGradient += Time.deltaTime * _changeColorSpeed;
            else
                _placeInGradient -= Time.deltaTime * _changeColorSpeed;

            if (_placeInGradient > 1)
                _firstLerp = false;
            else if (_placeInGradient < 0)
                _firstLerp = true;
        }
    }

    public override void ResetEffect()
    {
        if (_thisAudioSource != null)
            _thisAudioSource.Stop();

        if (_play)
            _backgroundImage.color = _startColor;
    }

    private void ChangeColor(Color newColor)
    {
        _backgroundImage.color = Color.Lerp(_backgroundImage.color, newColor, Time.deltaTime * _changeColorSpeed);
    }

    private bool HasReachedColor(Color checkColor)
    {
        return _backgroundImage.color == checkColor;
    }
}
