using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogBook : MonoBehaviour
{
    [SerializeField] AudioSource _music;
    [SerializeField, Range(0.1f, 100)] float _fadeMusicSpeed = 1;
    [SerializeField] protected DialogBoxScriptableObject _thisDialogBoxScriptableObject;
    [SerializeField] GameObject _dialog;
    [SerializeField] string _nextSceneName;

    Dialog _dialogScript;

    protected bool _dialogPlaying = false;

    private void Awake()
    {
        Cursor.visible = false;

        _dialogScript = _dialog.GetComponent<Dialog>();
        _dialog.SetActive(false);
    }

    private void Start()
    {
        StartDialogs();
    }

    private void OnDisable()
    {
        _dialogScript.DialogOverEvent -= DialogOver;
    }

    private void OnEnable()
    {
        _dialogScript.DialogOverEvent += DialogOver;
    }

    public void StartDialogs()
    {
        _dialogPlaying = true;

        _dialog.SetActive(true);
        _dialogScript.StartAllDialogs(_thisDialogBoxScriptableObject);
    }

    private void DialogOver()
    {
        Cursor.visible = true;

        _dialogPlaying = false;

        GameManager.LevelEndReached(_nextSceneName);

        StartCoroutine(FadeMusic());
    }

    IEnumerator FadeMusic()
    {
        while (_music.volume > 0)
        {
            _music.volume = Mathf.MoveTowards(_music.volume, 0, Time.deltaTime * _fadeMusicSpeed);
            yield return new WaitForEndOfFrame();
        }
    }
}
