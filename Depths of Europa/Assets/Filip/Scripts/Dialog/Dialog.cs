using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Statics;
using System.Text;

public class Dialog : MonoBehaviour
{
    public delegate void DialogOver();
    public event DialogOver DialogOverEvent;

    [Header("Settings")]

    [SerializeField, Range(1, 300)] float _normalTextSpeed;
    [SerializeField, Range(1, 300)] float _fastTextSpeed;
    [SerializeField] Color _tintColorWhenNotTalking;

    [Header("Drop")]

    [SerializeField] Text _dialogText;
    [SerializeField] Image _leftImage;
    [SerializeField] Image _rightImage;
    [SerializeField] AudioSource _audioSource;

    bool _dialogPlaying = false;

    DialogBoxScriptableObject _currentScriptableObject;
    int _currentDialogBox = 0;

    Timer _textSpeedTimer;

    bool _pressedDown = false;
    bool _buttonUp = false;
    int _timesPressedSkip = 0;

    private void Awake()
    {
        _textSpeedTimer = new Timer( 1 / _normalTextSpeed);
    }

    private void Update()
    {
        if (_dialogPlaying)
        {
            _pressedDown = Input.GetButtonDown(GameInput.TEXT_SKIP);
            _buttonUp = Input.GetButtonUp(GameInput.TEXT_SKIP);

            if (_pressedDown)
                _timesPressedSkip++;
        }
    }

    public void StartAllDialogs(DialogBoxScriptableObject scriptableObject)
    {
        _timesPressedSkip = 0;
        _dialogPlaying = true;
        _currentDialogBox = 0;
        _currentScriptableObject = scriptableObject;

        StartNextDialogBox();
    }

    private bool StartNextDialogBox()
    {
        if (_currentScriptableObject.DialogBoxes.Length > _currentDialogBox)
        {
            StartCoroutine(DoDialogBox(_currentScriptableObject.DialogBoxes[_currentDialogBox]));
            return true;
        }

        return false;
    }

    IEnumerator DoDialogBox(DialogBoxObject boxObject)
    {
        bool canSkip = false;
        string text = boxObject.DialogText;
        int placeInText = 0;

        StringBuilder stringBuilder = new StringBuilder();

        SetBoxSettings(boxObject);

        while (!canSkip)
        {
            _textSpeedTimer.Time += Time.deltaTime;

            if (_pressedDown)
            {
                if (placeInText < text.Length)
                    SetSpeed(_fastTextSpeed);
                else
                    canSkip = true;
            }

            if (placeInText < text.Length)
            {
                if (_textSpeedTimer.Expired())
                {
                    _textSpeedTimer.Reset();
                    stringBuilder.Append(text[placeInText++].ToString());

                    if (placeInText >= text.Length)
                        _audioSource.Stop();
                }
            }

            _dialogText.text = stringBuilder.ToString();

            if (_timesPressedSkip >= 2)
            {
                placeInText = text.Length;
                _dialogText.text = text;
            }

            yield return new WaitForEndOfFrame();
        }

        DoAnotherDialogBox();
    }

    private void DoAnotherDialogBox()
    {
        ResetAfterBox();

        if (!StartNextDialogBox())
        {
            _dialogPlaying = false;

            if (DialogOverEvent != null)
                DialogOverEvent.Invoke();
        }
    }

    private void SetBoxSettings(DialogBoxObject boxObject)
    {
        _dialogText.font = boxObject.Font;
        _leftImage.sprite = boxObject.LeftSprite;
        _rightImage.sprite = boxObject.RightSprite;

        if (boxObject.RightTalking)
            TintSprites(_tintColorWhenNotTalking, Color.white);
        else
            TintSprites(Color.white, _tintColorWhenNotTalking);

        if (boxObject.AudioClip != null)
        {
            _audioSource.clip = boxObject.AudioClip;
            _audioSource.Play();
        }
    }

    private void TintSprites(Color left, Color right)
    {
        _leftImage.color = left;
        _rightImage.color = right;
    }

    private void ResetAfterBox()
    {
        _timesPressedSkip = 0;
        SetSpeed(_normalTextSpeed);
        _currentDialogBox++;
    }

    private void SetSpeed(float speed)
    {
        _textSpeedTimer.Duration = 1 / speed;
    }
}
