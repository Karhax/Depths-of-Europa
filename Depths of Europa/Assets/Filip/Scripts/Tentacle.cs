using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Statics;

public class Tentacle : MonoBehaviour
{
    [SerializeField] string _animationName = "Play";

    [SerializeField, Range(0, 100)] int _damage = 15;
    [SerializeField, Range(0, 100)] int _damageVariancy = 5;

    [SerializeField, Range(0.1f, 10f)] float _waitTilNextDamage = 1.5f;

    bool _canAttack = true;
    Animator _thisAnimator;

    private void Awake()
    {
        _thisAnimator = GetComponent<Animator>();
        _thisAnimator.Play(_animationName, 0, Random.Range(0f, 1f));

        _damage += Random.Range(-_damageVariancy, _damageVariancy);
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
