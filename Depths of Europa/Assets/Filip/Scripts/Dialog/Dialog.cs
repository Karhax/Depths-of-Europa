using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Statics;
using System.Text;

public class Dialog : MonoBehaviour
{
    [Header("Settings")]

    [SerializeField, Range(1, 300)] float _normalTextSpeed;
    [SerializeField, Range(1, 300)] float _fastTextSpeed;

    [Header("Drop")]

    [SerializeField] Text _dialogText;
    [SerializeField] Image _icon;
    [SerializeField] AudioSource _audioSource;

    DialogBoxScriptableObject _currentScriptableObject;
    int _currentDialogBox = 0;

    Timer _textSpeedTimer;

    bool _pressedDown = false;
    bool _buttonUp = false;

    private void Awake()
    {
        _textSpeedTimer = new Timer( 1 / _normalTextSpeed);
    }

    private void Update()
    {
        _pressedDown = Input.GetButtonDown(GameInput.TEXT_SKIP);
        _buttonUp = Input.GetButtonUp(GameInput.TEXT_SKIP);
    }

    public void StartAllDialogs(DialogBoxScriptableObject scriptableObject)
    {
        _currentDialogBox = 0;
        _currentScriptableObject = scriptableObject;

        if (_currentScriptableObject.DialogBoxes.Length > _currentDialogBox)
            StartCoroutine(DoDialogBox(_currentScriptableObject.DialogBoxes[_currentDialogBox]));       
    }

    IEnumerator DoDialogBox(DialogBoxObject dialogObject)
    {
        int timesPressedSkip = 0;
        bool canSkip = false;

        string text = dialogObject.DialogText;
        int placeInText = 0;

        StringBuilder stringBuilder = new StringBuilder();

        _dialogText.font = dialogObject.Font;
        _icon.sprite = dialogObject.Icon;

        if (dialogObject.AudioClip != null)
        {
            _audioSource.clip = dialogObject.AudioClip;
            _audioSource.Play();
        }
        else
        {
            _audioSource.Stop();
        }

        while (!canSkip)
        {
            _textSpeedTimer.Time += Time.deltaTime;

            if (_pressedDown)
            {
                if (placeInText < text.Length)
                    _textSpeedTimer.Duration = 1 / _fastTextSpeed;
                else
                    canSkip = true;

                /*if (timesPressedSkip < 2)
                {
                    timesPressedSkip++;
                    placeInText = text.Length;
                    _dialogText.text = text;
                }*/
            }
            else if (_buttonUp)
                _textSpeedTimer.Duration = 1 / _normalTextSpeed;


            if (placeInText < text.Length)
            {
                if (_textSpeedTimer.Expired())
                {
                    _textSpeedTimer.Reset();
                    stringBuilder.Append(text[placeInText++].ToString());
                }
            }

            _dialogText.text = stringBuilder.ToString();

            yield return new WaitForEndOfFrame();
        }

        Debug.Log("NEW");

        DoAnotherDialogBox();
    }

    private void DoAnotherDialogBox()
    {
        _textSpeedTimer.Duration = 1 / _normalTextSpeed;
        _currentDialogBox++;

        if (_currentDialogBox < _currentScriptableObject.DialogBoxes.Length)
            StartCoroutine(DoDialogBox(_currentScriptableObject.DialogBoxes[_currentDialogBox]));
    }
}
