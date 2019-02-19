using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Statics;

[System.Serializable]
public class EnemyEscapeBase : EnemyStateAttackEscapeBase
{
    [SerializeField, Range(0, 10)] protected float _durationToEscapePastLight;
    [SerializeField, Range(0, 15)] protected float _lookRange;

    protected Timer _escapedTimer;

    protected bool _doTimer = true;
    protected Transform _escapeFrom;

    public override void SetUp(EnemyBase script, bool noticeByHighSpeed)
    {
        _escapedTimer = new Timer(_durationToEscapePastLight);
        base.SetUp(script, noticeByHighSpeed);
    }

    public override void EnterState()
    {
        base.EnterState();
        _escapeFrom = _playerShip;
        Flee();
    }

    public override void ExitState()
    {
        _doTimer = true;
        _escapedTimer.Reset();
    }

    public override EnemyStates FixedUpdate()
    {
        TurnTowardsTravelDistance(_turnSpeed);
        SetVelocity();

        if (_doTimer)
        {
            _escapedTimer.Time += Time.deltaTime;

            if (_escapedTimer.Expired())
                return EnemyStates.IDLE;
        }

        RaycastHit2D hit = Physics2D.BoxCast(_thisTransform.position, BOX_CAST_BOX, 0, _thisTransform.right, _lookRange, LayerMask.GetMask(Layers.DEFAULT, Layers.CHASER_SPAWN));

        if (hit.collider != null)
            Divert();
        else
            return Flee();

        return EnemyStates.STAY;
    }

    public override EnemyStates OnTriggerEnter(Collider2D other)
    {
        if (other.CompareTag(Tags.LIGHT))
        {
            _doTimer = false;
            _escapedTimer.Reset();
            _escapeFrom = _playerShip;
        }
        else if (other.CompareTag(Tags.FLARE_TRIGGER) || EnteredBase(other) || other.CompareTag(Tags.ENEMY_LIGHT))
            _escapeFrom = other.transform;

        return EnemyStates.STAY;
    }

    public override EnemyStates OnTriggerExit(Collider2D other)
    {
        if (other.CompareTag(Tags.LIGHT) || other.CompareTag(Tags.FLARE_TRIGGER) || other.CompareTag(Tags.BASE) || other.CompareTag(Tags.ENEMY_LIGHT))
            _doTimer = true;

        return EnemyStates.STAY;
    }

    private EnemyStates Flee()
    {
        if (_escapeFrom == null)
            return EnemyStates.IDLE; 

        SetNewDirection(_thisTransform.position - _escapeFrom.position);

        return EnemyStates.STAY;
    }
}
