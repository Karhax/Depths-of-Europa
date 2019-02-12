﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Statics;
using UnityEngine.UI;

public class Waypoint : MonoBehaviour
{
    [Header("Settings")]

    [SerializeField, Range(0, 500)] float _radius;
    [SerializeField, Range(0, 15)] float _fadeInSpeed;
    [SerializeField, Range(0, 15)] float _fadeOutSpeed;
    [SerializeField, Range(0, 15)] float _timeActive;
    [SerializeField, Range(0, 5)] float _waitTimeToStart;
    [SerializeField, Range(0, 20)] float _soundStrength;

    [Header("Drop")]

    [SerializeField] MoveShip _moveShipScript;
    [SerializeField] RectTransform _waypointRectTransform;
    [SerializeField] Transform _waypointTransform;

    Timer _waitToStartTimer;
    Timer _timerActive;
    Image _waypointImage;
    bool _wayPointOn = false;
    bool _fadingIn = true;
    bool _isPaused = false;

    PauseMenuScript _pauseMenuScript;

    private void Awake()
    {
        _waitToStartTimer = new Timer(_waitTimeToStart);
        _timerActive = new Timer(_timeActive);

        _waypointImage = _waypointRectTransform.GetComponent<Image>();
        _waypointRectTransform.gameObject.SetActive(false);
        _waypointImage.color = new Color(1, 1, 1, 0);

        if (_waypointTransform == null)
        {
            Debug.LogWarning("Waypoint is not pointing at something!", gameObject);
            _waypointTransform = transform;
        }
    }

    private void Start()
    {
        _pauseMenuScript = GameManager.CameraObject.GetComponentInChildren<PauseMenuScript>();
    }

    private void OnEnable()
    {
        if (_pauseMenuScript != null)
            _pauseMenuScript.PauseState += Paused;
    }

    private void OnDisable()
    {
        if (_pauseMenuScript != null)
            _pauseMenuScript.PauseState -= Paused;
    }

    private void Paused(bool paused)
    {
        _isPaused = paused;
    }

    private void Update()
    {
        if (!_isPaused)
        {
            _waitToStartTimer.Time += Time.deltaTime;

            bool action = Input.GetButtonDown(GameInput.SKIP_AND_SONAR);

            if (!_wayPointOn && action)
                StartPulse();
            else if (_wayPointOn && _waitToStartTimer.Expired())
            {
                _timerActive.Time += Time.deltaTime;
                SetWaypoint();

                if (_fadingIn)
                    FadeIn();
                else if (!_fadingIn && _timerActive.Expired())
                    FadeOut();
            }
        }
    }

    private void StartPulse()
    {
        _waitToStartTimer.Reset();
        _waypointRectTransform.gameObject.SetActive(true);
        _wayPointOn = true;
        _moveShipScript.TryMakeSound(_soundStrength, 1, true);
    }

    private void FadeIn()
    {
        if (Fade(_fadeInSpeed) >= 1)
        {
            _timerActive.Reset();
            _fadingIn = false;
        }
    }

    private float Fade(float speed)
    {
        float newAlphaValue = _waypointImage.color.a + speed * Time.deltaTime;
        _waypointImage.color = new Color(1, 1, 1, newAlphaValue);

        return newAlphaValue;
    }

    private void FadeOut()
    {
        if (Fade(-_fadeOutSpeed) <= 0)
            Stop();
    }

    private void Stop()
    {
        _fadingIn = true;
        _wayPointOn = false;
        _waypointRectTransform.gameObject.SetActive(false);
    }

    private void SetWaypoint()
    {
        Vector2 toWaypoint = (_waypointTransform.position - transform.position).normalized;

        _waypointRectTransform.localPosition = toWaypoint * _radius;
        _waypointRectTransform.right = toWaypoint;
    }
}
