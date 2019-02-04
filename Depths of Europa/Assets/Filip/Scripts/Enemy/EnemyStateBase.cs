using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyStates
{
    IDLE,
    ATTACK,
    ESCAPE,
    STAY
}

[System.Serializable]
public abstract class EnemyStateBase
{
    protected BasicEnemy0 _script;
    protected Rigidbody2D _thisRigidbody;

    public virtual void SetUp(BasicEnemy0 script)
    {
        _script = script;
        _thisRigidbody = _script.GetComponent<Rigidbody2D>();
    }

    public abstract void EnterState();
    public abstract EnemyStates Update();
    public abstract void ExitState();

    public abstract EnemyStates OnTriggerEnter(Collider2D other);
    public abstract EnemyStates OnTriggerExit(Collider2D other);
}
