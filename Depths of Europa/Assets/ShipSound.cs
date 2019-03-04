using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Statics;

public class ShipSound : MonoBehaviour
{
    [Header("Settings")]

    [SerializeField, Range(0, 5)] float _dynamicSoundModifier = 1;
    [SerializeField, Range(0, 5)] float _stressedSoundModifier = 1;

    [SerializeField, Range(0, 1)] float _maxEngineDynamicVolume = 1;
    [SerializeField, Range(0, 1)] float _maxEngineStressedVolume = 1;
    [SerializeField, Range(0, 1)] float _stressedSoundStart = 0.5f;

    [SerializeField, Range(1, 10)] float _collisionSoundStrengthModifier = 3f;

    [Header("Drop")]

    [SerializeField] AudioSource _engineDynamic;
    [SerializeField] AudioSource _engineStressed;

    [SerializeField] AudioSource _hitMetalAudio;
    [SerializeField] AudioSource _hitIceAudio;
    [SerializeField] AudioSource _hitFishAudio;

    Rigidbody2D _thisRigidBody;
    float _shipMaxSpeed;
    float _startStressedSpeed;

    private void Awake()
    {
        _thisRigidBody = GetComponent<Rigidbody2D>();
        _shipMaxSpeed = GetComponent<MoveShip>().GetMaxShipSpeed();
        _startStressedSpeed = _shipMaxSpeed * _stressedSoundStart;
    }

    private void Update()
    {
        float newDynamicVolume = _thisRigidBody.velocity.magnitude / _shipMaxSpeed * _dynamicSoundModifier;
        _engineDynamic.volume = newDynamicVolume < _maxEngineDynamicVolume ? newDynamicVolume : _maxEngineDynamicVolume;

        if (_thisRigidBody.velocity.magnitude > _startStressedSpeed)
        {
            float newStressedVolume = (_thisRigidBody.velocity.magnitude - _startStressedSpeed) / (_shipMaxSpeed - _startStressedSpeed) * _stressedSoundModifier;
            _engineStressed.volume = newStressedVolume < _maxEngineStressedVolume ? newStressedVolume : _maxEngineStressedVolume;
        }
        else
            _engineStressed.volume = 0;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        float collisionStrength = other.relativeVelocity.magnitude;

        if (other.transform.CompareTag(Tags.BASE))
            StartCollisionAudio(_hitMetalAudio, collisionStrength);
        else if (other.transform.CompareTag(Tags.ENEMY))
            StartCollisionAudio(_hitFishAudio, collisionStrength);
        else if (other.transform.CompareTag(Tags.ICE))
            StartCollisionAudio(_hitIceAudio, collisionStrength);
    }

    private void StartCollisionAudio(AudioSource source, float strength)
    {
        if (!source.isPlaying)
        {
            source.volume = strength / _collisionSoundStrengthModifier;
            source.Play();
        }
    }
}
