using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsMenuManager : MonoBehaviour {

    [SerializeField] string _menuSceneName;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void BackToMenu()
    {
        if (_menuSceneName != "" && SceneManager.GetSceneByName(_menuSceneName) != null)
            SceneManager.LoadScene(_menuSceneName);
        else
            Debug.LogWarning("No scene with that name exists, please ensure that you have entered the scene name correctly");
    }
}
