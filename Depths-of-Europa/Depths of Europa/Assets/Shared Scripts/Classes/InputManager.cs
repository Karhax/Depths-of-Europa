using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using Statics;

public struct Axis
{
    public string Name { get; private set; }
    public string DescriptiveName { get; private set; }
    public string DescriptiveNegativeName { get; private set; }
    public string NegativeButton { get; private set; }
    public string PositiveButton { get; private set; }
    public string AltNegativeButton { get; private set; }
    public string AltPositiveButton { get; private set; }

    public string FullButtonName { get; private set; }
    public string FullOrdinaryButtonName { get; private set; }
    public string FullNegativeButtonName { get; private set; }

    public Axis(string name, string descriptiveName, string descriptiveNegativeName, string negativeButton, string positiveButton, string altNegativeButton, string altPositiveButton)
    {
        Name = name;
        DescriptiveName = descriptiveName;
        DescriptiveNegativeName = descriptiveNegativeName;
        NegativeButton = negativeButton;
        PositiveButton = positiveButton;
        AltNegativeButton = altNegativeButton;
        AltPositiveButton = altPositiveButton;

        if (NegativeButton == string.Empty)
            FullOrdinaryButtonName = "(" + PositiveButton + ")";
        else
            FullOrdinaryButtonName = "(" + NegativeButton + ") & (" + PositiveButton + ")";

        if (altPositiveButton == string.Empty)
            FullNegativeButtonName = "";
        else if (altNegativeButton == string.Empty)
            FullNegativeButtonName = "(" + altPositiveButton + ")";
        else
            FullNegativeButtonName = "(" + AltNegativeButton + ") & (" + AltPositiveButton + ")";

        if (FullNegativeButtonName == string.Empty)
            FullButtonName = FullOrdinaryButtonName;
        else
            FullButtonName = FullOrdinaryButtonName + " / " + FullNegativeButtonName;
    }
}

public class InputManager : MonoBehaviour
{
    static InputManager _inputManager = null;
    static Dictionary<string, Axis> _allAxis = new Dictionary<string, Axis>();

    private void Awake()
    {
        bool inEditor = false;

        if (_inputManager == null)
            _inputManager = this;
        else
            Destroy(this);
#if UNITY_EDITOR
        Object inputManager = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0];
        SerializedObject obj = new SerializedObject(inputManager);
        SerializedProperty axisArray = obj.FindProperty("m_Axes");
        if (axisArray.arraySize == 0)
            Debug.LogWarning("No Axes");

        for (int i = 0; i < axisArray.arraySize; i++)
        {
            SerializedProperty axis = axisArray.GetArrayElementAtIndex(i);

            string name = GetNextName(axis, true);
            string descriptiveName = GetNextName(axis);
            string descriptiveNegativeName = GetNextName(axis);
            string negativeButton = GetNextName(axis);
            string positiveButton = GetNextName(axis);
            string altNegativeButton = GetNextName(axis);
            string altPositiveButton = GetNextName(axis);

            _allAxis.Add(name, new Axis(name, descriptiveName, descriptiveNegativeName, negativeButton, positiveButton, altNegativeButton, altPositiveButton));
        }
        inEditor = true;
#endif
        if (!inEditor)
        {
            _allAxis.Add(GameInput.HORIZONTAL, new Axis(GameInput.HORIZONTAL, string.Empty, string.Empty, "left", "right", "a", "d"));
            _allAxis.Add(GameInput.VERTICAL, new Axis(GameInput.VERTICAL, string.Empty, string.Empty, "down", "up", "s", "w"));
            _allAxis.Add(GameInput.ACTION, new Axis(GameInput.ACTION, "Headlights", string.Empty, string.Empty, "mouse 0", string.Empty, string.Empty));
            _allAxis.Add(GameInput.ACTION2, new Axis(GameInput.ACTION2, "Shoot Flare", string.Empty, string.Empty, "mouse 1", string.Empty, string.Empty));
            _allAxis.Add(GameInput.ACTION3, new Axis(GameInput.ACTION3, "Turn Lights On/Off", string.Empty, string.Empty, "mouse 2", string.Empty, string.Empty));
            _allAxis.Add(GameInput.SKIP_DIALOG, new Axis(GameInput.SKIP_DIALOG, "Skip Dialog", string.Empty, string.Empty, "space", string.Empty, string.Empty));
            _allAxis.Add(GameInput.SUBMIT, new Axis(GameInput.SUBMIT, string.Empty, string.Empty, string.Empty, "enter", string.Empty, string.Empty));
            _allAxis.Add(GameInput.CANCEL, new Axis(GameInput.CANCEL, string.Empty, string.Empty, string.Empty, "escape", string.Empty, string.Empty));
            _allAxis.Add(GameInput.ENTER_SUBMARINE, new Axis(GameInput.ENTER_SUBMARINE, string.Empty, string.Empty, string.Empty, "tab", string.Empty, string.Empty));
            _allAxis.Add(GameInput.SONAR, new Axis(GameInput.SONAR, "Shoot Sonar", string.Empty, string.Empty, "f", string.Empty, string.Empty));
            _allAxis.Add(GameInput.SIDE_STRAFE_LEFT, new Axis(GameInput.SIDE_STRAFE_LEFT, "Side Strafe Left", string.Empty, string.Empty, "q", string.Empty, string.Empty));
            _allAxis.Add(GameInput.SIDE_STRAFE_RIGHT, new Axis(GameInput.SIDE_STRAFE_RIGHT, "Side Strafe Right", string.Empty, string.Empty, "e", string.Empty, string.Empty));
            _allAxis.Add(GameInput.BUBBLE_BLAST, new Axis(GameInput.BUBBLE_BLAST, "BubbleBlast", string.Empty, string.Empty, "left shift", string.Empty, string.Empty));
        }
    }

#if UNITY_EDITOR
    private string GetNextName(SerializedProperty axis, bool inChildren = false)
    {
        axis.Next(inChildren);
        return axis.stringValue;
    }
#endif
    public static Axis GetAxis(string axisName)
    {
        try
        {
            return _allAxis[axisName];
        }
        catch
        {
            throw new System.Exception("This input does not exist! - " + axisName);
        }
    }
}
