using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Statics;

public class DamageShip : MonoBehaviour
{
    public delegate void ShipTakeDamage(float value);
    public event ShipTakeDamage ShipTakeDamageEvent;

    [Header("Settings")]

    [SerializeField, Range(0, 100)] float _highMassCollision = 10;
    [SerializeField, Range(0, 100)] float _leastHighMassVelocity = 2f;
    [SerializeField, Range(0, 20)] float _highMassCollisionModifier = 2.5f;

    [SerializeField, Range(0, 50)] float _minCollisionMassForDamage = 0.1f;
    [SerializeField, Range(0, 250)] int _maxHp;
    [SerializeField, Range(0, 25)] int _minimumCollisionDamage;
    [SerializeField, Range(0, 100)] float _collisionDamageModifier;
    [SerializeField, Range(0, 10)] float _waitAfterDeathTilRestart = 2f;

    [Header("Drop")]

    [SerializeField] AudioSource _deathAudio;
    [SerializeField] ParticleSystem _deathParticles;

    int _currentHp;

    bool _isDead = false;

    private void Awake()
    {
        _currentHp = _maxHp;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.transform.CompareTag(Tags.ENEMY) && !(collision.gameObject.layer == LayerMask.NameToLayer(Layers.FISH_UNDER)) || collision.transform.CompareTag(Tags.BASE))
        {
            Rigidbody2D collisionRigidBody = collision.transform.GetComponent<Rigidbody2D>();

            if (collisionRigidBody != null)
            {
                if (collisionRigidBody.mass > _minCollisionMassForDamage)
                    TakeDamage(collision.relativeVelocity.magnitude, collisionRigidBody);
            }
        }
            
    }

    public void RecoverDamage(int damageToRecover)
    {
        _currentHp = _currentHp + damageToRecover > _maxHp ? _maxHp : _currentHp + damageToRecover;

        if (ShipTakeDamageEvent != null)
            ShipTakeDamageEvent.Invoke((float)_currentHp / _maxHp);
    }
    public void Hit(int damage)
    {
        GetComponent<CameraShake>().DoShake(damage);

        _currentHp -= damage;

        if (ShipTakeDamageEvent != null)
            ShipTakeDamageEvent.Invoke((float)_currentHp / _maxHp);

        CheckIfDead();
    }

    private void TakeDamage(float crashForce, Rigidbody2D collisionRigidBody)
    {
        int damage = (int)(crashForce * _collisionDamageModifier);

        if (collisionRigidBody.mass > _highMassCollision && collisionRigidBody.velocity.magnitude > _leastHighMassVelocity)
            damage = (int)(damage * _highMassCollisionModifier);

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

        GameManager.PlayerKilledReaction();
        _deathAudio.Play();
        _deathParticles.Play();

        ShipInBase turnOffShip = GetComponent<ShipInBase>();

        if (turnOffShip != null)
            turnOffShip.InBase(true, Vector3.zero);

        StartCoroutine(Restart());
    }

    IEnumerator Restart()
    {
        yield return new WaitForSeconds(_waitAfterDeathTilRestart);
        GameManager.LevelRestartRequested();
    }
}
