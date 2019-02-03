using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageShip : MonoBehaviour
{
    [SerializeField, Range(0, 250)] int _maxHp;
    [SerializeField, Range(0, 100)] float _collisionDamageModifier;

    int _currentHp;

    private void Awake()
    {
        _currentHp = _maxHp;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _currentHp -= (int)(collision.relativeVelocity.magnitude * _collisionDamageModifier);
        CheckIfDead();
    }

    private void CheckIfDead()
    {
        if (_currentHp <= 0)
            Die();
    }

    private void Die()
    {
        Debug.Log("Dead");
    }
}
