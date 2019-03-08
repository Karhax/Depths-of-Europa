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

    protected LayerMask _avoidLayer;
    protected Transform _faceTransform;
    protected Vector2 _boxCastBox;
    public virtual void SetUp(EnemyBase script, bool noticeByHighSpeed, Transform faceTransform, float enemyWidth)
    {
        _boxCastBox = new Vector2(enemyWidth, enemyWidth);
        _avoidLayer = LayerMask.GetMask(Layers.CHASER_SPAWN, Layers.DEFAULT, Layers.BASE, Layers.FLOATING_OBJECT, Layers.GO_THROUGH_WALL);

        _faceTransform = faceTransform;

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

    public abstract EnemyStates OnTriggerEnter(Collider2D other);

    public abstract EnemyStates OnTriggerExit(Collider2D other);

    protected void TurnTowardsTravelDistance(float turnSpeed)
    {
        if (_direction != (Vector2)_thisTransform.right)
        {
            _thisTransform.right = Vector2.MoveTowards(_thisTransform.right, _thisRigidbody.velocity, Time.deltaTime * turnSpeed);

            if (_thisTransform.rotation.eulerAngles.y != 0)
                _thisTransform.rotation = Quaternion.Euler(0, 0, 180);
        }
            
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
    [SerializeField, Range(0, 20)] int _framesBetweenSideRayShoot = 7; 
    [SerializeField, Range(5, 15)] float _sideViewRayLength = 10f;
    [SerializeField, Range(0, 90)] float _sideLookRotation = 45f;
    [SerializeField, Range(2, 10)] float _sideLookIgnoreRange = 8f;
    [SerializeField, Range(0, 15)] protected float _dodgeSpeed;

    protected Transform _playerShip;
    static protected int _divertDirection = 1;

    int _currentFrameCounter;
    readonly int LEFT = -1;
    readonly int RIGHT = 1;

    public override void SetUp(EnemyBase script, bool noticeByHighSpeed, Transform faceTransform, float enemyWidth)
    {
        base.SetUp(script, noticeByHighSpeed, faceTransform, enemyWidth);
    }

    public override void EnterState()
    {
        _currentFrameCounter = _framesBetweenSideRayShoot;

        if (_playerShip == null)
            _playerShip = GameManager.ShipObject.transform;
    }

    protected void Divert()
    {
        _currentFrameCounter++;

        if (_currentFrameCounter >= _framesBetweenSideRayShoot)
        {
            RaycastHit2D hitLeft = GetDivertSideRayHit(LEFT);
            
            if (hitLeft.collider == null)
                _divertDirection = LEFT;
            else if (Vector3.Distance(_thisTransform.position, hitLeft.transform.position) < _sideLookIgnoreRange)
            {
                RaycastHit2D hitRight = GetDivertSideRayHit(RIGHT);

                if (hitRight.collider == null)
                    _divertDirection = RIGHT;
                else
                {
                    if (Vector3.Distance(_thisTransform.position, hitLeft.transform.position) > Vector3.Distance(_thisTransform.position, hitRight.transform.position))
                        _divertDirection = LEFT;
                    else
                        _divertDirection = RIGHT;
                }
            }
            _currentFrameCounter = 0;
        }

        _direction = (_direction + _divertDirection * (Vector2)_thisTransform.up * Time.deltaTime * _dodgeSpeed).normalized * _stateSpeed;
    }

    private RaycastHit2D GetDivertSideRayHit(int leftRight)
    {
        Vector3 rayDirection = Quaternion.AngleAxis(_sideLookRotation * -leftRight, Vector3.back) * _thisTransform.right;
        return Physics2D.Raycast(_faceTransform.position, rayDirection, _sideViewRayLength, _avoidLayer);
    }
}
