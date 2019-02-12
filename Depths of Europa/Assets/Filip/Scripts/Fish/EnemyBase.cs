using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    protected static int _currentMaxSortOrder = 0;

    [SerializeField] protected bool _noticeByHighSpeed;

    protected EnemyStateBase _currentState;

    private void Awake()
    {
        GetComponent<SpriteRenderer>().sortingOrder = _currentMaxSortOrder++;
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

    protected abstract void ChangeState(EnemyStates state);

    protected  void ChangeState(EnemyStateBase state)
    {
        if (_currentState != null)
            _currentState.ExitState();

        _currentState = state;

        _currentState.EnterState();
    }
}
