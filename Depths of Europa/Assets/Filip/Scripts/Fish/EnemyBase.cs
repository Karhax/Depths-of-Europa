using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Statics;

public abstract class EnemyBase : MonoBehaviour
{
    protected bool _shouldMove = false;

    protected static int _currentMaxSortOrder = 0;

    [SerializeField] protected bool _noticeByHighSpeed;

    protected EnemyStateBase _currentState;

    private void Start()
    {
        GetComponent<SpriteRenderer>().sortingOrder = _currentMaxSortOrder++;
    }

    protected virtual void FixedUpdate()
    {
        if (_shouldMove)
            ChangeState(_currentState.FixedUpdate());
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(Tags.IN_CAMERA_TRIGGER))
            _shouldMove = true;

        ChangeState(_currentState.OnTriggerEnter(other));
    }

    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(Tags.IN_CAMERA_TRIGGER))
            TurnOfMovement();

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

    protected void TurnOfMovement()
    {
        _shouldMove = false;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }
}
