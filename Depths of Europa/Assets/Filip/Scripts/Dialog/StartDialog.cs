using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Statics;
using UnityEngine.UI;

public class StartDialog : MonoBehaviour
{
    public delegate void DialogOverDel();
    public event DialogOverDel DialogOverEvent;

    [SerializeField] bool _playOnAwake = false;
    [SerializeField] protected DialogBoxScriptableObject _thisDialogBoxScriptableObject;
    [SerializeField] GameObject _dialog;
    [SerializeField] GameObject _tutorialDialog;

    Dialog _dialogScript;
    Dialog _tutorialDialogScript;

    protected bool _dialogPlaying = false;

    private void Awake()
    {
        _dialogScript = _dialog.GetComponent<Dialog>();
        _dialog.SetActive(false);

        if (_tutorialDialog != null)
        {
            _tutorialDialogScript = _tutorialDialog.GetComponent<Dialog>();
            _tutorialDialog.SetActive(false);
        }
    }

    private void Start()
    {
        if (_playOnAwake)
            StartDialogs();
    }

    private void OnDisable()
    {
        _dialogScript.DialogOverEvent -= DialogOver;

        if (_tutorialDialogScript != null)
            _tutorialDialogScript.DialogOverEvent -= DialogOver;
    }

    private void OnEnable()
    {
        _dialogScript.DialogOverEvent += DialogOver;

        if (_tutorialDialogScript != null)
            _tutorialDialogScript.DialogOverEvent += DialogOver;
    }

    public bool StartDialogs(DialogBoxScriptableObject dialog = null, bool isTutorial = false)
    {
        Debug.Log(Time.time);

        if (dialog != null)
            _thisDialogBoxScriptableObject = dialog;

        if (!_dialogPlaying)
        {
            _dialogPlaying = true;

            if (isTutorial)
            {
                _tutorialDialog.SetActive(true);
                _tutorialDialogScript.StartAllDialogs(_thisDialogBoxScriptableObject);
            }
            else
            {
                _dialog.SetActive(true);
                _dialogScript.StartAllDialogs(_thisDialogBoxScriptableObject);
            }

            return true;
        }

        return false;
    }

    private void DialogOver()
    {
        if (_tutorialDialog != null)
        {
            _tutorialDialog.SetActive(false);
        }

        _dialog.SetActive(false);
        _dialogPlaying = false;

        if (DialogOverEvent != null)
            DialogOverEvent.Invoke();
    }
}
