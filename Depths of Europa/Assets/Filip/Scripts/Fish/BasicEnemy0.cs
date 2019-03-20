using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy0 : EnemyBase
{
    [SerializeField, Range(0, 3)] float _maxPitchDeviation = 0.25f;
    [SerializeField, Range(0, 5)] float _maxSoundWaitTime = 0.5f;
    [SerializeField] AudioSource _attackSound;
    [SerializeField] Transform _faceTransform;

    [Space]

    [SerializeField] EnemyIdleBase _idleState;
    [SerializeField] EnemyAttackBase _attackState;
    [SerializeField] EnemyEscapeBase _escapeState;

    protected override void Awake()
    {
        if (_attackSound != null)
            _attackSound.pitch = 1 + Random.Range(-_maxPitchDeviation, _maxPitchDeviation);

        base.Awake();

        _idleState.SetUp(this, _noticeByHighSpeed, _faceTransform, _enemyRadius);
        _attackState.SetUp(this, _noticeByHighSpeed, _faceTransform, _enemyRadius);
        _escapeState.SetUp(this, _noticeByHighSpeed, _faceTransform, _enemyRadius);

        ChangeState(_idleState);
    }

    protected override void ChangeState(EnemyStates state)
    {
        switch (state)
        {
            case (EnemyStates.STAY): { return; }
            case (EnemyStates.IDLE): { ChangeState(_idleState); break; }
            case (EnemyStates.ESCAPE): { ChangeState(_escapeState); break; }
            case (EnemyStates.ATTACK):
                {
                    StartCoroutine(PlaySound(_currentState != _attackState));

                    ChangeState(_attackState);
                    break;
                }

            default: { throw new System.Exception("This enemy state is not fully implemented! " + this + " " + gameObject.name); }
        }
    }

    IEnumerator PlaySound(bool shouldPlay)
    {
        yield return new WaitForSeconds(Random.Range(0, _maxSoundWaitTime));

        if (shouldPlay && _attackSound != null)
            _attackSound.Play();
    }
}
