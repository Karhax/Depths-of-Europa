using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicNonHostileFish0 : EnemyBase
{
    [Space]

    [SerializeField] EnemyIdleBase _idleState;
    [SerializeField] EnemyEscapeBase _escapeState;

    private void Awake()
    {
        _idleState.SetUp(this, _noticeByHighSpeed);
        _escapeState.SetUp(this, _noticeByHighSpeed);

        ChangeState(_idleState);
    }

    protected override void ChangeState(EnemyStates state)
    {
        switch (state)
        {
            case (EnemyStates.ATTACK):
            case (EnemyStates.STAY): { return; }
            case (EnemyStates.IDLE): { ChangeState(_idleState); break; }
            case (EnemyStates.ESCAPE): { ChangeState(_escapeState); break; }

            default: { throw new System.Exception("This enemy state is not fully implemented! " + this + " " + gameObject.name); }
        }
    }
}
