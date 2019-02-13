using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicUnderPlayerFish0 : EnemyBase
{
    [Header("Notice by high speed does nothing here")]

    [SerializeField] EnemyIdleBase _idleState;

    private void Awake()
    {
        _idleState.SetUp(this, _noticeByHighSpeed);

        ChangeState(_idleState);
    }

    protected override void ChangeState(EnemyStates state) { }
}
