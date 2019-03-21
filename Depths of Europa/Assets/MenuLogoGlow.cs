using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuLogoGlow : MonoBehaviour
{
    [SerializeField, Range(0.1f, 100)] float _speed = 5;
    [SerializeField] Gradient gradient;

    Material _material;

    bool _lerpUp = true;
    Timer _timer;

    private void Awake()
    {
        _timer = new Timer(_speed);
        _material = GetComponent<UnityEngine.UI.Image>().material;
    }

    private void Update()
    {
        _timer.Time += Time.deltaTime;

        LerpColor();
    }

    private void LerpColor()
    {
        _material.SetColor("_EmissionColor", gradient.Evaluate(_timer.Ratio()));

        if (_timer.Expired())
            _timer.Reset();
    }
}
