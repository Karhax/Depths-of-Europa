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
    [SerializeField, Range(0, 15)] float _lookRange;
    [SerializeField, Range(0, 50)] float _dodgeSpeed;

    Transform _playerShip;

    Vector2 _escapeDirection;
    Timer _escapedTimer;

    int _divertDirection;
    bool _doTimer = false;

    public override void SetUp(BasicEnemy0 script)
    {
        _escapedTimer = new Timer(_durationToEscapePastLight);
        base.SetUp(script);
    }

    public override void EnterState()
    {
        _divertDirection = Random.Range(-1, 1);
        if (_divertDirection == 0)
            _divertDirection = 1;

        if (_playerShip == null)
            _playerShip = GameObject.FindGameObjectWithTag(Tags.PLAYER_OUTSIDE).transform;

        Flee();
    }

    public override void ExitState()
    {
        _escapedTimer.Reset();
    }

    public override EnemyStates FixedUpdate()
    {
        if (_escapeDirection != (Vector2)thisTransform.right)
            thisTransform.right = Vector2.MoveTowards(thisTransform.right, _thisRigidbody.velocity, Time.deltaTime * _turnSpeed);

        _thisRigidbody.velocity = Vector2.Lerp(_thisRigidbody.velocity, _escapeDirection, Time.deltaTime * _turnSmootheness);

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
            Flee(flarePosition);
        else
            Flee(_playerShip.position);
    }

    private void Flee(Vector2 position)
    {
        _escapeDirection = ((Vector2)thisTransform.position - position).normalized * _escapeSpeed;
    }

    private void Divert()
    {
        _escapeDirection = (_escapeDirection + _divertDirection * (Vector2)thisTransform.up * Time.deltaTime * _dodgeSpeed).normalized * _escapeSpeed;
    }
}
