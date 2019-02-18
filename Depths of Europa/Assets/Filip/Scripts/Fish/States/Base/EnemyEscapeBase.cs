﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Statics;

[System.Serializable]
public class EnemyEscapeBase : EnemyStateAttackEscapeBase
{
    [SerializeField, Range(0, 10)] protected float _durationToEscapePastLight;
    [SerializeField, Range(0, 15)] protected float _lookRange;

    protected Timer _escapedTimer;

    protected bool _doTimer = false;
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
        _doTimer = false;
        _escapedTimer.Reset();
    }

    public override EnemyStates FixedUpdate()
    {
        TurnTowardsTravelDistance(_turnSpeed);
        SetVelocity();

        RaycastHit2D hit = Physics2D.BoxCast(_thisTransform.position, BOX_CAST_BOX, 0, _thisTransform.right, _lookRange, LayerMask.GetMask(Layers.DEFAULT, Layers.CHASER_SPAWN));

        if (hit.collider != null)
            Divert();
        else
            Flee();

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
            _escapeFrom = _playerShip;
        }
        else if ((other.CompareTag(Tags.FLARE_TRIGGER)) || (EnteredBase(other)))
            _escapeFrom = other.transform;

        return EnemyStates.STAY;
    }

    public override EnemyStates OnTriggerExit(Collider2D other)
    {
        if (other.CompareTag(Tags.LIGHT))
            _doTimer = true;

        return EnemyStates.STAY;
    }

    private void Flee()
    {
        SetNewDirection(_thisTransform.position - _escapeFrom.position);
    }
}
