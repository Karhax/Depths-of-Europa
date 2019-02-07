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
    [SerializeField, TextAreaAttribute(4, 4)] string _dialogText;
    [SerializeField] Sprite _icon;
    [SerializeField] Font _font;
    [SerializeField] AudioClip _audioClip;

    public string DialogText { get { return _dialogText; } private set { _dialogText = value; } }
    public Sprite Icon { get { return _icon; } private set { _icon = value; } }
    public Font Font { get { return _font; } private set { _font = value; } }
    public AudioClip AudioClip { get { return _audioClip; } private set { _audioClip = value; } }
}
