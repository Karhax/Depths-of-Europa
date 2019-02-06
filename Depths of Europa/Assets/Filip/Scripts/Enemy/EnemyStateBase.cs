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
    protected Transform thisTransform;
    protected Rigidbody2D _thisRigidbody;

    protected readonly Vector2 BOX_CAST_BOX = new Vector2(1, 1);
    public virtual void SetUp(BasicEnemy0 script)
    {
        _script = script;
        thisTransform = _script.transform;
        _thisRigidbody = _script.GetComponent<Rigidbody2D>();
    }

    public abstract void EnterState();
    public abstract EnemyStates FixedUpdate();
    public abstract void ExitState();

    public abstract EnemyStates OnTriggerEnter(Collider2D other);
    public abstract EnemyStates OnTriggerExit(Collider2D other);
}
