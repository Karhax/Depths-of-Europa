using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsMenuManager : MonoBehaviour {

    [SerializeField] string _menuSceneName;
    [SerializeField] Dropdown _resolutionDropdown;
    float _soundVolumeMaster, _soundVolumeMusic;

    Resolution[] _screenResolutions;

	// Use this for initialization
	void Start () {
        _screenResolutions = Screen.resolutions;

        //_ResolutionQuickSort(_screenResolutions, 0, _screenResolutions.Length-1);

        
        _resolutionDropdown.onValueChanged.AddListener(delegate { Screen.SetResolution(_screenResolutions[_resolutionDropdown.value].width,
            _screenResolutions[_resolutionDropdown.value].height, false); });
        for(int i = 0; i < _screenResolutions.Length;i++)
        {

            _resolutionDropdown.options[i].text = ResolutionToString(_screenResolutions[i]);
            Debug.Log(ResolutionToString(_screenResolutions[i]));
            _resolutionDropdown.value = i;
            if((i != _screenResolutions.Length-1))
            _resolutionDropdown.options.Add(new Dropdown.OptionData(_resolutionDropdown.options[i].text));
        }
        
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
