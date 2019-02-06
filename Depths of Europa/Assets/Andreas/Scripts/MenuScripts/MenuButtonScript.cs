using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuButtonScript : MonoBehaviour {

    [SerializeField, Tooltip("The Name of the scene that is to be loaded when the start game button is clicked.")] string _gameStartSceneName;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void StartGame()
    {
        if (SceneManager.GetSceneByName(_gameStartSceneName) != null)
            SceneManager.LoadScene(_gameStartSceneName);
        else
            Debug.LogWarning("No scene with that name exists, please ensure that you have entered the scene name correctly");
    }
    public void Settings()
    {
        Debug.Log("Settings menu and settings menu opening not implimented");
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
