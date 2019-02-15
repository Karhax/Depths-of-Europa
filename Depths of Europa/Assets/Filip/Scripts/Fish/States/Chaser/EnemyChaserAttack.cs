using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Statics;

[System.Serializable]
public class EnemyChaserAttack : EnemyAttackBase
{
    [SerializeField, Range(0.1f, 15)] float _timeToHunt;

    Timer _huntTimer;

    public override void SetUp(EnemyBase script, bool noticeByHighSpeed)
    {
        base.SetUp(script, noticeByHighSpeed);
        _huntTimer = new Timer(_timeToHunt);
    }

    public override void EnterState()
    {
        base.EnterState();
        _huntTimer.Reset();
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

        _huntTimer.Time += Time.deltaTime;

        if (_huntTimer.Expired() && !_playerInRange)
            return EnemyStates.ESCAPE;

        return EnemyStates.STAY;
    }

    public override EnemyStates OnTriggerEnter(Collider2D other)
    {
        if (other.CompareTag(Tags.PLAYER_OUTSIDE))
        {
            HitPlayer();
            return EnemyStates.ESCAPE;
        }
        else if ((other.CompareTag(Tags.NOTICE_HIGH_SPEED) && _noticeByHighSpeed) || (other.CompareTag(Tags.NOTICE_LOW_SPEED) && !_noticeByHighSpeed))
            _playerInRange = true;
        else if (other.CompareTag(Tags.BASE))
            return EnemyStates.ESCAPE;

        return EnemyStates.STAY;
    }
}
