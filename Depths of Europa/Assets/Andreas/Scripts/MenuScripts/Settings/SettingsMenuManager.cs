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
        _resolutionDropdown.onValueChanged.AddListener(delegate { Screen.SetResolution(_screenResolutions[_resolutionDropdown.value].width,
            _screenResolutions[_resolutionDropdown.value].height, false); });
        for(int i = 0; i < _screenResolutions.Length;i++)
        {
            _resolutionDropdown.options[i].text = ResolutionToString(_screenResolutions[i]);
            _resolutionDropdown.value = i;
            _resolutionDropdown.options.Add(new Dropdown.OptionData(_resolutionDropdown.options[i].text));
        }
	}

    string ResolutionToString(Resolution res)
    {
        return res.width + "x" + res.height;
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
