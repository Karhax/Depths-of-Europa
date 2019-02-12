using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Statics;

[System.Serializable]
public class EnemyEscapeBase : EnemyStateAttackEscapeBase
{
    [SerializeField, Range(0, 10)] float _durationToEscapePastLight;
    [SerializeField, Range(0, 15)] float _lookRange;

    Timer _escapedTimer;

    bool _doTimer = false;

    public override void SetUp(EnemyBase script, bool noticeByHighSpeed)
    {
        _escapedTimer = new Timer(_durationToEscapePastLight);
        base.SetUp(script, noticeByHighSpeed);
    }

    public override void EnterState()
    {
        base.EnterState();
        Flee();
    }

    public override void ExitState()
    {
        _escapedTimer.Reset();
    }

    public override EnemyStates FixedUpdate()
    {
        TurnTowardsTravelDistance(_turnSpeed);
        SetVelocity();

        RaycastHit2D hit = Physics2D.BoxCast(thisTransform.position, BOX_CAST_BOX, 0, thisTransform.right, _lookRange, LayerMask.GetMask(Layers.DEFAULT));

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
        }
            
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
        Collider2D[] colliders = new Collider2D[1];

        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(LayerMask.GetMask(Layers.FLARE));
        contactFilter.SetLayerMask(LayerMask.GetMask(Layers.ENEMY));

        thisTransform.GetComponent<Collider2D>().OverlapCollider(contactFilter, colliders);

        bool isFlare = false;
        Vector2 flarePosition = Vector2.zero;

        if (colliders[0] != null)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].CompareTag(Tags.FLARE))
                {
                    isFlare = true;
                    flarePosition = colliders[i].transform.position;
                    break;
                }
            }
        }

        if (isFlare)
            SetNewDirection((Vector2)thisTransform.position - flarePosition);
        else
            SetNewDirection(thisTransform.position - _playerShip.position);
    }
}
