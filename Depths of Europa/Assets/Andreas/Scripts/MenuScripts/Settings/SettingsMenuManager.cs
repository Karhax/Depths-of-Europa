using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.PostProcessing;

public class SettingsMenuManager : MonoBehaviour {

    [SerializeField] string _menuSceneName;
    [SerializeField] Dropdown _resolutionDropdown;
    [SerializeField] Toggle _fullScreenToggle;
    [SerializeField] Text _masterVolumeText, _musicVolumeText, _sfxVolumeText, _dialogVolumeText, _secondMarkerText, _titleText;
    [SerializeField] Slider _masterVolumeSlider, _musicVolumeSlider, _sfxVolumeSlider, _dialogVolumeSlider, _gammaSlider;
    [SerializeField] AudioMixer _audioMixer;
    [SerializeField] GameObject _confirmMenu;
    [SerializeField] PostProcessingProfile _postProcessor;
    [SerializeField] Image _mainSettings, _gammaScreen, _mainMenu;
    [SerializeField] Camera _camera;
    [SerializeField] GameObject _screenParent;


    float _soundVolumeMaster, _soundVolumeMusic, _soundVolumeSFX, _soundVolumeDialog;
    bool _isFullScreen, _keepNewResolution, _revertToOldResolution, _oldFullScreenMode;
    Resolution _previousResolution;
    readonly float RESET_RESOLUTION_CONFIRM_DURATION = 15, DECIMAL_TO_DECIBEL = 20, INVERSE_LOG = 10;
    Timer _secondsToResolutionReset;
    //float binky;
    struct Settings
    {
        internal float _soundVolumeMaster, _soundVolumeMusic, _soundVolumeSFX, _soundVolumeDialog;
        internal bool _isFullScreen;
        internal Resolution _resolution;
    }

    Settings _oldSettings;
    Resolution[] _screenResolutions;

    Resolution GetActualResolution()
    {
        Resolution currentResolution = new Resolution();
        currentResolution.width = Screen.width;
        currentResolution.height = Screen.height;
        return currentResolution;
    }
    Resolution GetSelectedResolution()
    {
        Resolution currentResolution = new Resolution();
        currentResolution.width = _screenResolutions[_resolutionDropdown.value].width;
        currentResolution.height = _screenResolutions[_resolutionDropdown.value].height;
        return currentResolution;
    }

    // Use this for initialization
    void Start() {
        #region Faffy Setup Stuff
        Resolution currentResolution = GetActualResolution();
        _screenResolutions = Screen.resolutions;
        _previousResolution = currentResolution;

        _oldSettings = new Settings();
        _oldSettings._isFullScreen = Screen.fullScreen;
        _oldSettings._resolution = currentResolution;
        _oldSettings._soundVolumeMaster = Mathf.Pow(INVERSE_LOG, (_audioMixer.GetVolumeValue("Master")/ DECIMAL_TO_DECIBEL));
        _oldSettings._soundVolumeMusic = Mathf.Pow(INVERSE_LOG, (_audioMixer.GetVolumeValue("Music") / DECIMAL_TO_DECIBEL));
        _oldSettings._soundVolumeDialog = Mathf.Pow(INVERSE_LOG, (_audioMixer.GetVolumeValue("Dialog") / DECIMAL_TO_DECIBEL));
        _oldSettings._soundVolumeSFX = Mathf.Pow(INVERSE_LOG, (_audioMixer.GetVolumeValue("SFX") / DECIMAL_TO_DECIBEL));
        #endregion

        _ResolutionQuickSort(_screenResolutions, 0, _screenResolutions.Length-1);
        _isFullScreen = Screen.fullScreen;
        _fullScreenToggle.isOn = _isFullScreen;
        _oldFullScreenMode = _isFullScreen;
        _fullScreenToggle.onValueChanged.AddListener(delegate { _isFullScreen = _fullScreenToggle.isOn; Screen.fullScreen = _isFullScreen;
            _secondsToResolutionReset = new Timer(RESET_RESOLUTION_CONFIRM_DURATION); StartCoroutine(ChangeWindowMode()); });

        for(int i = 0; i < _screenResolutions.Length;i++)
        {

            _resolutionDropdown.options[i].text = ResolutionToString(_screenResolutions[i]);
            //Debug.Log(ResolutionToString(_screenResolutions[i]));
            _resolutionDropdown.value = i;
            if((i != _screenResolutions.Length-1))
            _resolutionDropdown.options.Add(new Dropdown.OptionData(_resolutionDropdown.options[i].text));
        }

        //Debug.Log(_screenResolutions.FindResolutionInArray(currentResolution));
        _resolutionDropdown.value = _screenResolutions.FindResolutionInArray(currentResolution);
        _resolutionDropdown.RefreshShownValue();
        _resolutionDropdown.onValueChanged.AddListener(delegate {
            Screen.SetResolution(_screenResolutions[_resolutionDropdown.value].width,
            _screenResolutions[_resolutionDropdown.value].height, _isFullScreen); _secondsToResolutionReset = new Timer(RESET_RESOLUTION_CONFIRM_DURATION); StartCoroutine(ChangeResolution());
        });

        _masterVolumeSlider.onValueChanged.AddListener(delegate { SetMasterVolume(_masterVolumeSlider); });
        _musicVolumeSlider.onValueChanged.AddListener(delegate { SetMusicVolume(_musicVolumeSlider); });
        _dialogVolumeSlider.onValueChanged.AddListener(delegate { SetDialogVolume(_dialogVolumeSlider); });
        _sfxVolumeSlider.onValueChanged.AddListener(delegate { SetSFXVolume(_sfxVolumeSlider); });
        _gammaSlider.onValueChanged.AddListener(delegate { SetNewGamma(_gammaSlider); });
        _gammaSlider.GammaToSlider(_postProcessor.colorGrading.settings.colorWheels.log.power.a);
        //Debug.Log(_gammaSlider.value); Debug.Log(_postProcessor.colorGrading.settings.colorWheels.log.power.a);
        _sfxVolumeSlider.value = Mathf.Pow(INVERSE_LOG, (_audioMixer.GetVolumeValue("SFX")/ DECIMAL_TO_DECIBEL));
        _musicVolumeSlider.value = Mathf.Pow(INVERSE_LOG, (_audioMixer.GetVolumeValue("Music")/ DECIMAL_TO_DECIBEL));
        _masterVolumeSlider.value = Mathf.Pow(INVERSE_LOG, ( _audioMixer.GetVolumeValue("Master")/ DECIMAL_TO_DECIBEL));
        _dialogVolumeSlider.value = Mathf.Pow(INVERSE_LOG, (_audioMixer.GetVolumeValue("Dialog")/ DECIMAL_TO_DECIBEL));
        //Debug.Log(_audioMixer.GetVolumeValue("Music", -80, 0));     

    }

