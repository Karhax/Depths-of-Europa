using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
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
        if (_inputManager == null)
            _inputManager = this;
        else
            Destroy(this);

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
    }

    private string GetNextName(SerializedProperty axis, bool inChildren = false)
    {
        axis.Next(inChildren);
        return axis.stringValue;
    }

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
