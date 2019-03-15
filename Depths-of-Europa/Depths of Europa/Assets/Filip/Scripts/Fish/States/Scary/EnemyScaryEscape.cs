using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Statics;

[System.Serializable]
public class EnemyScaryEscape : EnemyEscapeBase
{
    public override EnemyStates OnTriggerEnter(Collider2D other)
    {
        return EnemyStates.STAY;
    }

    public override EnemyStates OnTriggerExit(Collider2D other)
    {
        return EnemyStates.STAY;
    }
}
