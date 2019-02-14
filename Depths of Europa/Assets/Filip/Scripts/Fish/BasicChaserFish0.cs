using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicChaserFish0 : EnemyBase
{
    public delegate void FishOffScreen();
    public event FishOffScreen FishOffScreenEvent;

    [Space]

    [SerializeField] EnemyIdleBase _idleState;
    [SerializeField] EnemyChaserAttack _attackState;
    [SerializeField] EnemyEscapeBase _escapeState;

    private void Awake()
    {
        _idleState.SetUp(this, _noticeByHighSpeed);
        _attackState.SetUp(this, _noticeByHighSpeed);
        _escapeState.SetUp(this, _noticeByHighSpeed);
    }

    private void Start()
    {
        ChangeState(_attackState);
    }

    protected override void OnTriggerExit2D(Collider2D other)
    {
        base.OnTriggerExit2D(other);

        if (!_shouldMove && FishOffScreenEvent != null)
            FishOffScreenEvent.Invoke();
    }

    protected override void ChangeState(EnemyStates state)
    {
        if (state == EnemyStates.ESCAPE && _currentState == _idleState)
            return;

        switch (state)
        {
            case (EnemyStates.STAY): { return; }
            case (EnemyStates.ESCAPE): { ChangeState(_escapeState); break; }
            case (EnemyStates.IDLE): { ChangeState(_idleState); break; }
            case (EnemyStates.ATTACK): { ChangeState(_attackState); break; }

            default: { throw new System.Exception("This enemy state is not fully implemented! " + this + " " + gameObject.name); }
        }
    }
}
