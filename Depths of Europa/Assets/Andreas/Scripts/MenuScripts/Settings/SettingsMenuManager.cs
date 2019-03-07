using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsMenuManager : MonoBehaviour {

    [SerializeField] string _menuSceneName;
    [SerializeField] Dropdown _resolutionDropdown;
    [SerializeField] Toggle _fullScreenToggle;
    [SerializeField] Text _masterVolumeText, _musicVolumeText, _sfxVolumeText, _dialogVolumeText, _secondMarkerText, _titleText;
    [SerializeField] Slider _masterVolumeSlider, _musicVolumeSlider, _sfxVolumeSlider,_dialogVolumeSlider;
    [SerializeField] AudioMixer _audioMixer;
    [SerializeField] GameObject _confirmMenu;
    float _soundVolumeMaster, _soundVolumeMusic, _soundVolumeSFX, _soundVolumeDialog;
    bool _isFullScreen, _keepNewResolution, _revertToOldResolution, _oldFullScreenMode;
    Resolution _previousResolution;
    readonly float RESET_RESOLUTION_CONFIRM_DURATION = 15;
    Timer _secondsToResolutionReset;
    
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
        _oldSettings._soundVolumeMaster = _audioMixer.GetVolumeValue("Master", -80, 0);
        _oldSettings._soundVolumeMusic = _audioMixer.GetVolumeValue("Music", -80, 0);
        _oldSettings._soundVolumeDialog = _audioMixer.GetVolumeValue("Dialog", -80, 0);
        _oldSettings._soundVolumeSFX = _audioMixer.GetVolumeValue("SFX", -80, 0);
        #endregion

        //_ResolutionQuickSort(_screenResolutions, 0, _screenResolutions.Length-1);
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
        _sfxVolumeSlider.value = _audioMixer.GetVolumeValue("SFX", -80, 0); _musicVolumeSlider.value = _audioMixer.GetVolumeValue("Music", -80, 0);
        _masterVolumeSlider.value =_audioMixer.GetVolumeValue("Master", -80, 0); _dialogVolumeSlider.value =  _audioMixer.GetVolumeValue("Dialog", -80, 0);
        Debug.Log(_audioMixer.GetVolumeValue("Music", -80, 0));     

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
    void Update () {
		
	}



    void SetMasterVolume(Slider volume)
    {
        _soundVolumeMaster = volume.value;
        _masterVolumeText.text = volume.SliderValueToPercentString();
        _audioMixer.SetFloat("Master", volume.value != 0 ? Mathf.Lerp(-80, 0, volume.value):-80);
        Debug.Log(_audioMixer.GetVolumeValue("Master"));
    }
    void SetMusicVolume(Slider volume)
    {
        _soundVolumeMusic = volume.value;
        _musicVolumeText.text = volume.SliderValueToPercentString();
        _audioMixer.SetFloat("Music", volume.value != 0 ? Mathf.Lerp(-80, 0, volume.value) : -80);
    }
    void SetSFXVolume(Slider volume)
    {
        _soundVolumeSFX = volume.value;
        _sfxVolumeText.text = volume.SliderValueToPercentString();
        _audioMixer.SetFloat("SFX", volume.value != 0 ? Mathf.Lerp(-80, 0, volume.value) : -80);
    }
    void SetDialogVolume(Slider volume)
    {
        _soundVolumeDialog = volume.value;
        _dialogVolumeText.text = volume.SliderValueToPercentString();
        _audioMixer.SetFloat("Dialog", volume.value != 0 ? Mathf.Lerp(-80, 0, volume.value) : -80);
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
