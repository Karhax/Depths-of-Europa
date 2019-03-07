using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New DialogEffectShake", menuName = "Dialog/Effect/Dialog Shake")]
public class DialogEffectShake : DialogEffectBase
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

    public override AllDialogEffects GetStopEffect()
    {
        return AllDialogEffects.SHAKE;
    }
}
