using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Statics;

public class BasicFrozenFish0 : EnemyBase
{
    [SerializeField] Transform _faceTransform;
    [SerializeField, Range(0, 10)] float _timeInLightToScared;

    [Space]

    [SerializeField] EnemyIdleBase _idleState;
    [SerializeField] EnemyAttackBase _attackState;
    [SerializeField] EnemyEscapeBase _escapeState;

    float _currentLightTime = 0;
    List<Collider2D> _fleeFrom = new List<Collider2D>();

    protected override void Awake()
    {
        base.Awake();

        _idleState.SetUp(this, _noticeByHighSpeed, _faceTransform);
        _attackState.SetUp(this, _noticeByHighSpeed, _faceTransform);
        _escapeState.SetUp(this, _noticeByHighSpeed, _faceTransform);

        ChangeState(_idleState);
    }

    private void Update()
    {
        IsInLight();
    }

    private void IsInLight()
    {
        if (_fleeFrom.Count > 0)
        {
            _currentLightTime += Time.deltaTime;

            if (_currentLightTime >= _timeInLightToScared)
                base.OnTriggerEnter2D(_fleeFrom[0]);
        }
        else if (_currentLightTime > 0)
            _currentLightTime -= Time.deltaTime;
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(Tags.FLARE_TRIGGER) || other.CompareTag(Tags.LIGHT))
            _fleeFrom.Add(other);
        else
            base.OnTriggerEnter2D(other);
    }

    protected override void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(Tags.FLARE_TRIGGER) || other.CompareTag(Tags.LIGHT))
            _fleeFrom.Remove(other);
        else
            base.OnTriggerExit2D(other);
    }

    protected override void ChangeState(EnemyStates state)
    {
        switch (state)
        {
            case (EnemyStates.STAY): { return; }
            case (EnemyStates.IDLE): { ChangeState(_idleState); break; }
            case (EnemyStates.ESCAPE): { ChangeState(_escapeState); break; }
            case (EnemyStates.ATTACK): { ChangeState(_attackState); break; }

            default: { throw new System.Exception("This enemy state is not fully implemented! " + this + " " + gameObject.name); }
        }
    }
}
