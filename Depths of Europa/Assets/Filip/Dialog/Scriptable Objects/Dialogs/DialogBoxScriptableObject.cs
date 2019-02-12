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
    [SerializeField] CharacterScriptableObject _rightCharacter;
    [SerializeField] CharacterScriptableObject _leftCharacter;
    [SerializeField, TextAreaAttribute(3, 3)] string _dialogText;
    [SerializeField] AudioClip _textAudio;

    public bool RightTalking { get { return _rightTalking; } private set { _rightTalking = value; } }
    public string DialogText { get { return _dialogText; } private set { _dialogText = value; } }
    public AudioClip TextAudio { get { return _textAudio; } private set { _textAudio = value; } }
}
