using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyIdleBase : EnemyStateBase
{
    [SerializeField] protected float _idleRadius;
    [SerializeField] protected float _speed;

    [SerializeField] float _turnSpeed;

    [SerializeField] float _minBackUpDistance;
    [SerializeField] float _maxBackUpDistance;

    [SerializeField] float _minPointDistance;

    Vector2 _currentDestination;
    Vector2 _centerPosition;

    float _currentTurnSpeed;
    const float MIDDLE_ANGLE = 90f;

    public override void EnterState()
    {
        _centerPosition = _script.transform.position;
        PickNewDestination();
    }

    public override void ExitState()
    {
    }

    public override EnemyStates Update()
    {
        Vector2 direction = _currentDestination - (Vector2)_script.transform.position;

        if (direction != (Vector2)_script.transform.right)
            _script.transform.right = Vector2.MoveTowards(_script.transform.right, direction, Time.deltaTime * _currentTurnSpeed);

        if (ReachedPoint())
            SetVelocity();
            //_thisRigidbody.MovePosition(Vector2.MoveTowards(_script.transform.position, _currentDestination, Time.deltaTime * _speed));
        else
            PickNewDestination();

        return EnemyStates.STAY;
    }

    public override EnemyStates OnTriggerEnter(Collider2D other)
    {
        BackUp(_script.transform.position - other.transform.position);
        return EnemyStates.STAY;
    }

    public override EnemyStates OnTriggerExit(Collider2D other)
    {
        return EnemyStates.STAY;
    }

    private void BackUp(Vector2 direction)
    {
        _currentDestination = (Vector2)_script.transform.position + direction * Random.Range(_minBackUpDistance, _maxBackUpDistance);
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

            if (!(Vector2.Angle((Vector2)_script.transform.right, _currentDestination) > 60))
                doAgain = false;
        }

        SetTurnSpeed();
    }

    private void SetTurnSpeed()
    {
        _currentTurnSpeed = _turnSpeed * Vector2.Angle(_script.transform.right, _currentDestination - (Vector2)_script.transform.position) / MIDDLE_ANGLE;
    }

    private void SetVelocity()
    {
        _thisRigidbody.velocity = Vector2.Lerp(_thisRigidbody.velocity, (_currentDestination - (Vector2)_script.transform.position).normalized * _speed, Time.deltaTime);
    }

    private bool ReachedPoint()
    {
        return !(_script.transform.position.x > _currentDestination.x - 0.5f && _script.transform.position.x < _currentDestination.x + 0.05f &&
                 _script.transform.position.y > _currentDestination.y - 0.5f && _script.transform.position.y < _currentDestination.y + 0.05f);
    }
}
