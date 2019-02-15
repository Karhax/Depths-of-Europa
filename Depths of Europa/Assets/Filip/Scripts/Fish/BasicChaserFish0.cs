using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicChaserFish0 : EnemyBase
{
    public delegate void FishOffScreen();
    public event FishOffScreen FishOffScreenEvent;

    [Space]

    [SerializeField] EnemyChaserAttack _attackState;
    [SerializeField] EnemyChaserEscape _escapeState;

    Transform _spawn;

    private void Awake()
    {
        _attackState.SetUp(this, _noticeByHighSpeed);
        _escapeState.SetUp(this, _noticeByHighSpeed);
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

    protected override void OnTriggerExit2D(Collider2D other)
    {
        base.OnTriggerExit2D(other);

        if (!_shouldMove && FishOffScreenEvent != null)
            FishOffScreenEvent.Invoke();
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
