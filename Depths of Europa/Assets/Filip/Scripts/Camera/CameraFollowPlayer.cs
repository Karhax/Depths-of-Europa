using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    [SerializeField, Range(0, 5)] float _cameraFollowSpeed = 1;
    [SerializeField, Range(1, 5)] float _cameraGoBackAmount = 2;
    [SerializeField] float _forwardCameraZoomOutAmount = 2;
    [SerializeField] float _backwardCameraZoomOutAmount = 1.2f;
    [SerializeField] Transform _player;

    Camera _thisCamera;
    Rigidbody2D _playerRigidBody;

    float _originalCameraSize;

    private void Awake()
    {
        _thisCamera = GetComponent<Camera>();
        _playerRigidBody = _player.GetComponent<Rigidbody2D>();

        _originalCameraSize = _thisCamera.orthographicSize;
    }

    private void LateUpdate()
    {
        FollowPlayer();
        ZoomOutDueToSpeed();
    }

    private void FollowPlayer()
    {
        Vector3 newPosition = _player.transform.position + (Vector3)_playerRigidBody.velocity * _cameraGoBackAmount;
        newPosition.z = transform.position.z;
        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * _cameraFollowSpeed);
    }

    private void ZoomOutDueToSpeed()
    {
        float dotProduct = Vector3.Dot(_playerRigidBody.velocity, _player.up);
        float newSize = _originalCameraSize;

        if (dotProduct >= 0)
            newSize += dotProduct * _forwardCameraZoomOutAmount;
        else
            newSize += -dotProduct * _backwardCameraZoomOutAmount;

        _thisCamera.orthographicSize = newSize;
    }
}
