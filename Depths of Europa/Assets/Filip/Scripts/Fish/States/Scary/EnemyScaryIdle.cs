using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Statics;

[System.Serializable]
public class EnemyScaryIdle : EnemyIdleBase
{
    public override EnemyStates OnTriggerEnter(Collider2D other)
    {
        if (other.CompareTag(Tags.LIGHT) || other.CompareTag(Tags.FLARE_TRIGGER))
            return ShouldAttack(other.transform.position);
        else if (other.CompareTag(Tags.NOTICE_HIGH_SPEED) && _noticeByHighSpeed)
            return ShouldAttack(other.transform.position);
        else if (other.CompareTag(Tags.NOTICE_LOW_SPEED) && !_noticeByHighSpeed)
            return ShouldAttack(other.transform.position);
        else if (other.CompareTag(Tags.WALL) || other.CompareTag(Tags.ICE))
            BackUp(_thisTransform.position - other.transform.position);
        else if (other.CompareTag(Tags.PLAYER_OUTSIDE) || other.CompareTag(Tags.ALL_FISH_ESCAPE))
            return EnemyStates.ESCAPE;

        return EnemyStates.STAY;
    }
}
