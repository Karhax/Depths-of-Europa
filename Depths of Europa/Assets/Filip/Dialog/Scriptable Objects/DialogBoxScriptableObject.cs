﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class DialogEffectBase : ScriptableObject
{
    public abstract void Update();
}

[CreateAssetMenu(fileName = "New DialogEffectColor", menuName = "Dialog/Effect/Color")]
public class DialogEffectColor : DialogEffectBase
{
    [SerializeField] Color _effectColor1 = Color.white;
    [SerializeField] Color _effectColor2 = Color.white;
    [SerializeField] float _changeColorSpeed = 5;

    public override void Update()
    {
    }
}

[CreateAssetMenu(fileName = "New DialogEffectShake", menuName = "Dialog/Effect/Shake")]
public class DialogEffectShake : DialogEffectBase
{
    [SerializeField] float _shakeSpeed;

    public override void Update()
    {
    }
}

[CreateAssetMenu(fileName = "New Dialog Box", menuName = "Dialog/Dialog Box")]
public class DialogBoxScriptableObject : ScriptableObject
{
    [SerializeField] DialogBoxObject[] _dialogBoxes;

    public DialogBoxObject[] DialogBoxes { get { return _dialogBoxes; } private set { _dialogBoxes = value; } }
}

[System.Serializable]
public class DialogBoxObject
{
    [SerializeField] bool _rightTalking = true;
    [SerializeField] CharacterScriptableObject _rightCharacter;
    [SerializeField] CharacterScriptableObject _leftCharacter;
    [SerializeField, TextAreaAttribute(3, 3)] string _dialogText;
    [SerializeField] AudioClip _textAudio;
    [SerializeField] Sprite _backgroundSprite;
    [SerializeField] DialogEffectBase _effect;

    public bool RightTalking { get { return _rightTalking; } private set { _rightTalking = value; } }
    public CharacterScriptableObject RightCharacter { get { return _rightCharacter; } private set { _rightCharacter = value; } }
    public CharacterScriptableObject LeftCharacter { get { return _leftCharacter; } private set { _leftCharacter = value; } }
    public string DialogText { get { return _dialogText; } private set { _dialogText = value; } }
    public Sprite BackgroundSprite { get { return _backgroundSprite; } private set { _backgroundSprite = value; } }
    public AudioClip TextAudio { get { return _textAudio; } private set { _textAudio = value; } }
}
