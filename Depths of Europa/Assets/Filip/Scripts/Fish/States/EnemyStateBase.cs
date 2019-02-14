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
    [SerializeField, Range(0, 10)] protected float _stateSpeedModifier;
    [SerializeField, Range(0, 10)] protected float _turnSpeed;
    [SerializeField, Range(0, 5)] protected float _turnSmootheness;

    protected EnemyBase _script;
    protected Transform _thisTransform;
    protected Rigidbody2D _thisRigidbody;

    protected Vector2 _direction;
    protected bool _noticeByHighSpeed;

    protected readonly Vector2 BOX_CAST_BOX = new Vector2(0.25f, 0.25f);
    public virtual void SetUp(EnemyBase script, bool noticeByHighSpeed)
    {
        _noticeByHighSpeed = noticeByHighSpeed;
        _script = script;
        _thisTransform = _script.transform;
        _thisRigidbody = _script.GetComponent<Rigidbody2D>();

        float newSpeed = _stateSpeed + Random.Range(-_stateSpeedModifier, _stateSpeedModifier);

        if (newSpeed <= 0)
            Debug.LogWarning("The settings make the speed negative! Change SpeedStateModifier!", _thisTransform.gameObject);
        else
            _stateSpeed = newSpeed;
    }

    public abstract void EnterState();
    public abstract EnemyStates FixedUpdate();
    public abstract void ExitState();

    public virtual EnemyStates OnTriggerEnter(Collider2D other)
    {
        if (other.CompareTag(Tags.BASE))
            return EnemyStates.ESCAPE;

        return EnemyStates.STAY;
    }
    public abstract EnemyStates OnTriggerExit(Collider2D other);

    protected void TurnTowardsTravelDistance(float turnSpeed)
    {
        if (_direction != (Vector2)_thisTransform.right)
            _thisTransform.right = Vector2.MoveTowards(_thisTransform.right, _thisRigidbody.velocity, Time.deltaTime * turnSpeed);
    }

    protected void SetNewDirection(Vector2 newDirection)
    {
        _direction = newDirection.normalized * _stateSpeed;
    }

    protected void SetVelocity()
    {
        _thisRigidbody.velocity = Vector2.Lerp(_thisRigidbody.velocity, _direction, Time.deltaTime * _turnSmootheness);
    }

    protected EnemyStates ShouldEscape(Vector3 escapeFrom)
    {
        RaycastHit2D hit = Physics2D.Raycast(_thisTransform.position, escapeFrom - _thisTransform.position, Vector2.Distance(_thisTransform.position, escapeFrom), LayerMask.GetMask(Layers.DEFAULT));

        if (hit.collider == null)
            return EnemyStates.ESCAPE;

        return EnemyStates.STAY;
    }
}

[System.Serializable]
public abstract class EnemyStateAttackEscapeBase : EnemyStateBase
{
    [SerializeField, Range(0, 15)] protected float _dodgeSpeed;

    protected Transform _playerShip;
    static protected int _divertDirection;

    public override void SetUp(EnemyBase script, bool noticeByHighSpeed)
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
        _direction = (_direction + _divertDirection * (Vector2)_thisTransform.up * Time.deltaTime * _dodgeSpeed).normalized * _stateSpeed;
    }
}
