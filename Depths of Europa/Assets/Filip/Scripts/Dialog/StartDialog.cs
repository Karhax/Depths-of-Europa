using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Statics;
using UnityEngine.UI;

public class StartDialog : MonoBehaviour
{
    public delegate void DialogOverDel();
    public event DialogOverDel DialogOverEvent;

    [SerializeField] protected DialogBoxScriptableObject _thisDialogBoxScriptableObject;
    [SerializeField] GameObject _dialog;

    Dialog _dialogScript;

    protected bool _dialogPlaying = false;

    private void Awake()
    {
        _dialogScript = _dialog.GetComponent<Dialog>();
        _dialog.SetActive(false);
    }

    private void OnDisable()
    {
        _dialogScript.DialogOverEvent -= DialogOver;
    }

    private void OnEnable()
    {
        _dialogScript.DialogOverEvent += DialogOver;
    }

    public bool StartDialogs(DialogBoxScriptableObject dialog = null)
    {
        if (dialog != null)
            _thisDialogBoxScriptableObject = dialog;

        if (!_dialogPlaying)
        {
            _dialogPlaying = true;
            _dialog.SetActive(true);
            _dialogScript.StartAllDialogs(_thisDialogBoxScriptableObject);

            return true;
        }

        return false;
    }

    private void DialogOver()
    {
        _dialog.SetActive(false);
        _dialogPlaying = false;

        if (DialogOverEvent != null)
            DialogOverEvent.Invoke();
    }
}
