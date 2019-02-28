using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSound : MonoBehaviour
{
    [Header("Settings")]

    [SerializeField, Range(0, 5)] float _dynamicSoundModifier = 1;
    [SerializeField, Range(0, 5)] float _stressedSoundModifier = 1;

    [SerializeField, Range(0, 1)] float _maxEngineDynamicVolume = 1;
    [SerializeField, Range(0, 1)] float _maxEngineStressedVolume = 1;
    [SerializeField, Range(0, 1)] float _stressedSoundStart = 0.5f;

    [Header("Drop")]

    [SerializeField] AudioSource _engineDynamic;
    [SerializeField] AudioSource _engineStressed;

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
}
