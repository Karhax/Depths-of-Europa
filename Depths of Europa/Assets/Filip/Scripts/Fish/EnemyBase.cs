using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Statics;

public abstract class EnemyBase : MonoBehaviour
{
    [SerializeField, Range(0.1f, 50)] protected float _enemyRadius = 0.25f;
    [SerializeField] protected bool _noticeByHighSpeed;

    protected bool _shouldMove = false;

    protected static int _currentMaxSortOrder = 0;

    protected EnemyStateBase _currentState;

    Collider2D[] _colliders;

    protected virtual void Awake()
    {
        _colliders = GetComponents<Collider2D>();
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

    protected void ChangeState(EnemyStateBase state)
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
    
    private void OnOfColliders(bool state)
    {
        for (int i = 0; i < _colliders.Length; i++)
        {
            _colliders[i].enabled = state;
        }
    }
}
