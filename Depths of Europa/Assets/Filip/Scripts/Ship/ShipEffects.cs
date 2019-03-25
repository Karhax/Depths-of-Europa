using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Statics;

public class ShipEffects : MonoBehaviour
{
    [Header("Settings")]

    [SerializeField, Range(0, 10)] float _animationSpeedModifier;
    [SerializeField, Range(0, 10)] float _minAnimationSpeed;
    [SerializeField, Range(0, 10)] float _maxAnimationSpeed;

    [SerializeField, Range(0, 500)] float _amountOfParticlesModifier = 35f;
    [SerializeField, Range(0, 50)] float _minAmountOfParticles = 1f;

    [SerializeField, Range(0, 3)] float _minHitForceOnIceToPlayParticles = 0.5f;

    [Header("Drop")]

    [SerializeField] ParticleSystem _bubblesParticleSystem;
    [SerializeField] Animator _animator;
    [SerializeField] GameObject _hitIceParticleSystem;

    Rigidbody2D _thisRigidbody;
    ParticleSystem.EmissionModule _particleSystemEmission;

    private void Awake()
    {
        _thisRigidbody = GetComponent<Rigidbody2D>();
        _particleSystemEmission = _bubblesParticleSystem.emission;
    }

    bool _doParticles = true;

    private void Update()
    {
        float dotProduct = Vector3.Dot(_thisRigidbody.velocity, transform.up);
        SetAnimation();

        if (_doParticles)
            AmountOfBubbles(dotProduct);
    }

    public void TurnOffEngine()
    {
        _particleSystemEmission.rateOverTime = new ParticleSystem.MinMaxCurve(1, 1);
        _doParticles = false;
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
            amount = transform.InverseTransformVector(_thisRigidbody.velocity).y * _amountOfParticlesModifier;
            amount = amount < _minAmountOfParticles ? _minAmountOfParticles : amount;
        }

        _particleSystemEmission.rateOverTime = new ParticleSystem.MinMaxCurve(amount, amount);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag(Tags.ICE) && collision.relativeVelocity.magnitude > _minHitForceOnIceToPlayParticles)
            SpawnIceParticles(collision);
    }

    private void SpawnIceParticles(Collision2D collision)
    {
        ContactPoint2D[] contacts = new ContactPoint2D[1];
        collision.GetContacts(contacts);
        Vector3 spawnPoint = contacts[0].point;

        Instantiate(_hitIceParticleSystem, spawnPoint, Quaternion.identity, transform);
    }
}
