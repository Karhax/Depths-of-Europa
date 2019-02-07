using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Statics;

public class StartDialog : MonoBehaviour
{
    [Header("Settings")]

    [SerializeField] float _minDialogTime;

    [Header("Drop")]

    [SerializeField] DialogBoxScriptableObject _thisDialogBoxScriptableObject;

    Timer _minDialogTimer;

    private void Awake()
    {
        _minDialogTimer = new Timer(_minDialogTime);
    }

    public void StartAllDialogs()
    {

    }

    IEnumerator DoDialogBox(DialogBoxObject dialogObject)
    {
        yield return new WaitForEndOfFrame();
    }
}
