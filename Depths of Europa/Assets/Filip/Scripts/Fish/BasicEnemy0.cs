using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy0 : EnemyBase
{
    [Space]

    [SerializeField] EnemyIdleBase _idleState;
    [SerializeField] EnemyAttackBase _attackState;
    [SerializeField] EnemyEscapeBase _escapeState;

    protected override void Awake()
    {
        base.Awake();

        _idleState.SetUp(this, _noticeByHighSpeed);
        _attackState.SetUp(this, _noticeByHighSpeed);
        _escapeState.SetUp(this, _noticeByHighSpeed);

        ChangeState(_idleState);
    }

    protected override void ChangeState(EnemyStates state)
    {
        switch (state)
        {
            case (EnemyStates.STAY): { return; }
            case (EnemyStates.IDLE): { ChangeState(_idleState); break; }
            case (EnemyStates.ESCAPE): { ChangeState(_escapeState); break; }
            case (EnemyStates.ATTACK): { ChangeState(_attackState); break; }

            default: { throw new System.Exception("This enemy state is not fully implemented! " + this + " " + gameObject.name); }
        }
    }
}
