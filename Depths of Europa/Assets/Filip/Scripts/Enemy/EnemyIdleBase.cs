using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Statics;

[System.Serializable]
public class EnemyIdleBase : EnemyStateBase
{
    [SerializeField] bool _noticeByHighSpeed;
    [SerializeField, Range(0, 15)] protected float _idleRadius;
    [SerializeField, Range(0, 10)] protected float _speed;

    [SerializeField, Range(0, 5)] float _turnSmootheness;
    [SerializeField, Range(0, 10)] float _turnSpeed;

    [SerializeField, Range(0, 10)] float _minBackUpDistance;
    [SerializeField, Range(0, 15)] float _maxBackUpDistance;

    Vector2 _currentDestination;
    Vector2 _centerPosition = Vector2.zero;

    float _currentTurnSpeed;
    const float MIDDLE_ANGLE = 90f;
    const float RAY_CAST_LENGTH = 15f;

    LayerMask _lookForPlayerLayer;

    public override void SetUp(BasicEnemy0 script)
    {
        base.SetUp(script);
        _lookForPlayerLayer = LayerMask.GetMask(Layers.PLAYER_SHIP) | LayerMask.GetMask(Layers.DEFAULT);
    }

    public override void EnterState()
    {
        _centerPosition = thisTransform.position;
        PickNewDestination();
    }

    public override void ExitState()
    {}

    public override EnemyStates FixedUpdate()
    {
        Vector2 direction = _currentDestination - (Vector2)thisTransform.position;

        if (direction != (Vector2)thisTransform.right)
            thisTransform.right = Vector2.MoveTowards(thisTransform.right, _thisRigidbody.velocity, Time.deltaTime * _currentTurnSpeed);

        if (ReachedPoint())
            SetVelocity();
        else
            PickNewDestination();

        return EnemyStates.STAY;
    }

    public override EnemyStates OnTriggerEnter(Collider2D other)
    {
        if (other.CompareTag(Tags.LIGHT))
            return EnemyStates.ESCAPE;
        else if (other.CompareTag(Tags.NOTICE_HIGH_SPEED) && _noticeByHighSpeed)
        {
            return ShouldAttack(other.transform.position);
        }
        else if (other.CompareTag(Tags.NOTICE_LOW_SPEED) && !_noticeByHighSpeed)
        {
            return ShouldAttack(other.transform.position);
        }
        else if (!other.CompareTag(Tags.ENEMY) && other.CompareTag(Tags.PLAYER_OUTSIDE))
            BackUp(thisTransform.position - other.transform.position);
        return EnemyStates.STAY;
    }

    public override EnemyStates OnTriggerExit(Collider2D other)
    {
        return EnemyStates.STAY;
    }

    private void BackUp(Vector2 direction)
    {
        _currentDestination = (Vector2)thisTransform.position + direction * Random.Range(_minBackUpDistance, _maxBackUpDistance);
        SetTurnSpeed();
    }

    private void PickNewDestination()
    {
        int tries = 0;
        bool doAgain = true;

        while (doAgain && tries < 10)
        {
            tries++;
            _currentDestination = new Vector2(Random.Range(-_idleRadius, _idleRadius), Random.Range(-_idleRadius, _idleRadius)) + _centerPosition;

            if (!(Vector2.Angle(thisTransform.right, _currentDestination) > 60))
                doAgain = false;
        }

        SetTurnSpeed();
    }

    private void SetTurnSpeed()
    {
        _currentTurnSpeed = _turnSpeed * Vector2.Angle(thisTransform.right, _currentDestination - (Vector2)thisTransform.position) / MIDDLE_ANGLE;
    }

    private void SetVelocity()
    {
        _thisRigidbody.velocity = Vector2.Lerp(_thisRigidbody.velocity, (_currentDestination - (Vector2)thisTransform.position).normalized * _speed, Time.deltaTime * _turnSmootheness);
    }

    private bool ReachedPoint()
    {
        return !(thisTransform.position.x > _currentDestination.x - 0.5f && thisTransform.position.x < _currentDestination.x + 0.05f &&
                 thisTransform.position.y > _currentDestination.y - 0.5f && thisTransform.position.y < _currentDestination.y + 0.05f);
    }

    private EnemyStates ShouldAttack(Vector2 _playerPosition)
    {
        RaycastHit2D hit = Physics2D.BoxCast(thisTransform.position, BOX_CAST_BOX, 0, _playerPosition - (Vector2)thisTransform.position, RAY_CAST_LENGTH, _lookForPlayerLayer);

        if (hit.collider != null)
            return EnemyStates.ATTACK;

        return EnemyStates.STAY;
    }
}
