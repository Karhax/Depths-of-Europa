using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShakeObject
{
    RectTransform rectTransform;
    Vector3 originalPosition;
    Vector3 newPosition = Vector3.zero;

    bool canShake = false;

    public ShakeObject(RectTransform rectTransform)
    {
        this.rectTransform = rectTransform;

        originalPosition = this.rectTransform.localPosition;
        canShake = true;
    }

    public void SetNewPosition(Vector3 newPosition, float strength, bool smooth = false, float _smoothSpeed = 0)
    {
        if (canShake)
        {
            if (smooth)
            {
                this.newPosition = originalPosition + newPosition * strength;
                SmoothMove(_smoothSpeed);
            }
            else
                rectTransform.localPosition = originalPosition + newPosition * strength * Time.deltaTime;
        }
    }

    public void SmoothMove(float _smoothSpeed)
    {
        if (canShake)
            rectTransform.localPosition = Vector2.MoveTowards(rectTransform.position, newPosition, Time.deltaTime * _smoothSpeed);
    }

    public void Revert()
    {
        rectTransform.localPosition = originalPosition;
    }
}

[CreateAssetMenu(fileName = "New DialogEffectShake", menuName = "Dialog/Effect/Dialog Shake")]
public class DialogEffectShake : DialogEffectBase
{
    [SerializeField] bool _shakeTextBox = false;
    [SerializeField] bool _shakeBackground = true;
    [SerializeField] bool _shakeRightImage = true;
    [SerializeField] bool _shakeLeftImage = true;

    [SerializeField] bool _doNotShakeHorizontal = false;
    [SerializeField] bool _doNotShakeVertical = false;
    [SerializeField] float _shakeSpeed;
    [SerializeField] int _ignoreFrames = 10;

    [SerializeField] bool _smooth = false;
    [SerializeField] float _smoothSpeed = 10f;

    List<ShakeObject> _shakeables = new List<ShakeObject>();

    int _currentIgnoreFrames = 0;

    public override void SetUpEffect(Dialog dialogScript)
    {
        _currentIgnoreFrames = _ignoreFrames;

        if (_doNotShakeHorizontal && _doNotShakeVertical)
            Debug.LogWarning("There will be no shake because both vertical and horizontal movement is disabled!", this);

        AddShakeable(dialogScript.DialogBox, _shakeTextBox);
        AddShakeable(dialogScript.BackgroundImage, _shakeBackground);
        AddShakeable(dialogScript.RightImage, _shakeRightImage);
        AddShakeable(dialogScript.LeftImage, _shakeLeftImage);

        if (!(_shakeTextBox && _shakeBackground && _shakeLeftImage && _shakeRightImage))
            Debug.LogWarning("There will be no shake because no element is set to shake!");
    }

    private void AddShakeable(Component component, bool shouldAdd)
    {
        if (component != null && shouldAdd)
            _shakeables.Add(new ShakeObject(component.GetComponent<RectTransform>()));
    }

    public override void UpdateEffect()
    {
        if (_currentIgnoreFrames >= _ignoreFrames)
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
        foreach (ShakeObject shakeObject in _shakeables)
        {
            shakeObject.Revert();
        }
    }
}
