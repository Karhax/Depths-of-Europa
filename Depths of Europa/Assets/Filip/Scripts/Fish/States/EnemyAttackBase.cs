using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Statics;

[System.Serializable]
public class EnemyAttackBase : EnemyStateAttackEscapeBase
{
    [SerializeField, Range(0, 10)] float _durationToAttackOutOfSight;    
    [SerializeField, Range(0, 15)] float _avoidRange;
    [SerializeField, Range(0, 15)] float _attackRange;
    [SerializeField, Range(0, 50)] int _damage;

    Timer _attackTimer;

    bool _doTimer = false;
    bool _playerInRange = false;

    public override void SetUp(EnemyBase script, bool noticeByHighSpeed)
    {
        _attackTimer = new Timer(_durationToAttackOutOfSight);
        base.SetUp(script, noticeByHighSpeed);
    }

    public override void EnterState()
    {
        base.EnterState();
        Attack();
        _playerInRange = true;
    }

    public override void ExitState()
    {
        _attackTimer.Reset();
    }

    public override EnemyStates FixedUpdate()
    {
        TurnTowardsTravelDistance(_turnSpeed);
        SetVelocity();

        RaycastHit2D hit = Physics2D.BoxCast(_thisTransform.position, BOX_CAST_BOX, 0, _thisTransform.right, _avoidRange, LayerMask.GetMask(Layers.DEFAULT));

        if (hit.collider != null)
            Divert();
        else
            Attack();

        if (_doTimer)
        {
            _attackTimer.Time += Time.deltaTime;

            if (_attackTimer.Expired() && !_playerInRange)
                return EnemyStates.IDLE;
        }

        return EnemyStates.STAY;
    }

    public override EnemyStates OnTriggerEnter(Collider2D other)
    {
        if (other.CompareTag(Tags.LIGHT))
            return EnemyStates.ESCAPE;
        else if (other.CompareTag(Tags.PLAYER_OUTSIDE))
        {
            HitPlayer();
            return EnemyStates.ESCAPE;
        }    
        else if ((other.CompareTag(Tags.NOTICE_HIGH_SPEED) && _noticeByHighSpeed) || (other.CompareTag(Tags.NOTICE_LOW_SPEED) && !_noticeByHighSpeed))
            _playerInRange = true;

        return EnemyStates.STAY;
    }

    public override EnemyStates OnTriggerExit(Collider2D other)
    {
        if ((other.CompareTag(Tags.NOTICE_HIGH_SPEED) && _noticeByHighSpeed) || (other.CompareTag(Tags.NOTICE_LOW_SPEED) && !_noticeByHighSpeed))
            _playerInRange = false;

        return EnemyStates.STAY;
    }

    private void Attack()
    {
        RaycastHit2D hit = Physics2D.BoxCast(_thisTransform.position, BOX_CAST_BOX, 0, _thisTransform.right, _attackRange, LayerMask.GetMask(Layers.PLAYER_SHIP));
        Debug.DrawRay(_thisTransform.position, _thisTransform.right * _attackRange, Color.red, 0.1f);

        if (hit)
        {
            _doTimer = false;
            _attackTimer.Reset();
        }    
        else
            _doTimer = true;

        SetNewDirection(_playerShip.position - _thisTransform.position);
    }

    private void HitPlayer()
    {
        _playerShip.GetComponent<DamageShip>().Hit(_damage);
    }
}
