using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Statics;

[System.Serializable]
public class EnemyChaserEscape : EnemyStateAttackEscapeBase
{
    [SerializeField, Range(0, 15)] float _lookRange;

    Transform _spawn;

    public override void SetUp(EnemyBase script, bool noticeByHighSpeed, Transform faceTransform, float enemyWidth)
    {
        base.SetUp(script, noticeByHighSpeed, faceTransform, enemyWidth);
        _spawn = ((BasicChaserFish0)script).GetSpawn();

        _avoidLayer = LayerMask.GetMask(Layers.DEFAULT, Layers.BASE, Layers.FLOATING_OBJECT);
    }

    public override void EnterState()
    {
        base.EnterState();
        Flee();
    }

    public override void ExitState()
    {}

    public override EnemyStates FixedUpdate()
    {
        TurnTowardsTravelDistance(_turnSpeed);
        SetVelocity();

        RaycastHit2D hit = Physics2D.BoxCast(_faceTransform.position, _boxCastBox, 0, _thisTransform.right, _lookRange, _avoidLayer);

        if (hit.collider != null)
            Divert();
        else
            Flee();

        return EnemyStates.STAY;
    }

    public override EnemyStates OnTriggerEnter(Collider2D other)
    {
        return EnemyStates.STAY;
    }

    public override EnemyStates OnTriggerExit(Collider2D other)
    {
        return EnemyStates.STAY;
    }

    private void Flee()
    {
        SetNewDirection((Vector2)_spawn.position - (Vector2)_thisTransform.position);
    }
}
