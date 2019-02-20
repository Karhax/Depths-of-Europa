using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Statics;

public class LightShip : MonoBehaviour
{
    public delegate void ShipUsedFlare(int flare);
    public event ShipUsedFlare ShipUsedFlareEvent;

    [Header("Settings")]

    [SerializeField, Range(0, 15)] int _maxAmountFlares;
    [SerializeField, Range(0, 15)] float _flareOutSpeed;
    [SerializeField, Range(0, 10)] float _maxFlareSpeed;
    [SerializeField, Range(0, 10)] float _minTimeBetweenFlares;

    [Header("Drop")]

    [SerializeField] GameObject _trigger;
    [SerializeField] GameObject _flarePrefab;
    [SerializeField] GameObject _lightHolder;
    [SerializeField] GameObject _headlight;

    int _currentFlareCount;
    int _floorMask;
    float _camRayLength = 100f;
    bool _isPaused = false;

    Rigidbody2D thisRigidbody;

    Timer _waitBetweenFlare;

    PauseMenuScript _pauseMenuScript;
    private void Awake()
    {
        _waitBetweenFlare = new Timer(_minTimeBetweenFlares, _minTimeBetweenFlares);

        thisRigidbody = GetComponent<Rigidbody2D>();
        _currentFlareCount = _maxAmountFlares;

        _floorMask = LayerMask.GetMask(Layers.CLICKABLE);
    }

    private void Start()
    {
        _pauseMenuScript = GameManager.CameraObject.GetComponentInChildren<PauseMenuScript>();
        _pauseMenuScript.PauseState += Paused;

        if (ShipUsedFlareEvent != null)
            ShipUsedFlareEvent.Invoke(_currentFlareCount);
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
            _waitBetweenFlare.Time += Time.deltaTime;

            bool flare = Input.GetButton(GameInput.ACTION2);
            bool onOff = Input.GetButtonDown(GameInput.ACTION3);
            bool headLight = Input.GetButtonDown(GameInput.ACTION);

            if (onOff)
                TurnLightsOnOff();

            if (headLight && _lightHolder.activeSelf)
                TurnHeadLightsOnOff();

            if (flare && _currentFlareCount > 0 && _waitBetweenFlare.Expired())
                UseFlare();
        }
    }

    private void TurnLightsOnOff()
    {
        _lightHolder.SetActive(!_lightHolder.activeSelf);

        if (_lightHolder.activeSelf)
            _trigger.SetActive(_headlight.activeSelf);
        else
            _trigger.SetActive(false);
    }

    private void TurnHeadLightsOnOff()
    {
        _trigger.SetActive(!_trigger.activeSelf);
        _headlight.SetActive(!_headlight.activeSelf);
    }
    private void UseFlare()
    {
        _waitBetweenFlare.Reset();

        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit floorHit;
        Vector2 direction = new Vector2();

        if (Physics.Raycast(camRay, out floorHit, _camRayLength, _floorMask))
            direction = floorHit.point - transform.position;
        else
            throw new System.Exception("Clickable not set up right!");

        if (direction.magnitude > _maxFlareSpeed)
            direction = direction.normalized * _maxFlareSpeed;

        GameObject newFlare = Instantiate(_flarePrefab, transform.position, Quaternion.identity) as GameObject;
        newFlare.GetComponent<Rigidbody2D>().velocity = thisRigidbody.velocity + direction * _flareOutSpeed;

        _currentFlareCount -= 1;

        if (ShipUsedFlareEvent != null)
            ShipUsedFlareEvent.Invoke(_currentFlareCount);
    }
}
