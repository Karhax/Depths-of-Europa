using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyIdleBase : EnemyStateBase
{
    [SerializeField] protected float _idleRadius;
    [SerializeField] protected float _speed;

    Vector2 _currentDestination;

    public override void EnterState()
    {
        PickNewDestination();
    }

    public override void ExitState()
    {
    }

    public override EnemyStates Update()
    {
        if ((Vector2)_script.transform.position != _currentDestination)
            _thisRigidbody.MovePosition(Vector2.MoveTowards(_script.transform.position, _currentDestination, Time.deltaTime * _speed));
        else
            PickNewDestination();

        return EnemyStates.STAY;
    }

    public override EnemyStates OnTriggerStay(Collider2D other)
    {
        PickNewDestination();
        return EnemyStates.STAY;
    }

    public override EnemyStates OnTriggerExit(Collider2D other)
    {
        return EnemyStates.STAY;
    }

    private void PickNewDestination()
    {
        _currentDestination = new Vector2(Random.Range(-_idleRadius, _idleRadius), Random.Range(-_idleRadius, _idleRadius));
        _script.transform.right = _currentDestination - (Vector2)_script.transform.position;
    }
}
