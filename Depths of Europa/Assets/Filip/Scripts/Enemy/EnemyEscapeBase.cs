using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Statics;

[System.Serializable]
public class EnemyEscapeBase : EnemyStateBase
{
    [SerializeField, Range(0, 15)] float _escapeSpeed;
    [SerializeField, Range(0, 10)] float _durationToEscapePastLight;
    [SerializeField, Range(0, 5)] float _turnSmootheness;
    [SerializeField, Range(0, 10)] float _turnSpeed;

    Transform _playerShip;

    Vector2 _escapeDirection;
    Timer _escapedTimer;

    bool _doTimer = false;
    bool diverting = false;

    public override void SetUp(BasicEnemy0 script)
    {
        _escapedTimer = new Timer(_durationToEscapePastLight);
        base.SetUp(script);
    }

    public override void EnterState()
    {
        if (_playerShip == null)
            _playerShip = GameObject.FindGameObjectWithTag(Tags.PLAYER_OUTSIDE).transform;

        _escapedTimer.Reset();
        _escapeDirection = (thisTransform.position - _playerShip.position).normalized * _escapeSpeed;
    }

    public override void ExitState()
    {}

    public override EnemyStates Update()
    {
        if (_escapeDirection != (Vector2)thisTransform.right)
            thisTransform.right = Vector2.MoveTowards(thisTransform.right, _escapeDirection, Time.deltaTime * _turnSpeed);

        _thisRigidbody.velocity = Vector2.Lerp(_thisRigidbody.velocity, _escapeDirection, Time.deltaTime * _turnSmootheness);

        RaycastHit2D hit = Physics2D.BoxCast(thisTransform.position, new Vector2(0.1f, 0.1f), 0, thisTransform.right, 6, LayerMask.GetMask(Layers.DEFAULT));

        Debug.Log(hit.collider);

        if (hit.collider != null)
        {
            _escapeDirection = (_escapeDirection + -(Vector2)thisTransform.up * Time.deltaTime * 25).normalized * _escapeSpeed;
        }

        if (_doTimer)
        {
            _escapedTimer.Time += Time.deltaTime;

            if (_escapedTimer.Expired())
                return EnemyStates.IDLE;
        }

        return EnemyStates.STAY;
    }

    public override EnemyStates OnTriggerEnter(Collider2D other)
    {
        if (other.CompareTag(Tags.LIGHT))
        {
            _doTimer = false;
            _escapedTimer.Reset();
        }
            
        return EnemyStates.STAY;
    }

    public override EnemyStates OnTriggerExit(Collider2D other)
    {
        if (other.CompareTag(Tags.LIGHT))
            _doTimer = true;

        return EnemyStates.STAY;
    }
}
