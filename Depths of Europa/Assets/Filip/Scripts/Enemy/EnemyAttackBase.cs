using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Statics;

[System.Serializable]
public class EnemyAttackBase : EnemyStateBase
{
    [SerializeField, Range(0, 15)] float _attackSpeed;
    [SerializeField, Range(0, 10)] float _durationToAttackOutOfSight;
    [SerializeField, Range(0, 5)] float _turnSmootheness;
    [SerializeField, Range(0, 10)] float _turnSpeed;
    [SerializeField, Range(0, 15)] float _lookRange;
    [SerializeField, Range(0, 50)] float _dodgeSpeed;

    Transform _playerShip;

    Vector2 _attackDirection;
    Timer _attackTimer;

    int _divertDirection;
    bool _doTimer = false;

    public override void SetUp(BasicEnemy0 script)
    {
        _attackTimer = new Timer(_durationToAttackOutOfSight);
        base.SetUp(script);
    }

    public override void EnterState()
    {
        _divertDirection = Random.Range(-1, 1);
        if (_divertDirection == 0)
            _divertDirection = 1;

        if (_playerShip == null)
            _playerShip = GameObject.FindGameObjectWithTag(Tags.PLAYER_OUTSIDE).transform;

        Attack();
    }

    public override void ExitState()
    {
        _attackTimer.Reset();
    }

    public override EnemyStates FixedUpdate()
    {
        if (_attackDirection != (Vector2)thisTransform.right)
            thisTransform.right = Vector2.MoveTowards(thisTransform.right, _thisRigidbody.velocity, Time.deltaTime * _turnSpeed);

        _thisRigidbody.velocity = Vector2.Lerp(_thisRigidbody.velocity, _attackDirection, Time.deltaTime * _turnSmootheness);

        RaycastHit2D hit = Physics2D.BoxCast(thisTransform.position, BOX_CAST_BOX, 0, thisTransform.right, _lookRange, LayerMask.GetMask(Layers.DEFAULT));

        if (hit.collider != null)
            Divert();
        else
            Attack();

        if (_doTimer)
        {
            _attackTimer.Time += Time.deltaTime;

            if (_attackTimer.Expired())
                return EnemyStates.IDLE;
        }

        return EnemyStates.STAY;
    }

    public override EnemyStates OnTriggerEnter(Collider2D other)
    {
        if (other.CompareTag(Tags.LIGHT))
            return EnemyStates.ESCAPE;

        return EnemyStates.STAY;
    }

    public override EnemyStates OnTriggerExit(Collider2D other)
    {
        return EnemyStates.STAY;
    }

    private void Attack()
    {
        Attack(_playerShip.position);
    }

    private void Attack(Vector2 position)
    {
        _attackDirection = (position - (Vector2)thisTransform.position).normalized * _attackSpeed;
    }

    private void Divert()
    {
        _attackDirection = (_attackDirection + _divertDirection * (Vector2)thisTransform.up * Time.deltaTime * _dodgeSpeed).normalized * _attackSpeed;
    }
}
