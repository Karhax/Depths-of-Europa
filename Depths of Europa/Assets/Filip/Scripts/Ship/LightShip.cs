using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Statics;

public class LightShip : MonoBehaviour
{
    [Header("Settings")]

    [SerializeField] int _maxAmountFlares;
    [SerializeField] float _flareOutSpeed;

    [Header("Drop")]

    [SerializeField] GameObject _flarePrefab;
    [SerializeField] GameObject _lightHolder;
    [SerializeField] GameObject _headlight;

    int _currentFlareCount;
    bool _canUseFlare = true;
    int _floorMask;
    float _camRayLength = 100f;

    Rigidbody2D thisRigidbody;

    Timer _waitBetweenFlare = new Timer(0.2f);

    private void Awake()
    {
        thisRigidbody = GetComponent<Rigidbody2D>();
        _currentFlareCount = _maxAmountFlares;

        _floorMask = LayerMask.GetMask(Layers.CLICKABLE);
    }

    private void Update()
    {
        _waitBetweenFlare.Time += Time.deltaTime;

        bool flare = Input.GetButton(GameInput.ACTION2);
        bool onOff = Input.GetButtonDown(GameInput.ACTION3);
        bool headLight = Input.GetButtonDown(GameInput.ACTION);

        if (onOff)
            TurnLightsOnOff();

        if (headLight && _lightHolder.activeSelf)
            TurnHeadLightsOnOff();

        if (flare && _currentFlareCount > 0 && _canUseFlare && _waitBetweenFlare.Expired())
            UseFlare();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag(Tags.FLARE) && _canUseFlare)
            _canUseFlare = false;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(Tags.FLARE))
            _canUseFlare = true;
    }

    private void TurnLightsOnOff()
    {
        _lightHolder.SetActive(!_lightHolder.activeSelf);
    }

    private void TurnHeadLightsOnOff()
    {
        _headlight.SetActive(!_headlight.activeSelf);
    }
    private void UseFlare()
    {
        _waitBetweenFlare.Reset();

        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit floorHit;
        Vector2 direction = new Vector2();

        if (Physics.Raycast(camRay, out floorHit, _camRayLength, _floorMask))
            direction = (floorHit.point - transform.position).normalized;
        else
            throw new System.Exception("Clickable not set up right!");

        GameObject newFlare = Instantiate(_flarePrefab, transform.position, Quaternion.identity) as GameObject;
        newFlare.GetComponent<Rigidbody2D>().velocity = thisRigidbody.velocity + direction * _flareOutSpeed;

        _currentFlareCount -= 1;
    }
}
