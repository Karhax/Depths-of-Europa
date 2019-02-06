using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy0 : MonoBehaviour
{
    static int _currentMaxSortOrder = 0;

    [SerializeField] bool _noticeByHighSpeed;

    [Space]

    [SerializeField] EnemyIdleBase _idleState;
    [SerializeField] EnemyAttackBase _attackState;
    [SerializeField] EnemyEscapeBase _escapeState;

    EnemyStateBase _currentState;
    Transform _playerShip;

    private void Awake()
    {
        GetComponent<SpriteRenderer>().sortingOrder = _currentMaxSortOrder++;

        _idleState.SetUp(this, _noticeByHighSpeed);
        _attackState.SetUp(this, _noticeByHighSpeed);
        _escapeState.SetUp(this, _noticeByHighSpeed);

        ChangeState(_idleState);
    }

    private void FixedUpdate()
    {
        ChangeState(_currentState.FixedUpdate());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        ChangeState(_currentState.OnTriggerEnter(other));
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        ChangeState(_currentState.OnTriggerExit(other));
    }

    private void ChangeState(EnemyStates state)
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

    private void ChangeState(EnemyStateBase state)
    {
        if (_currentState != null)
            _currentState.ExitState();

        _currentState = state;

        _currentState.EnterState();
    }
}