    public void MoveToGamma()
    {
        float positionDifference = Mathf.Abs(_camera.transform.position.x - _gammaScreen.transform.position.x);
        _screenParent.transform.position = new Vector2(_screenParent.transform.position.x - positionDifference, _screenParent.transform.position.y);
    }
    public void BackToSettings()
    {
        float positionDifference = Mathf.Abs(_camera.transform.position.x - _mainSettings.transform.position.x);
        _screenParent.transform.position = new Vector2(_screenParent.transform.position.x + positionDifference, _screenParent.transform.position.y);
    }

    public void BackToMainMenu()
    {
        float positionDifference = Mathf.Abs(_camera.transform.position.x - _mainMenu.transform.position.x);
        _screenParent.transform.position = new Vector2(_screenParent.transform.position.x + positionDifference, _screenParent.transform.position.y);
    }
    public void MoveToSettings()
    {
        float positionDifference = Mathf.Abs(_camera.transform.position.x - _mainSettings.transform.position.x);
        _screenParent.transform.position = new Vector2(_screenParent.transform.position.x - positionDifference, _screenParent.transform.position.y);
    }


    public void SetFullScreen(bool mode)
    {
        Screen.fullScreen = mode;
    }

    void _ResolutionQuickSort(Resolution[] data, int left, int right)
    {
        Resolution Pivot = data[left];
        int leftHold = left;
        int rightHold = right;
        while (leftHold < rightHold)
        {
            while (data[leftHold].width < Pivot.width && leftHold <= rightHold)
            {
                leftHold++;
            }
            while (data[rightHold].width > Pivot.width && rightHold >= leftHold)
            {
                rightHold--;
            }
            if (leftHold < rightHold)
            {
                Resolution tempLeft = data[leftHold];
                Resolution tempRight = data[rightHold];
                data[leftHold] = tempRight;
                data[rightHold] = tempLeft;
                if (data[rightHold].width == Pivot.width && data[leftHold].width == Pivot.width)
                {
                    leftHold++;
                }
            }
        }
        if (left < leftHold - 1)
        {
            _ResolutionQuickSort(data, left, leftHold - 1);
        }
        if (right > rightHold + 1)
        {
            _ResolutionQuickSort(data, rightHold + 1, right);
        }
    }

        string ResolutionToString(Resolution res)
    {
        return res.width + "x" + res.height + "  @ " + res.refreshRate + "HZ";
    }

    // Update is called once per frame
    //void Update () {
    //binky = Mathf.Pow(10, (_audioMixer.GetVolumeValue("Master") / DECIMAL_TO_DECIBEL));
    // }
    /* private void Update()
    {
        Debug.Log(_postProcessor.colorGrading.settings.colorWheels.log.power.a);
    }*/


