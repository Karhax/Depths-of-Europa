using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Statics;

[System.Serializable]
public class EnemyAttackBase : EnemyStateAttackEscapeBase
{ 
    [SerializeField, Range(0, 15)] protected float _avoidRange;
    [SerializeField, Range(0, 15)] protected float _attackRange;
    [SerializeField, Range(0, 50)] protected int _damage;
    [SerializeField, Range(0.1f, 50)] protected float _timeToHunt;
    [SerializeField, Range(1, 50)] protected float _maxDistanceFromPlayerToStopAttack;

    protected Timer _huntTimer;

    protected bool _doTimer = false;

    public override void SetUp(EnemyBase script, bool noticeByHighSpeed, Transform faceTransform)
    {
        _huntTimer = new Timer(_timeToHunt);
        base.SetUp(script, noticeByHighSpeed, faceTransform);
    }

    public override void ExitState()
    {
        _huntTimer.Reset();
    }

    public override EnemyStates FixedUpdate()
    {
        TurnTowardsTravelDistance(_turnSpeed);
        SetVelocity();

        EnemyStates lostPlayer = EnemyStates.STAY;

        RaycastHit2D hit = Physics2D.BoxCast(_faceTransform.position, BOX_CAST_BOX, 0, _thisTransform.right, _avoidRange, _avoidLayer);

        if (hit.collider != null)
        {
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer(Layers.BASE))
                return EnemyStates.ESCAPE;

            Divert();
        }       
        else
            lostPlayer = Attack();

        if (lostPlayer != EnemyStates.STAY)
            return lostPlayer;

        return HuntTimer();
    }

    protected virtual EnemyStates Attack()
    {
        if (Vector2.Distance(_thisTransform.position, _playerShip.position) > _maxDistanceFromPlayerToStopAttack)
            return EnemyStates.IDLE;

        SetNewDirection(_playerShip.position - _thisTransform.position);

        return EnemyStates.STAY;
    }

    protected virtual EnemyStates HuntTimer()
    {
        _huntTimer.Time += Time.deltaTime;

        if (_huntTimer.Expired())
            return EnemyStates.IDLE;

        return EnemyStates.STAY;
    }

    public override EnemyStates OnTriggerEnter(Collider2D other)
    {
        if (other.CompareTag(Tags.LIGHT) || other.CompareTag(Tags.FLARE_TRIGGER) || other.CompareTag(Tags.ENEMY_LIGHT) || other.CompareTag(Tags.ALL_FISH_ESCAPE))
            return EnemyStates.ESCAPE;
        else if (other.CompareTag(Tags.PLAYER_OUTSIDE))
        {
            HitPlayer();
            return EnemyStates.ESCAPE;
        }

        return EnemyStates.STAY;
    }

    public override EnemyStates OnTriggerExit(Collider2D other)
    {
        return EnemyStates.STAY;
    }

    protected void HitPlayer()
    {
        _playerShip.GetComponent<DamageShip>().Hit(_damage);
    }
}
