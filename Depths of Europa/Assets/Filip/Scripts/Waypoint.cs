using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum UICorners
{
    BOTTOM_LEFT,
    TOP_LEFT,
    TOP_RIGHT,
    BOTTOM_RIGHT
}

public class Waypoint : MonoBehaviour
{
    [Header("Settings")]

    [SerializeField] float _radius;
    [SerializeField] float _widthModifier;

    [Header("Drop")]

    [SerializeField] RectTransform _waypointImage;
    [SerializeField] Transform _waypointTransform;

    Camera _camera;
    Quaternion _arrowDownRotation;

    readonly int POINT_DOWN_ROTATION = -90;
    readonly float _offScreenLow = 0f;
    readonly float _offScreenHigh = 1f;

    private void Awake()
    {
        _arrowDownRotation = Quaternion.Euler(0, 0, POINT_DOWN_ROTATION);
        _camera = Camera.main;
    }

    private void Update()
    {
        SetWaypoint();
    }

    private void SetWaypoint()
    {
        Vector3 screenSpacePosition = _camera.WorldToViewportPoint(_waypointTransform.position);

        if (screenSpacePosition.x > _offScreenLow && screenSpacePosition.x < _offScreenHigh && screenSpacePosition.y > _offScreenLow && screenSpacePosition.y < _offScreenHigh)
            WaypointOnScreen();
        else
            WaypointOffScreen();
    }

    private void WaypointOnScreen()
    {
        _waypointImage.position = _waypointTransform.position;
        _waypointImage.rotation = _arrowDownRotation;
    }

    private void WaypointOffScreen()
    {
        Vector2 toWaypoint = (_waypointTransform.position - transform.position).normalized;

        _waypointImage.localPosition = toWaypoint * _radius;

        float angle = Mathf.Atan2(toWaypoint.y, toWaypoint.x) * Mathf.Rad2Deg;
        Quaternion newRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        _waypointImage.rotation = newRotation;
    }
}
