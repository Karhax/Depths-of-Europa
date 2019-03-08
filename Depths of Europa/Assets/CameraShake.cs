using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraShake : MonoBehaviour
{
    [SerializeField, Range(0.1f, 20)] float _shakeLength = 1;
    [SerializeField, Range(0.1f, 150)] float _maxShakeModifier = 10;
    [SerializeField, Range(0, 100)] float _maxDamageShake = 20f;

    Camera _camera;
    float _currentShakeModifier;

    private void Start()
    {
        _camera = GameManager.CameraObject.GetComponent<Camera>();
    }

    public void DoShake(float damageDone)
    {
        float ratio = damageDone / _maxDamageShake;
        _currentShakeModifier = ratio * _maxShakeModifier;
        StartCoroutine(Shake(ratio));
    }

    IEnumerator Shake(float ratio)
    {
        Timer shakeTimer = new Timer(_shakeLength);

        while (!shakeTimer.Expired())
        {
            shakeTimer.Time += Time.deltaTime;
            _camera.transform.position += (Vector3)Random.insideUnitCircle * Time.deltaTime * _currentShakeModifier;

            _currentShakeModifier = _maxShakeModifier * (1 - shakeTimer.Ratio()) * ratio;

            yield return new WaitForEndOfFrame();
        }
    }
}
