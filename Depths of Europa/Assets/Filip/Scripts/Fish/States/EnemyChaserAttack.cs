using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Statics;

[System.Serializable]
public class EnemyChaserAttack : EnemyAttackBase
{
    public override EnemyStates OnTriggerEnter(Collider2D other)
    {
        if (other.CompareTag(Tags.PLAYER_OUTSIDE))
        {
            HitPlayer();
            return EnemyStates.ESCAPE;
        }
        else if ((other.CompareTag(Tags.NOTICE_HIGH_SPEED) && _noticeByHighSpeed) || (other.CompareTag(Tags.NOTICE_LOW_SPEED) && !_noticeByHighSpeed))
            _playerInRange = true;

        return EnemyStates.STAY;
    }
}
