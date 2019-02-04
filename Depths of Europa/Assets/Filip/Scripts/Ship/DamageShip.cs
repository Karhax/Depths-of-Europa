﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageShip : MonoBehaviour
{
    [Header("Settings")]

    [SerializeField, Range(0, 250)] int _maxHp;
    [SerializeField, Range(0, 25)] int _minimumCollisionDamage;
    [SerializeField, Range(0, 100)] float _collisionDamageModifier;
    [SerializeField, Range(0, 10)] float _healthBarShowDuration;

    [Header("Drop")]

    [SerializeField] Slider _healthBar;

    int _currentHp;

    private void Awake()
    {
        _currentHp = _maxHp;
        _healthBar.maxValue = _maxHp;
        _healthBar.value = _healthBar.maxValue;
        _healthBar.gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        TakeDamage(collision.relativeVelocity.magnitude);
    }

    private void TakeDamage(float crashForce)
    {
        int damage = (int)(crashForce * _collisionDamageModifier);

        if (damage >= _minimumCollisionDamage)
        {
            _currentHp -= damage;

            _healthBar.gameObject.SetActive(true);
            _healthBar.value = _currentHp;
            StartCoroutine(RemoveHealthBar());

            CheckIfDead();
        }
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

    IEnumerator RemoveHealthBar()
    {
        Timer timer = new Timer(_healthBarShowDuration);

        while (!timer.Expired())
        {
            timer.Time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        _healthBar.gameObject.SetActive(false);
    }
}