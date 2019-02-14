using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicChaserFish0 : EnemyBase
{
    public delegate void FishOffScreen();
    public event FishOffScreen FishOffScreenEvent;

    [SerializeField, Range(0.1f, 15f)] float _timeOutsideScreenRespawn;

    [Space]

    [SerializeField] EnemyIdleBase _idleState;
    [SerializeField] EnemyChaserAttack _attackState;
    [SerializeField] EnemyEscapeBase _escapeState;

    Camera _camera;
    Timer _offScreenTimer;

    readonly float OFF_SCREEN_LOW = 0f;
    readonly float OFF_SCREEN_HIGH = 1f;

    private void Awake()
    {
        _offScreenTimer = new Timer(_timeOutsideScreenRespawn);

        _idleState.SetUp(this, _noticeByHighSpeed);
        _attackState.SetUp(this, _noticeByHighSpeed);
        _escapeState.SetUp(this, _noticeByHighSpeed);

        ChangeState(_idleState);
    }

    private void Start()
    {
        _camera = GameManager.CameraObject.GetComponent<Camera>();
    }

    private void Update()
    {
        IsOutsideScreen();
    }

    private void IsOutsideScreen()
    {
        Vector3 screenSpacePosition = _camera.WorldToViewportPoint(transform.position);
        
        if (!(screenSpacePosition.x > OFF_SCREEN_LOW && screenSpacePosition.x < OFF_SCREEN_HIGH && screenSpacePosition.y > OFF_SCREEN_LOW && screenSpacePosition.y < OFF_SCREEN_HIGH))
        {
            _offScreenTimer.Time += Time.deltaTime;

            if (_offScreenTimer.Expired())
                Debug.Log("HERE");

            if (_offScreenTimer.Expired() && FishOffScreenEvent != null)
                FishOffScreenEvent.Invoke();
        }
        else
            _offScreenTimer.Reset();
    }

    protected override void ChangeState(EnemyStates state)
    {
        if (state == EnemyStates.ESCAPE && _currentState == _idleState)
            return;

        switch (state)
        {
            case (EnemyStates.STAY): { return; }
            case (EnemyStates.ESCAPE): { ChangeState(_escapeState); break; }
            case (EnemyStates.IDLE): { ChangeState(_idleState); break; }
            case (EnemyStates.ATTACK): { ChangeState(_attackState); break; }

            default: { throw new System.Exception("This enemy state is not fully implemented! " + this + " " + gameObject.name); }
        }
    }
}
