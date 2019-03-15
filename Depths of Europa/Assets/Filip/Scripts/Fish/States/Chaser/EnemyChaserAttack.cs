using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Statics;

[System.Serializable]
public class EnemyChaserAttack : EnemyAttackBase
{
    public override void SetUp(EnemyBase script, bool noticeByHighSpeed, Transform faceTransform, float enemyWidth)
    {
        base.SetUp(script, noticeByHighSpeed, faceTransform, enemyWidth);
    }

    protected override EnemyStates Attack()
    {
        if (Vector2.Distance(_thisTransform.position, _playerShip.position) > _maxDistanceFromPlayerToStopAttack)
            return EnemyStates.ESCAPE;

        SetNewDirection(_playerShip.position - _thisTransform.position);

        return EnemyStates.STAY;
    }

    protected override EnemyStates HuntTimer()
    {
        _huntTimer.Time += Time.deltaTime;

        if (_huntTimer.Expired())
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
        else if (other.CompareTag(Tags.ALL_FISH_ESCAPE))
            return EnemyStates.ESCAPE;

        return EnemyStates.STAY;
    }
}
