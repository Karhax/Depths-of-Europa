using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsMenuManager : MonoBehaviour {

    [SerializeField] string _menuSceneName;
    [SerializeField] Dropdown _resolutionDropdown;
    [SerializeField] Toggle _fullScreenToggle;
    float _soundVolumeMaster, _soundVolumeMusic, _soundVolumeSFX;
    bool _isFullScreen;
    

    Resolution[] _screenResolutions;

    // Use this for initialization
    void Start() {
        _screenResolutions = Screen.resolutions;
        //_ResolutionQuickSort(_screenResolutions, 0, _screenResolutions.Length-1);
        _isFullScreen = Screen.fullScreen;
        _fullScreenToggle.isOn = _isFullScreen;
        _fullScreenToggle.onValueChanged.AddListener(delegate { _isFullScreen = _fullScreenToggle.isOn; Screen.fullScreen = _isFullScreen; });
        

        for(int i = 0; i < _screenResolutions.Length;i++)
        {

            _resolutionDropdown.options[i].text = ResolutionToString(_screenResolutions[i]);
            //Debug.Log(ResolutionToString(_screenResolutions[i]));
            _resolutionDropdown.value = i;
            if((i != _screenResolutions.Length-1))
            _resolutionDropdown.options.Add(new Dropdown.OptionData(_resolutionDropdown.options[i].text));
        }

        Resolution currentResolution = new Resolution();
        currentResolution.width = Screen.width;
        currentResolution.height = Screen.height;
        //Debug.Log(_screenResolutions.FindResolutionInArray(currentResolution));
        _resolutionDropdown.value = _screenResolutions.FindResolutionInArray(currentResolution);
        _resolutionDropdown.RefreshShownValue();
        _resolutionDropdown.onValueChanged.AddListener(delegate {
            Screen.SetResolution(_screenResolutions[_resolutionDropdown.value].width,
            _screenResolutions[_resolutionDropdown.value].height, _isFullScreen);
        });
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



    public void SetMasterVolume(float volume)
    {
        _soundVolumeMaster = volume;
    }
    public void SetMusicVolume(float volume)
    {
        _soundVolumeMusic = volume;
    }
    public void SetSFXVolume(float volume)
    {
        _soundVolumeSFX = volume;
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
}
