using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Statics;

[System.Serializable]
public class EnemyIdleBase : EnemyStateBase
{
    [SerializeField, Range(0, 15)] protected float _idleRadius;

    [SerializeField, Range(0, 10)] protected float _minBackUpDistance;
    [SerializeField, Range(0, 15)] protected float _maxBackUpDistance;

    protected Vector2 _centerPosition = Vector2.zero;
    protected Vector2 _currentDestination;

    protected float _currentTurnSpeed;
    protected readonly float MIDDLE_ANGLE = 90f;
    protected readonly float RAY_CAST_LENGTH = 15f;
    protected readonly int NUMBER_OF_TRIES_FIND_NEW_PATH = 5;
    protected readonly int TRY_ANGLE = 60;

    protected LayerMask _lookForPlayerLayer;

    public override void SetUp(EnemyBase script, bool noticeByHighSpeed)
    {
        base.SetUp(script, noticeByHighSpeed);
        _lookForPlayerLayer = LayerMask.GetMask(Layers.PLAYER_SHIP) | LayerMask.GetMask(Layers.DEFAULT);
    }

    public override void EnterState()
    {
        _centerPosition = _thisTransform.position;
        PickNewDestination();
    }

    public override void ExitState()
    {}

    public override EnemyStates FixedUpdate()
    {
        TurnTowardsTravelDistance(_currentTurnSpeed);

        if (ReachedPoint())
            SetVelocity();
        else
            PickNewDestination();

        return EnemyStates.STAY;
    }

    public override EnemyStates OnTriggerEnter(Collider2D other)
    {
        if (other.CompareTag(Tags.LIGHT) || other.CompareTag(Tags.PLAYER_OUTSIDE))
            return ShouldEscape(other.transform.position);
        else if (other.CompareTag(Tags.NOTICE_HIGH_SPEED) && _noticeByHighSpeed)
            return ShouldAttack(other.transform.position);
        else if (other.CompareTag(Tags.NOTICE_LOW_SPEED) && !_noticeByHighSpeed)
            return ShouldAttack(other.transform.position);
        else if (other.CompareTag(Tags.WALL))
            BackUp(_thisTransform.position - other.transform.position);
        return EnemyStates.STAY;
    }

    private EnemyStates ShouldEscape(Vector3 escapeFrom)
    {
        RaycastHit2D hit = Physics2D.Raycast(_thisTransform.position, escapeFrom - _thisTransform.position, Vector2.Distance(_thisTransform.position, escapeFrom), LayerMask.GetMask(Layers.DEFAULT));

        if (hit.collider == null)
            return EnemyStates.ESCAPE;

        return EnemyStates.STAY;
    }

    public override EnemyStates OnTriggerExit(Collider2D other)
    {
        return EnemyStates.STAY;
    }

    protected void BackUp(Vector2 direction)
    {
        SetNewDirection((direction * Random.Range(_minBackUpDistance, _maxBackUpDistance)) - (Vector2)_thisTransform.position);
        SetTurnSpeed();
    }

    protected virtual void PickNewDestination()
    {
        int tries = 0;
        bool doAgain = true;

        while (doAgain && tries < NUMBER_OF_TRIES_FIND_NEW_PATH)
        {
            tries++;
            _currentDestination = new Vector2(Random.Range(-_idleRadius, _idleRadius), Random.Range(-_idleRadius, _idleRadius)) + _centerPosition;
            SetNewDirection(_currentDestination - (Vector2)_thisTransform.position);

            if (!(Vector2.Angle(_thisTransform.right, _currentDestination) > TRY_ANGLE))
                doAgain = false;
        }

        SetTurnSpeed();
    }

    protected void SetTurnSpeed()
    {
        _currentTurnSpeed = _turnSpeed * Vector2.Angle(_thisTransform.right, _direction) / MIDDLE_ANGLE;
    }


    protected bool ReachedPoint()
    {
        float TOLERANCE = 0.25f;

        return !(_thisTransform.position.x > _currentDestination.x - TOLERANCE && _thisTransform.position.x < _currentDestination.x + TOLERANCE &&
                 _thisTransform.position.y > _currentDestination.y - TOLERANCE && _thisTransform.position.y < _currentDestination.y + TOLERANCE);
    }

    private EnemyStates ShouldAttack(Vector2 _playerPosition)
    {
        RaycastHit2D hit = Physics2D.Raycast(_thisTransform.position, _playerPosition - (Vector2)_thisTransform.position, Vector2.Distance(_thisTransform.position, _playerPosition), LayerMask.GetMask(Layers.DEFAULT));

        if (hit.collider == null)
            return EnemyStates.ATTACK;

        return EnemyStates.STAY;
    }
}
