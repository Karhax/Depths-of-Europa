using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialog Box", menuName = "Dialog/Dialog Box")]
public class DialogBoxScriptableObject : ScriptableObject
{
    [SerializeField] DialogBoxObject[] _dialogBoxes;

    public DialogBoxObject[] DialogBoxes { get { return _dialogBoxes; } private set { _dialogBoxes = value; } }
}

[System.Serializable]
public class DialogBoxObject
{
    [Header("Settings")]

    [SerializeField] bool _rightTalking = true;
    [SerializeField] CharacterScriptableObject _rightCharacter;
    [SerializeField] CharacterScriptableObject _leftCharacter;
    [SerializeField] Sprite _backgroundSprite;
    [SerializeField] AudioClip _textAudio;
    [SerializeField, TextAreaAttribute(3, 3)] string _dialogText;

    [Header("Effects")]

    [SerializeField] bool _stopPreviousColorEffect = false;
    [SerializeField] bool _stopPreviousShakeEffect = false;
    [SerializeField] DialogEffectBase[] _effects;

    public bool RightTalking { get { return _rightTalking; } private set { _rightTalking = value; } }
    public CharacterScriptableObject RightCharacter { get { return _rightCharacter; } private set { _rightCharacter = value; } }
    public CharacterScriptableObject LeftCharacter { get { return _leftCharacter; } private set { _leftCharacter = value; } }
    public string DialogText { get { return _dialogText; } private set { _dialogText = value; } }
    public Sprite BackgroundSprite { get { return _backgroundSprite; } private set { _backgroundSprite = value; } }
    public AudioClip TextAudio { get { return _textAudio; } private set { _textAudio = value; } }
    public DialogEffectBase[] Effects { get { return _effects; } private set { _effects = value; } }
    public bool StopColorEffect { get { return _stopPreviousColorEffect; } private set { _stopPreviousColorEffect = value; } }
    public bool StopShakeEffect { get { return _stopPreviousShakeEffect; } private set { _stopPreviousShakeEffect = value; } }
}
