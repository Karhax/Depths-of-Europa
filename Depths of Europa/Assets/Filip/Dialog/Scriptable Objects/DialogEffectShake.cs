using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShakeObject
{
    RectTransform rectTransform;
    Vector3 originalPosition;
    Vector3 newPosition = Vector3.zero;

    public ShakeObject(RectTransform rectTransform)
    {
        this.rectTransform = rectTransform;
        originalPosition = this.rectTransform.position;
    }

    public void SetNewPosition(Vector3 newPosition, float strength, bool smooth, float _smoothSpeed)
    {
        if (smooth)
        {
            this.newPosition = originalPosition + newPosition * strength;
            SmoothMove(_smoothSpeed);
        }
            
        else
            rectTransform.position = originalPosition + newPosition * strength * Time.deltaTime;
    }

    public void SmoothMove(float _smoothSpeed)
    {
        rectTransform.position = Vector2.MoveTowards(rectTransform.position, newPosition, Time.deltaTime * _smoothSpeed);
    }

    public void Revert()
    {
        rectTransform.position = originalPosition;
    }
}

[CreateAssetMenu(fileName = "New DialogEffectShake", menuName = "Dialog/Effect/Dialog Shake")]
public class DialogEffectShake : DialogEffectBase
{
    [SerializeField] bool _shakeTextBox = false;
    [SerializeField] bool _doNotShakeHorizontal = false;
    [SerializeField] bool _doNotShakeVertical = false;
    [SerializeField] float _shakeSpeed;
    [SerializeField] int ignoreFrames = 10;

    [SerializeField] bool _smooth = false;
    [SerializeField] float _smoothSpeed = 10f;

    List<ShakeObject> _shakeables = new List<ShakeObject>();

    int _currentIgnoreFrames = 0;

    public override void SetUpEffect(Dialog dialogScript)
    {
        _currentIgnoreFrames = ignoreFrames;

        if (_doNotShakeHorizontal && _doNotShakeVertical)
            Debug.LogWarning("There will be no shake because both vertical and horizontal movement is disabled!", this);

        if (_shakeTextBox && dialogScript.DialogBox != null)
            _shakeables.Add(new ShakeObject(dialogScript.DialogBox.GetComponent<RectTransform>()));

        if (dialogScript.BackgroundImage != null)
            _shakeables.Add(new ShakeObject(dialogScript.BackgroundImage.GetComponent<RectTransform>()));

        if (dialogScript.RightImage != null)
            _shakeables.Add(new ShakeObject(dialogScript.RightImage.GetComponent<RectTransform>()));

        if (dialogScript.LeftImage != null)
            _shakeables.Add(new ShakeObject(dialogScript.LeftImage.GetComponent<RectTransform>()));
    }

    public override void UpdateEffect()
    {
        if (_currentIgnoreFrames >= ignoreFrames)
        {
            _currentIgnoreFrames = 0;

            Vector2 newPosition = Random.insideUnitCircle;

            if (_doNotShakeHorizontal)
                newPosition.x = 0;
            if (_doNotShakeVertical)
                newPosition.y = 0;

            foreach (ShakeObject shakeObject in _shakeables)
            {
                shakeObject.SetNewPosition(newPosition, _shakeSpeed, _smooth, _smoothSpeed);
            }
        }
        else if (_smooth)
        {
            foreach (ShakeObject shakeObject in _shakeables)
            {
                shakeObject.SmoothMove(_smoothSpeed);
            }
        }

        _currentIgnoreFrames++;
    }

    public override void ResetEffect()
    {
        foreach(ShakeObject shakeObject in _shakeables)
        {
            shakeObject.Revert();
        }
    }
}
