using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Statics;
using UnityEngine.UI;

public class StartDialog : MonoBehaviour
{
    [SerializeField] DialogBoxScriptableObject _thisDialogBoxScriptableObject;
    [SerializeField] GameObject _dialog;

    Dialog _dialogScript;

    bool _dialogPlaying = false;

    private void Awake()
    {
        _dialogScript = _dialog.GetComponent<Dialog>();
    }

    private void Start()
    {
        StartDialogs();
    }

    private void OnDisable()
    {
        _dialogScript.DialogOverEvent -= DialogOver;
    }

    private void OnEnable()
    {
        _dialogScript.DialogOverEvent += DialogOver;
    }

    public void StartDialogs()
    {
        if (!_dialogPlaying)
        {
            _dialogPlaying = true;
            _dialog.SetActive(true);
            _dialogScript.StartAllDialogs(_thisDialogBoxScriptableObject);
        }
    }

    private void DialogOver()
    {
        _dialog.SetActive(false);
        _dialogPlaying = false;

        //LINK WITH STATION SCRIPT
    }
}
