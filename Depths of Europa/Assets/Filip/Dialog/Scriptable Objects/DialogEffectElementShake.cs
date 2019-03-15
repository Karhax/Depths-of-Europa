using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DialogShakeElements
{
    TEXT_BOX,
    BACKGROUND,
    LEFT_CHARACTER,
    RIGHT_CHARACTER
}

[CreateAssetMenu(fileName = "New DialogEffectElementShake", menuName = "Dialog/Effect/Element Shake")]
public class DialogEffectElementShake : DialogEffectOneTime
{
    [SerializeField] bool _shakeEntireDialog = false;
    [SerializeField] float _shakeTime;

    [SerializeField] DialogShakeElements _shakeElement;
    [SerializeField] bool _doNotShakeHorizontal = false;
    [SerializeField] bool _doNotShakeVertical = false;
    [SerializeField] float _shakeSpeed;
    [SerializeField] int _ignoreFrames = 10;

    ShakeObject _shakeObject;
    int _currentIgnoreFrames = 0;
    Timer _shakeTimer;

    public override void SetUpEffect(Dialog dialogScript)
    {
        if (_shakeTime <= 0)
            _shakeTime = 0.1f;

        _shakeTimer = new Timer(_shakeTime);

        _currentIgnoreFrames = _ignoreFrames;

        switch (_shakeElement)
        {
            case (DialogShakeElements.TEXT_BOX):        { SetShakeObject(dialogScript.DialogText);      break; }
            case (DialogShakeElements.BACKGROUND):      { SetShakeObject(dialogScript.BackgroundImage); break; }
            case (DialogShakeElements.RIGHT_CHARACTER): { SetShakeObject(dialogScript.RightImage);      break; }
            case (DialogShakeElements.LEFT_CHARACTER):  { SetShakeObject(dialogScript.LeftImage);       break; }
        }

        if (_shakeObject == null)
            Debug.LogWarning("There is no rectransform for this object!");
    }
    
    private void SetShakeObject(Component component)
    {
        if (component != null)
            _shakeObject = new ShakeObject(component.GetComponent<RectTransform>());   
    }

    public override void UpdateEffect()
    {
        if (!_shakeEntireDialog)
            _shakeTimer.Time += Time.deltaTime;

        if (!_shakeTimer.Expired())
        {
            if (_currentIgnoreFrames >= _ignoreFrames)
            {
                _currentIgnoreFrames = 0;

                Vector2 newPosition = Random.insideUnitCircle;

                if (_doNotShakeHorizontal)
                    newPosition.x = 0;
                if (_doNotShakeVertical)
                    newPosition.y = 0;

                _shakeObject.SetNewPosition(newPosition, _shakeSpeed);

            }

            _currentIgnoreFrames++;
        }
        else
            ResetEffect();
    }

    public override void ResetEffect()
    {
        _shakeObject.Revert();
    }
}