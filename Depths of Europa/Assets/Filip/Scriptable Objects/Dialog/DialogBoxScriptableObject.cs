using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialog Box", menuName = "Dialog/Dialog Box")]
public class DialogBoxScriptableObject : ScriptableObject
{
    [SerializeField] DialogBoxObject[] _dialogBoxes;

    public DialogBoxObject[] DialogBoxes { get; private set; }
}

[System.Serializable]
public class DialogBoxObject
{
    [SerializeField, TextAreaAttribute(4, 4)] string _dialogText;
    [SerializeField] Sprite _icon;
    [SerializeField] Font _font;

    public string DialogText { get; private set; }
    public string Icon { get; private set; }
    public Font Font { get; private set; }
}
