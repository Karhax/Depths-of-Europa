using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Statics;

public class BasicChaserFish0 : EnemyBase
{
    [SerializeField] Transform _faceTransform;

    [Space]

    [SerializeField] EnemyChaserAttack _attackState;
    [SerializeField] EnemyChaserEscape _escapeState;

    Transform _spawn;

    protected override void Awake()
    {
        base.Awake();

        _attackState.SetUp(this, _noticeByHighSpeed, _faceTransform, _enemyRadius);
        _escapeState.SetUp(this, _noticeByHighSpeed, _faceTransform, _enemyRadius);
    }

    private void Start()
    {
        ChangeState(_attackState);
    }

    public void SetSpawn(Transform spawn)
    {
        _spawn = spawn;
    }

    public Transform GetSpawn()
    {
        return _spawn;
    }

    public void ReachedSpawn()
    {
        GetComponent<Rigidbody2D>().simulated = false;
        Destroy(this);
    }

    protected override void OnTriggerExit2D(Collider2D other)
    {
        base.OnTriggerExit2D(other);
    }

    protected override void ChangeState(EnemyStates state)
    {

        switch (state)
        {
            case (EnemyStates.IDLE):
            case (EnemyStates.STAY): { return; }
            case (EnemyStates.ESCAPE): { ChangeState(_escapeState); break; }
            case (EnemyStates.ATTACK): { ChangeState(_attackState); break; }

            default: { throw new System.Exception("This enemy state is not fully implemented! " + this + " " + gameObject.name); }
        }
    }
}
