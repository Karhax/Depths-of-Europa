using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Statics;

public class Tutorial : MonoBehaviour
{
    public delegate void TutorialOver();
    public event TutorialOver TutorialOverEvent;

    [Header("Settings")]

    [SerializeField, Range(0.1f, 10)] float _minShowTime = 1f;

    [Header("Drop")]

    [SerializeField] Image _tutorialImage;
    [SerializeField] GameObject _background;
    [SerializeField] GameObject _skipObject;

    [SerializeField] AudioSource _stopAudio;
    [SerializeField] AudioSource _startAudio;

    bool _isInTutorial = false;

    private void Update()
    {
        if (_isInTutorial)
        {
            bool skip = Input.GetButtonDown(GameInput.SKIP_DIALOG) || Input.GetKeyDown("Space") || Input.GetKeyDown("Mouse0");

            if (skip)
                StopTutorial();
        }
    }

    public void StartTutorial(Sprite sprite)
    {
        StartCoroutine(Wait());

        _startAudio.Play();
        _tutorialImage.sprite = sprite;
        _tutorialImage.gameObject.SetActive(true);
        _background.SetActive(true);
        _skipObject.SetActive(true);
    }

    IEnumerator Wait()
    {
        yield return new WaitForSecondsRealtime(_minShowTime);
        _isInTutorial = true;
    }

    public void StopTutorial()
    {
        _stopAudio.Play();
        _isInTutorial = false;

        _tutorialImage.gameObject.SetActive(false);
        _background.gameObject.SetActive(false);
        _skipObject.SetActive(false);

        if (TutorialOverEvent != null)
            TutorialOverEvent.Invoke();
    }
}
