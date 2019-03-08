using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New DialogEffectCharacterShake", menuName = "Dialog/Effect/Character Shake")]
public class DialogEffectCharacterShake : DialogEffectBase
{
    [SerializeField] float _shakeSpeed;

    public override void SetUpEffect(Dialog dialogScript)
    {
    }

    public override void UpdateEffect()
    {
    }

    public override void ResetEffect()
    {
    }
}
