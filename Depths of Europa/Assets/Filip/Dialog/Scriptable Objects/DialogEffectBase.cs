using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public abstract class DialogEffectBase : ScriptableObject
{
    public abstract void SetUpEffect(Dialog dialogScript);

    public abstract void UpdateEffect();

    public abstract void ResetEffect();

    public abstract AllDialogEffects GetStopEffect();
}
