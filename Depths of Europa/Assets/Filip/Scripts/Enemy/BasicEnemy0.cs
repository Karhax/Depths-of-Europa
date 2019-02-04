using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy0 : MonoBehaviour
{
    [SerializeField] EnemyIdleBase _idleState;
    [SerializeField] EnemyAttackBase _attackState;
    [SerializeField] EnemyEscapeBase _escapeState;

    EnemyStateBase _currentState;

    private void Awake()
    {
        _idleState.SetUp(this);
        _attackState.SetUp(this);
        _escapeState.SetUp(this);

        ChangeState(_idleState);
    }

    private void Update()
    {
        ChangeState(_currentState.Update());
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        _currentState.OnTriggerStay(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        _currentState.OnTriggerExit(other);
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
