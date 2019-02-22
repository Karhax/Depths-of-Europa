using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipEffects : MonoBehaviour
{
    [Header("Settings")]

    [SerializeField, Range(0, 10)] float _animationSpeedModifier;
    [SerializeField, Range(0, 10)] float _minAnimationSpeed;
    [SerializeField, Range(0, 10)] float _maxAnimationSpeed;

    [SerializeField, Range(0, 500)] float _amountOfParticlesModifier = 35f;
    [SerializeField, Range(0, 50)] float _minAmountOfParticles = 1f;

    [Header("Drop")]



    Rigidbody2D _thisRigidbody;

    private void Awake()
    {
        _thisRigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        SetAnimation();
    }

    private void SetAnimation()
    {
        float currentAnimationSpeed = _thisRigidbody.velocity.magnitude * _animationSpeedModifier;
      //  _animator.speed = currentAnimationSpeed > _minAnimationSpeed ? currentAnimationSpeed : _minAnimationSpeed;
    }
}
