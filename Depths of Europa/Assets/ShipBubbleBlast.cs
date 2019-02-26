using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Statics;

public class ShipBubbleBlast : MonoBehaviour
{
    [Header("Settings")]

    [SerializeField] bool _usePowerUp = true;

    [SerializeField, Range(0.1f, 100)] float _rechargeTime;
    [SerializeField, Range(0, 5)] float _scareFishDuration;

    [Header("Drop")]

    [SerializeField] GameObject _scareFishTrigger;
    [SerializeField] ParticleSystem _particleSystem;

    Timer _rechargeTimer;
    PauseMenuScript _pauseMenuScript;

    bool _paused = false;

    private void Awake()
    {
        if (!_usePowerUp)
            Destroy(this);

        _rechargeTimer = new Timer(_rechargeTime, _rechargeTime);
    }

    private void Start()
    {
        _pauseMenuScript = GameManager.CameraObject.GetComponentInChildren<PauseMenuScript>();

        if (_pauseMenuScript != null)
            _pauseMenuScript.PauseState += Paused;
    }

    private void Paused(bool state)
    {
        _paused = state;
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

    private void Update()
    {
        if (!_paused)
        {
            _rechargeTimer.Time += Time.deltaTime;

            bool action = Input.GetButtonDown(GameInput.BUBBLE_BLAST);

            if (_rechargeTimer.Expired() && action)
                DoBubbleBlast();
        }
    }

    private void DoBubbleBlast()
    {
        _rechargeTimer.Reset();

        _scareFishTrigger.SetActive(true);
        _particleSystem.Play();

        StartCoroutine(TurnOffTrigger());
    }

    IEnumerator TurnOffTrigger()
    {
        yield return new WaitForSeconds(_scareFishDuration);

        _scareFishTrigger.SetActive(false);
    }
}
