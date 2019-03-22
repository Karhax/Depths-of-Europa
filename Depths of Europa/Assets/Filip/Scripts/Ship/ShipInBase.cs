using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipInBase : MonoBehaviour
{
    [Header("Settings")]

    [SerializeField, Range(0, 50)] float _inBaseMoveSpeed;
    [SerializeField, Range(0, 50)] float _inBaseRotateSpeed;

    [Header("Drop")]

    [SerializeField] MoveShip _moveShipScript;
    [SerializeField] Waypoint _waypointScript;
    [SerializeField] LightShip _lightShipScript;
    [SerializeField] ShipEffects _effectScript;

    public void InBase(bool state, Vector3 worldPosition, float worldZRotation = 0, bool moveShip = false)
    {
        _moveShipScript.enabled = !state;
        _waypointScript.enabled = !state;

        if (_lightShipScript.enabled)
            _lightShipScript.TurnHeadLightsOff();

        if (_effectScript.enabled)
            _effectScript.TurnOffEngine();
            

        _lightShipScript.enabled = !state;
        _effectScript.enabled = !state;

        ShipSideStrafing strafeScript = GetComponent<ShipSideStrafing>();

        if (strafeScript != null)
            strafeScript.TurnOff();

        Dialog dialog = GetComponentInChildren<Dialog>();

        if (dialog != null)
            Destroy(dialog.gameObject);

        if (moveShip)
            StartCoroutine(MoveShip(worldPosition, worldZRotation));
    }

    IEnumerator MoveShip(Vector3 worldPosition, float worldZRotation)
    {
        if (worldZRotation > 180)
            worldZRotation -= 360;

        GetComponent<Rigidbody2D>().simulated = false;

        while (transform.position != worldPosition || transform.rotation.eulerAngles.z != worldZRotation)
        {
            transform.position = Vector3.MoveTowards(transform.position, worldPosition, Time.deltaTime * _inBaseMoveSpeed);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, worldZRotation), Time.deltaTime * _inBaseRotateSpeed);
            yield return new WaitForEndOfFrame();
        }
    }
}
