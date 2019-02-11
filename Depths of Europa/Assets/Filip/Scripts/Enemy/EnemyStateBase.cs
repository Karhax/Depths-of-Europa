using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Statics;

public enum EnemyStates
{
    IDLE,
    ATTACK,
    ESCAPE,
    STAY
}

[System.Serializable]
public abstract class EnemyStateBase
{
    [SerializeField, Range(0, 50)] protected float _stateSpeed;
    [SerializeField, Range(0, 10)] protected float _turnSpeed;
    [SerializeField, Range(0, 5)] protected float _turnSmootheness;

    protected BasicEnemy0 _script;
    protected Transform thisTransform;
    protected Rigidbody2D _thisRigidbody;

    protected Vector2 _direction;
    protected bool _noticeByHighSpeed;

    protected readonly Vector2 BOX_CAST_BOX = new Vector2(1, 1);
    public virtual void SetUp(BasicEnemy0 script, bool noticeByHighSpeed)
    {
        _noticeByHighSpeed = noticeByHighSpeed;
        _script = script;
        thisTransform = _script.transform;
        _thisRigidbody = _script.GetComponent<Rigidbody2D>();
    }

    public abstract void EnterState();
    public abstract EnemyStates FixedUpdate();
    public abstract void ExitState();

    public abstract EnemyStates OnTriggerEnter(Collider2D other);
    public abstract EnemyStates OnTriggerExit(Collider2D other);

    protected void TurnTowardsTravelDistance(float turnSpeed)
    {
        if (_direction != (Vector2)thisTransform.right)
            thisTransform.right = Vector2.MoveTowards(thisTransform.right, _thisRigidbody.velocity, Time.deltaTime * turnSpeed);
    }

    protected void SetNewDirection(Vector2 newDirection)
    {
        _direction = newDirection.normalized * _stateSpeed;
    }

    protected void SetVelocity()
    {
        _thisRigidbody.velocity = Vector2.Lerp(_thisRigidbody.velocity, _direction, Time.deltaTime * _turnSmootheness);
    }
}

[System.Serializable]
public abstract class EnemyStateAttackEscapeBase : EnemyStateBase
{
    [SerializeField, Range(0, 15)] protected float _dodgeSpeed;

    protected Transform _playerShip;
    static protected int _divertDirection;

    public override void SetUp(BasicEnemy0 script, bool noticeByHighSpeed)
    {
        base.SetUp(script, noticeByHighSpeed);
    }

    public override void EnterState()
    {
        _divertDirection = Random.Range(-1, 1);
        if (_divertDirection == 0)
            _divertDirection = 1;

        if (_playerShip == null)
            _playerShip = GameManager.ShipObject.transform;
    }

    protected void Divert()
    {
        _direction = (_direction + _divertDirection * (Vector2)thisTransform.up * Time.deltaTime * _dodgeSpeed).normalized * _stateSpeed;
    }
}
