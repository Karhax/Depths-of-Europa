using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Statics;
using UnityEngine.UI;

public class StartDialog : MonoBehaviour
{
    [SerializeField] DialogBoxScriptableObject _thisDialogBoxScriptableObject;

    Dialog _dialogScript;

    private void Awake()
    {
        _dialogScript = GetComponentInChildren<Dialog>();
    }

    private void Start()
    {
        StartDialogs();
    }

    public void StartDialogs()
    {
        _dialogScript.StartAllDialogs(_thisDialogBoxScriptableObject);
    }
}
