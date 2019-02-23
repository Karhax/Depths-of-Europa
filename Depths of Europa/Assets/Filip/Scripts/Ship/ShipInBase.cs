using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipInBase : MonoBehaviour
{
    [SerializeField] MoveShip _moveShipScript;
    [SerializeField] Waypoint _waypointScript;
    [SerializeField] LightShip _lightShipScript;

    public void InBase(bool state)
    {
        _moveShipScript.enabled = !state;
        _waypointScript.enabled = !state;
        _lightShipScript.enabled = !state;

        Dialog dialog = GetComponentInChildren<Dialog>();

        if (dialog != null)
            Destroy(dialog.gameObject);
    }
}
