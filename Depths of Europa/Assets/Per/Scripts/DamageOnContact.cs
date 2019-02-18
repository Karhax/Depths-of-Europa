using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnContact : MonoBehaviour {

    [SerializeField] [Range(1, 50)] private int _damage = 1;
    private DamageShip _shipHealth = null;

    private Timer _cooldownTimer = null;
    [SerializeField] [Range(0.1f, 5f)] private float _damageCooldown = 1;

    private void Awake()
    {
        _cooldownTimer = new Timer(_damageCooldown, _damageCooldown);
    }

    private void Update()
    {
        if (!_cooldownTimer.Expired())
        {
            _cooldownTimer.Time += Time.deltaTime;
        }
        else if (_shipHealth != null)
        {
            DealDamage();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(Statics.Tags.PLAYER_OUTSIDE))
        {
            _shipHealth = other.gameObject.GetComponent<DamageShip>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(Statics.Tags.PLAYER_OUTSIDE))
        {
            _shipHealth = null;
        }
    }

    private void DealDamage()
    {
        if (_shipHealth != null)
        {
            _shipHealth.Hit(_damage);
            _cooldownTimer.Reset();
        }
    }
}
