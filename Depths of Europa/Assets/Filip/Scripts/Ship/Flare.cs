﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flare : MonoBehaviour
{
    [Header("Settings")]

    [SerializeField] float _flareTime;
    [Header("Ration between flareTime and toDestroy should be about 0.75")]
    [SerializeField] float _timeToDestroy;

    [Header("Drop")]

    [SerializeField] Light[] _lights;

    Timer _flareTimer;
    float[] _lightsMaxRanges;

    private void Awake()
    {
        _flareTimer = new Timer(_flareTime);

        _lightsMaxRanges = new float[_lights.Length];

        for (int i = 0; i < _lightsMaxRanges.Length; i++)
        {
            _lightsMaxRanges[i] = _lights[i].range;
        }
    }

	void Update ()
    {
        _flareTimer.Time += Time.deltaTime;

        if (_flareTimer.Expired() || _flareTimer.Time >= _timeToDestroy)
            RemoveFlare();
        else
            ChangeHue();
	}

    private void ChangeHue()
    {
        for (int i = 0; i < _lights.Length; i++)
        {
            _lights[i].range = (1 - _flareTimer.Ratio()) * _lightsMaxRanges[i];
        }
    }

    private void RemoveFlare()
    {
        Destroy(gameObject);
    }
}