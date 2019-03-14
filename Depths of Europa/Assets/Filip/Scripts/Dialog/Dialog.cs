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
    [SerializeField, Range(0.1f, 50f)] float _timeToForceSkip;
    [SerializeField] bool _willForceSkip = true;

    [Header("Drop")]

    [SerializeField] Image _dialogBox;
    [SerializeField] Text _dialogText;
    [SerializeField] Image _backgroundImage;
    [SerializeField] Image _leftImage;
    [SerializeField] Image _rightImage;
    [SerializeField] AudioSource _textAudioSource;
    [SerializeField] AudioSource _voiceAudioSource;

    [SerializeField] Image _rightNameBox;
    [SerializeField] Image _leftNameBox;
    [SerializeField] Text _rigthNameText;
    [SerializeField] Text _leftNameText;

    public Image DialogBox { get { return _dialogBox; } }
    public Text DialogText { get { return _dialogText; } }
    public Image BackgroundImage { get { return _backgroundImage; } }
    public Image LeftImage { get { return _leftImage; } }
    public Image RightImage { get { return _rightImage; } }
    public Image RightNameBox { get { return _rightNameBox; } }
    public Image LeftNameBox { get { return _leftNameBox; } }
    public Text RightNameText { get { return _rigthNameText; } }
    public Text LeftNameText { get { return _leftNameText; } }

    bool _dialogPlaying = false;

    DialogBoxScriptableObject _currentScriptableObject;

    List<DialogEffectBase> _currentEffects = new List<DialogEffectBase>();
    DialogEffectColor _currentColorEffect;
    DialogEffectShake _currentShakeEffect;

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
        if (GameManager.CameraObject != null)
        {
            _pauseMenuScript = GameManager.CameraObject.GetComponentInChildren<PauseMenuScript>();

            if (_pauseMenuScript != null)
                _pauseMenuScript.PauseState += Paused;
        }
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

            if (_currentColorEffect != null)
                _currentColorEffect.UpdateEffect();
            if (_currentShakeEffect != null)
                _currentShakeEffect.UpdateEffect();

            if (_currentEffects.Count > 0)
            {
                foreach ( DialogEffectBase effect in _currentEffects)
                {
                    effect.UpdateEffect();
                }
            }
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

            if (_pressedDown || (_forceSkipTimer.Expired() && _willForceSkip))
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

    private DialogEffectBase SetNewEffect(DialogEffectBase effect)
    {
        if (effect == null)
            return null;

        DialogEffectBase newEffect = Instantiate(effect);
        newEffect.SetUpEffect(this);
        return newEffect;
    }

    private void SetBoxSettings(DialogBoxObject boxObject)
    {
        if (_currentColorEffect != null && (boxObject.StopColorEffect || boxObject.ColorEffect != null))
        {
            _currentColorEffect.ResetEffect();
            _currentColorEffect = null;
        }
        if (_currentShakeEffect != null && (boxObject.StopShakeEffect || boxObject.ShakeEffect != null))
        {
            _currentShakeEffect.ResetEffect();
            _currentShakeEffect = null;
        }

        if (_currentColorEffect == null)
            _currentColorEffect = (DialogEffectColor)SetNewEffect(boxObject.ColorEffect);
        if (_currentShakeEffect == null)
            _currentShakeEffect = (DialogEffectShake)SetNewEffect(boxObject.ShakeEffect);

        if (boxObject.Effects != null)
        {
            for (int i = 0; i < boxObject.Effects.Length; i++)
            {
                DialogEffectBase newEffect = SetNewEffect(boxObject.Effects[i]);
                if (newEffect != null)
                    _currentEffects.Add(newEffect);
            }
        }

        if (boxObject.BackgroundSprite != null && _backgroundImage != null)
            _backgroundImage.sprite = boxObject.BackgroundSprite;

        SetCharacterSettings(boxObject.RightCharacter, _rightImage, boxObject.RightTalking, _rightNameBox, _rigthNameText);
        SetCharacterSettings(boxObject.LeftCharacter, _leftImage, !boxObject.RightTalking, _leftNameBox, _leftNameText);

        PlayAudioClip(_textAudioSource, boxObject.TextAudio);     
    }

    private void SetCharacterSettings(CharacterScriptableObject characterObject, Image characterImage, bool talking, Image nameBoxImage, Text nameText)
    {
        if (characterImage != null)
            SetSprite(characterObject.Sprite, characterImage);

        if (talking)
        {
            if (nameBoxImage != null)
                nameBoxImage.gameObject.SetActive(true);

            if (nameBoxImage != null)
            {
                nameText.font = characterObject.Font;
                nameText.text = characterObject.Name;
            }
                
            if (characterImage != null)
                TintSprite(characterImage, _tintColorWhenTalking);

            if (characterObject.Font != null)
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
            if (nameBoxImage != null)
                nameBoxImage.gameObject.SetActive(false);

            if (characterImage != null)
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
        foreach (DialogEffectBase effect in _currentEffects)
        {
            effect.ResetEffect();
        }
        _currentEffects.Clear();

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