    void SetNewGamma(Slider slider)
    {
        ColorGradingModel.Settings biggerSettings = _postProcessor.colorGrading.settings;
        ColorGradingModel.LogWheelsSettings settings = _postProcessor.colorGrading.settings.colorWheels.log;
        settings.power.a = slider.SliderToGamma();
        biggerSettings.colorWheels.log = settings;
        _postProcessor.colorGrading.settings = biggerSettings;
    }

    void SetMasterVolume(Slider volume)
    {
        _soundVolumeMaster = volume.value;
        _masterVolumeText.text = volume.SliderValueToPercentString();
        _audioMixer.SetFloat("Master", volume.value != 0 ?(DECIMAL_TO_DECIBEL * Mathf.Log10(volume.value)):-80);
        //Debug.Log(_audioMixer.GetVolumeValue("Master"));
    }
    void SetMusicVolume(Slider volume)
    {
        _soundVolumeMusic = volume.value;
        _musicVolumeText.text = volume.SliderValueToPercentString();
        _audioMixer.SetFloat("Music", volume.value != 0 ? (DECIMAL_TO_DECIBEL * Mathf.Log10(volume.value)) : -80);
    }
    void SetSFXVolume(Slider volume)
    {
        _soundVolumeSFX = volume.value;
        _sfxVolumeText.text = volume.SliderValueToPercentString();
        _audioMixer.SetFloat("SFX", volume.value != 0 ?(DECIMAL_TO_DECIBEL * Mathf.Log10(volume.value)) : -80);
    }
    void SetDialogVolume(Slider volume)
    {
        _soundVolumeDialog = volume.value;
        _dialogVolumeText.text = volume.SliderValueToPercentString();
        _audioMixer.SetFloat("Dialog", volume.value != 0 ? (DECIMAL_TO_DECIBEL * Mathf.Log10(volume.value)) : -80);
    }
    public void ButtonRevert()
    {
        _revertToOldResolution = true;
    }
    public void KeepNewResolution()
    {
        _keepNewResolution = true;
    }

    public void BackToMenu()
    {
        try
        {
            SceneManager.LoadScene(_menuSceneName);
        }
        catch (System.Exception)
        {
            Debug.LogError("No scene with that name exists, please ensure that you have entered the scene name correctly");
        }
    }
    IEnumerator ChangeWindowMode()
    {
        _secondsToResolutionReset.Reset();
        _titleText.text = "Keep new window mode?";
        _revertToOldResolution = false;
        _keepNewResolution = false;
        if (_confirmMenu != null)
        {
            _confirmMenu.SetActive(true);
            while (true)
            {
                if (_secondMarkerText != null)
                    _secondMarkerText.text = (_secondsToResolutionReset.Duration - _secondsToResolutionReset.Time).FloatToSecondsRemaining();
                if (_secondsToResolutionReset.Expired() || _revertToOldResolution)
                {
                    _revertToOldResolution = false;
                    _fullScreenToggle.isOn = _oldFullScreenMode;
                    //Debug.Log(_oldFullScreenMode);
                    Screen.fullScreen = _oldFullScreenMode;
                    break;
                }
                else if (_keepNewResolution)
                {
                    if (Application.isEditor)
                        _oldFullScreenMode = !_oldFullScreenMode;
                    else
                        _oldFullScreenMode = Screen.fullScreen;

                    _fullScreenToggle.isOn = _oldFullScreenMode;
                    _keepNewResolution = false;
                    break;
                }
                else
                {
                    _secondsToResolutionReset.Time += 1;
                }
                yield return new WaitForSecondsRealtime(1);


            }
            _confirmMenu.SetActive(false);
            StopAllCoroutines();
        }
    }
    IEnumerator ChangeResolution()
    {
        _secondsToResolutionReset.Reset();
        _titleText.text = "Keep new resolution?";
        _revertToOldResolution = false;
        _keepNewResolution = false;
        if (_confirmMenu != null)
        {
            _confirmMenu.SetActive(true);
            while (true)
            {
                if (_secondMarkerText != null)
                    _secondMarkerText.text = (_secondsToResolutionReset.Duration - _secondsToResolutionReset.Time).FloatToSecondsRemaining();
                if (_secondsToResolutionReset.Expired() || _revertToOldResolution)
                {
                    _revertToOldResolution = false;
                    _resolutionDropdown.value = _screenResolutions.FindResolutionInArray(_previousResolution);
                    _resolutionDropdown.RefreshShownValue();
                    Screen.SetResolution(_previousResolution.width, _previousResolution.height, _isFullScreen);
                    break;
                }
                else if(_keepNewResolution)
                {
                    _previousResolution = GetSelectedResolution();
                    _keepNewResolution = false;
                    break;
                }
                else
                {
                    _secondsToResolutionReset.Time += 1;
                }
                yield return new WaitForSecondsRealtime(1);


            }
            _confirmMenu.SetActive(false);
            StopAllCoroutines();
        }
    }
}
