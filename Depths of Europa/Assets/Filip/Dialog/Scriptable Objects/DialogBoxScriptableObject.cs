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
    [SerializeField] bool _rightTalking;
    [SerializeField] Sprite _leftSprite;
    [SerializeField] Sprite _rightSprite;
    [SerializeField, TextAreaAttribute(3, 3)] string _dialogText;
    [SerializeField] Font _font;
    [SerializeField] AudioClip _audioClip;

    public bool RightTalking { get { return _rightTalking; } set { _rightTalking = value; } }
    public string DialogText { get { return _dialogText; } private set { _dialogText = value; } }
    public Sprite LeftSprite { get { return _leftSprite; } private set { _leftSprite = value; } }
    public Sprite RightSprite { get { return _rightSprite; } private set { _rightSprite = value; } }
    public Font Font { get { return _font; } private set { _font = value; } }
    public AudioClip AudioClip { get { return _audioClip; } private set { _audioClip = value; } }
}
