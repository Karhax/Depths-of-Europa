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
    [SerializeField] AudioSource _textAudioSource;
    [SerializeField] AudioSource _voiceAudioSource;

    bool _dialogPlaying = false;

    DialogBoxScriptableObject _currentScriptableObject;
    int _currentDialogBox = 0;

    Timer _textSpeedTimer;
    Timer _minPlayTimer = new Timer(MIN_BOX_TIME);

    bool _pressedDown = false;
    int _timesPressedSkip = 0;

    const float MIN_BOX_TIME = 0.2f;

    private void Awake()
    {
        _textSpeedTimer = new Timer( 1 / _normalTextSpeed);
    }

    private void Update()
    {
        if (_dialogPlaying)
        {
            _pressedDown = Input.GetButtonDown(GameInput.SKIP_AND_SONAR);

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
            _minPlayTimer.Time += Time.deltaTime;

            if (_pressedDown)
            {
                if (placeInText < text.Length)
                    SetSpeed(_fastTextSpeed);
                else if (_minPlayTimer.Expired())
                    canSkip = true;
            }

            if (placeInText < text.Length)
            {
                if (_textSpeedTimer.Expired())
                {
                    _textSpeedTimer.Reset();
                    stringBuilder.Append(text[placeInText++].ToString());

                    if (placeInText >= text.Length)
                        _textAudioSource.Stop();
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

    private void SetSprite(Sprite newSprite, Image image)
    {
        if (newSprite != null)
        {
            image.gameObject.SetActive(true);
            image.sprite = newSprite;
        }
        else
            image.gameObject.SetActive(false);
    }

    private void SetBoxSettings(DialogBoxObject boxObject)
    {
        _dialogText.font = boxObject.Font;

        SetSprite(boxObject.LeftSprite, _leftImage);
        SetSprite(boxObject.RightSprite, _rightImage);

        if (boxObject.RightTalking)
            TintSprites(_tintColorWhenNotTalking, Color.white);
        else
            TintSprites(Color.white, _tintColorWhenNotTalking);

        PlayAudioClip(_textAudioSource, boxObject.TextAudio);
        PlayAudioClip(_voiceAudioSource, boxObject.VoiceAudio);

        _minPlayTimer.Reset();

        if (boxObject.VoiceAudio != null)
            _minPlayTimer.Duration = _voiceAudioSource.clip.length;
        else
            _minPlayTimer.Duration = MIN_BOX_TIME;
    }

    private void PlayAudioClip(AudioSource source, AudioClip clip)
    {
        if (clip != null)
        {
            source.clip = clip;
            source.Play();
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
