using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuButtonScript : MonoBehaviour {

    [SerializeField, Tooltip("The Name of the scene that is to be loaded when the start game button is clicked.")] string _gameStartSceneName;
    [SerializeField] string _settingMenuSceneName;
	// Use this for initialization
	void Start () {
        //Time.timeScale = 1;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void StartGame()
    {
        try
        {
            SceneManager.LoadScene(_gameStartSceneName);
        }
        catch (System.Exception)
        {
            Debug.LogError("No scene with that name exists, please ensure that you have entered the scene name correctly");
        }

    }
    public void Settings()
    {
        try
        {
            SceneManager.LoadScene(_settingMenuSceneName);
        }
        catch (System.Exception)
        {
            Debug.LogError("No scene with that name exists, please ensure that you have entered the scene name correctly");
        }
    }
    public void ExitToDesktop()
    {
        if (Application.isEditor)
        {
            Debug.Log("Not possible to exit to desktop while running in editor");
        }
        else
        {
            Application.Quit();
        }
    }


}
