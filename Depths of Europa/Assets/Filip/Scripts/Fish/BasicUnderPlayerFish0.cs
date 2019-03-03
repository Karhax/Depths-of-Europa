using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicUnderPlayerFish0 : EnemyBase
{
    [SerializeField] Transform _faceTransform;

    [Space]

    [Header("Notice by high speed does nothing here")]

    [SerializeField] EnemyIdleBase _idleState;

    protected override void Awake()
    {
        base.Awake();

        _idleState.SetUp(this, _noticeByHighSpeed, _faceTransform);

        ChangeState(_idleState);
    }

    protected override void ChangeState(EnemyStates state) { }
}
