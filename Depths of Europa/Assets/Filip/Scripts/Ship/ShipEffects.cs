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

    [SerializeField] ParticleSystem _bubblesParticleSystem;
    [SerializeField] Animator _animator;

    Rigidbody2D _thisRigidbody;
    ParticleSystem.EmissionModule _particleSystemEmission;

    private void Awake()
    {
        _thisRigidbody = GetComponent<Rigidbody2D>();
        _particleSystemEmission = _bubblesParticleSystem.emission;
    }

    private void Update()
    {
        float dotProduct = Vector3.Dot(_thisRigidbody.velocity, transform.up);

        SetAnimation();
        AmountOfBubbles(dotProduct);
    }

    private void SetAnimation()
    {
        float currentAnimationSpeed = _thisRigidbody.velocity.magnitude * _animationSpeedModifier;
        _animator.speed = currentAnimationSpeed > _minAnimationSpeed ? currentAnimationSpeed : _minAnimationSpeed;
    }

    private void AmountOfBubbles(float dotProduct)
    {
        float amount = 1;

        if (dotProduct >= 0)
        {
            amount = _thisRigidbody.velocity.magnitude * _amountOfParticlesModifier;
            amount = amount < _minAmountOfParticles ? _minAmountOfParticles : amount;
           
        }

        _particleSystemEmission.rateOverTime = new ParticleSystem.MinMaxCurve(amount, amount);
    }
}
