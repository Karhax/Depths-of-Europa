using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/*[CustomEditor(typeof(LineSetup))]
[CanEditMultipleObjects]
public class LineSetupEditor : Editor {


    SerializedProperty _lines;

    void OnEnable()
    {
       _lines = serializedObject.FindProperty("_lines");
    }
    
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        serializedObject.Update();
        Handles.BeginGUI();
        Rect bound = _lines.rectValue;
        
        Handles.EndGUI();
        serializedObject.ApplyModifiedProperties();
    }
}
*/