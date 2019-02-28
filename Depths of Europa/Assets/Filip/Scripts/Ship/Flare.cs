using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flare : MonoBehaviour
{
    [Header("Settings")]

    [SerializeField, Range(1, 50)] float _flareTime;

    [Header("Drop")]

    [SerializeField] AudioSource _loopAudio;
    [SerializeField] ParticleSystem _bubbleParticleSystem;
    [SerializeField] ParticleSystem _flareParticleSystem;
    [SerializeField] CircleCollider2D _detection;
    [SerializeField] Light[] _lights;

    ParticleSystem.EmissionModule _bubbleEmission;
    ParticleSystem.EmissionModule _flareEmission;

    Timer _flareTimer;
    float[] _lightsMaxRanges;
    float _bubbleParticlesStartEmission;
    float _flareParticleStartEmission;

    private void Awake()
    {
        _bubbleEmission = _bubbleParticleSystem.emission;
        _flareEmission = _flareParticleSystem.emission;

        _bubbleParticlesStartEmission = _bubbleEmission.rateOverTimeMultiplier;
        _flareParticleStartEmission = _flareEmission.rateOverTimeMultiplier;
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

        if (_flareTimer.Expired())
            RemoveFlare();
        else
            ChangeHue();
	}

    private void ChangeHue()
    {
        float ratio = 1 - _flareTimer.Ratio();

        for (int i = 0; i < _lights.Length; i++)
        {
            _lights[i].range = ratio * _lightsMaxRanges[i];
        }

        _loopAudio.volume = ratio;
        transform.localScale = Vector3.one * ratio;
        _flareEmission.rateOverTime = new ParticleSystem.MinMaxCurve(_flareParticleStartEmission * ratio, _flareParticleStartEmission * ratio);
        _bubbleEmission.rateOverTime = new ParticleSystem.MinMaxCurve(_bubbleParticlesStartEmission * ratio, _bubbleParticlesStartEmission * ratio);
    }

    private void RemoveFlare()
    {
        Destroy(gameObject);
    }
}
