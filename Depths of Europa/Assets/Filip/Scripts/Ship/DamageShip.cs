using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Statics;

public class DamageShip : MonoBehaviour
{
    public delegate void ShipTakeDamage(float value);
    public event ShipTakeDamage ShipTakeDamageEvent;

    [Header("Settings")]

    [SerializeField, Range(0, 250)] int _maxHp;
    [SerializeField, Range(0, 25)] int _minimumCollisionDamage;
    [SerializeField, Range(0, 100)] float _collisionDamageModifier;

    int _currentHp;

    bool _isDead = false;

    private void Awake()
    {
        _currentHp = _maxHp;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.transform.CompareTag(Tags.ENEMY))
            TakeDamage(collision.relativeVelocity.magnitude);
    }

    public void Hit(int damage)
    {
        _currentHp -= damage;

        if (ShipTakeDamageEvent != null)
            ShipTakeDamageEvent.Invoke((float)_currentHp / _maxHp);

        CheckIfDead();
    }

    private void TakeDamage(float crashForce)
    {
        int damage = (int)(crashForce * _collisionDamageModifier);

        if (damage >= _minimumCollisionDamage)
            Hit(damage);
    }

    private void CheckIfDead()
    {
        if (_currentHp <= 0 && !_isDead)
            Die();
    }

    private void Die()
    {
        if (!_isDead)
            _isDead = true;

        GameManager.LevelRestartRequested();
    }
}
