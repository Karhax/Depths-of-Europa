using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Statics;

public class Tentacle : MonoBehaviour
{
    [SerializeField, Range(0, 100)] int _damage = 15;
    [SerializeField, Range(0, 100)] int _variancy = 5;

    [SerializeField, Range(0.1f, 10f)] float _waitTilNextDamage = 1.5f;

    bool _canAttack = true;

    private void Awake()
    {
        _damage += Random.Range(-_variancy, _variancy);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        DamageShip damageShip = collision.transform.GetComponent<DamageShip>();

        if (damageShip != null && _canAttack)
        {
            damageShip.Hit(_damage);
            StartCoroutine(Wait());
        }    
    }

    IEnumerator Wait()
    {
        _canAttack = false;
        yield return new WaitForSeconds(_waitTilNextDamage);
        _canAttack = true;
    }
}
