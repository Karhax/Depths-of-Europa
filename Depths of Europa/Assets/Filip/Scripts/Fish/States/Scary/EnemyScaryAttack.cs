using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Statics;

[System.Serializable]
public class EnemyScaryAttack : EnemyAttackBase
{
    Transform _attackTarget;

    public override void EnterState()
    {
        _attackTarget = _playerShip;
        base.EnterState();
    }

    protected override EnemyStates Attack()
    {
        if (_attackTarget == null)
            return EnemyStates.IDLE;

        if (Vector2.Distance(_thisTransform.position, _attackTarget.position) > _maxDistanceFromPlayerToStopAttack)
            return EnemyStates.IDLE;

        SetNewDirection(_attackTarget.position - _thisTransform.position);

        return EnemyStates.STAY;
    }


    public override EnemyStates OnTriggerEnter(Collider2D other)
    {
        if ((other.CompareTag(Tags.LIGHT) || other.CompareTag(Tags.FLARE_TRIGGER)) || (other.CompareTag(Tags.NOTICE_HIGH_SPEED) && _noticeByHighSpeed) || (other.CompareTag(Tags.NOTICE_LOW_SPEED) && !_noticeByHighSpeed))
            _attackTarget = other.transform;
        else if (other.CompareTag(Tags.PLAYER_OUTSIDE))
        {
            HitPlayer();
            return EnemyStates.ESCAPE;
        }
        else if (other.CompareTag(Tags.ALL_FISH_ESCAPE))
            return EnemyStates.ESCAPE;

        return EnemyStates.STAY;
    }

    public override EnemyStates OnTriggerExit(Collider2D other)
    {
        return EnemyStates.STAY;
    }
}
