using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New DialogEffectColor", menuName = "Dialog/Effect/Color")]
public class DialogEffectColor : DialogEffectBase
{
    [SerializeField] Color _effectColor1 = Color.white;
    [SerializeField] Color _effectColor2 = Color.white;
    [SerializeField] float _changeColorSpeed = 5;

    Color _startColor;
    Image _backgroundImage;
    bool firstLerp = true;

    bool _play = true;

    public override void SetUpEffect(Dialog dialogScript)
    {
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
            if (firstLerp)
            {
                ChangeColor(_effectColor1);
                if (HasReachedColor(_effectColor1))
                    firstLerp = false;
            }
            else
            {
                ChangeColor(_effectColor2);
                if (HasReachedColor(_effectColor2))
                    firstLerp = true;
            }
        }
    }

    public override void ResetEffect()
    {
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

    public override AllDialogEffects GetStopEffect()
    {
        return AllDialogEffects.COLOR;
    }
}
