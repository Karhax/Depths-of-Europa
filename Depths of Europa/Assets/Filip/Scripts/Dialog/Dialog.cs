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
    [SerializeField] Color _tintColorWhenTalking;
    [SerializeField, Range(0.1f, 10f)] float _timeToForceSkip;

    [Header("Drop")]

    [SerializeField] Text _dialogText;
    [SerializeField] Image _leftImage;
    [SerializeField] Image _rightImage;
    [SerializeField] AudioSource _textAudioSource;
    [SerializeField] AudioSource _voiceAudioSource;

    [SerializeField] Image _rightNameBox;
    [SerializeField] Image _leftNameBox;
    [SerializeField] Text _rigthNameText;
    [SerializeField] Text _leftNameText;

    bool _dialogPlaying = false;

    DialogBoxScriptableObject _currentScriptableObject;
    int _currentDialogBox = 0;

    Timer _forceSkipTimer;
    Timer _textSpeedTimer;
    const float MIN_BOX_TIME = 0.2f;
    Timer _minPlayTimer = new Timer(MIN_BOX_TIME);

    bool _pressedDown = false;
    int _timesPressedSkip = 0;
    
    bool _isPaused = false;
    PauseMenuScript _pauseMenuScript;

    float _dialogParentWidth;
    readonly char SPACE = ' ';
    readonly int AMOUNT_TIMES_PRESSED_TO_SKIP = 2;

    private void Awake()
    {
        _dialogParentWidth = _dialogText.rectTransform.rect.width;

        _forceSkipTimer = new Timer(_timeToForceSkip);
        _textSpeedTimer = new Timer( 1 / _normalTextSpeed);
    }

    private void Start()
    {
        _pauseMenuScript = GameManager.CameraObject.GetComponentInChildren<PauseMenuScript>();
        _pauseMenuScript.PauseState += Paused;
    }

    private void OnEnable()
    {
        if (_pauseMenuScript != null)
            _pauseMenuScript.PauseState += Paused;
    }

    private void OnDisable()
    {
        if (_pauseMenuScript != null)
            _pauseMenuScript.PauseState -= Paused;
    }

    private void Paused(bool state)
    {
        _isPaused = state;
    }

    private void Update()
    {
        if (_dialogPlaying && !_isPaused)
        {
            _pressedDown = Input.GetButtonDown(GameInput.SKIP_DIALOG);

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

        SetBoxSettings(boxObject);
        text = FixTextLineBreaks(text);
        StringBuilder stringBuilder = new StringBuilder();

        while (!canSkip)
        {
            _textSpeedTimer.Time += Time.deltaTime;
            _minPlayTimer.Time += Time.deltaTime;

            if (placeInText >= text.Length)
                _forceSkipTimer.Time += Time.deltaTime;

            if (_pressedDown || _forceSkipTimer.Expired())
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

            if (_timesPressedSkip >= AMOUNT_TIMES_PRESSED_TO_SKIP)
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
        SetCharacterSettings(boxObject.RightCharacter, _rightImage, boxObject.RightTalking, _rightNameBox, _rigthNameText);
        SetCharacterSettings(boxObject.LeftCharacter, _leftImage, !boxObject.RightTalking, _leftNameBox, _leftNameText);

        PlayAudioClip(_textAudioSource, boxObject.TextAudio);     
    }

    private void SetCharacterSettings(CharacterScriptableObject characterObject, Image characterImage, bool talking, Image nameBoxImage, Text nameText)
    {
        SetSprite(characterObject.Sprite, characterImage);

        if (talking)
        {
            nameBoxImage.gameObject.SetActive(true);
            nameText.font = characterObject.Font;
            nameText.text = characterObject.Name;

            TintSprite(characterImage, _tintColorWhenTalking);
            _dialogText.font = characterObject.Font;

            if (characterObject.VoiceAudio != null)
            {
                PlayAudioClip(_voiceAudioSource, characterObject.VoiceAudio);
                _minPlayTimer.Duration = _voiceAudioSource.clip.length;
            }
            else
                _minPlayTimer.Duration = MIN_BOX_TIME;

            _minPlayTimer.Reset();
        }
        else
        {
            nameBoxImage.gameObject.SetActive(false);
            TintSprite(characterImage, _tintColorWhenNotTalking);
        }
            
    }

    private void PlayAudioClip(AudioSource source, AudioClip clip)
    {
        if (clip != null)
        {
            source.clip = clip;
            source.Play();
        }
    }

    private void TintSprite(Image image, Color color)
    {
        image.color = color;
    }

    private void ResetAfterBox()
    {
        _forceSkipTimer.Reset();
        _timesPressedSkip = 0;
        SetSpeed(_normalTextSpeed);
        _currentDialogBox++;
    }

    private void SetSpeed(float speed)
    {
        _textSpeedTimer.Duration = 1 / speed;
    }

    private string FixTextLineBreaks(string text)
    {
        StringBuilder stringBuilder = new StringBuilder();
        int lastSpaceIndex = 0;

        for (int i = 0; i < text.Length; i++)
        {
            if (text[i] == SPACE)
                lastSpaceIndex = i;

            stringBuilder.Append(text[i].ToString());
            _dialogText.text = stringBuilder.ToString();

            float textWidth = LayoutUtility.GetPreferredWidth(_dialogText.rectTransform);

            if (textWidth > _dialogParentWidth && text[i] != SPACE)
                stringBuilder.Replace(SPACE.ToString(), System.Environment.NewLine, lastSpaceIndex, i - lastSpaceIndex);
        }

        _dialogText.text = string.Empty;

        return stringBuilder.ToString();
    }
}