using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneEndHandling : MonoBehaviour {

    [SerializeField] private string _nextSceneName = "Level_1";

    public void CutsceneEndReaction()
    {
        SceneManager.LoadScene(_nextSceneName);
    }
}
