using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Statics;

public class MoveShip : MonoBehaviour
{
    [Header("Turn speed behaves differently by collider size! Big range to make work"), Space]
    [SerializeField, Range(0, 250)] float _turnSpeed = 60f;
    [Space]

    [Header("Settings work with Linear Drag and Angular Drag, change in sync"), Space]

    [SerializeField, Range(0, 4)] float _maxSpeedMagnitude = 2.25f;

    [SerializeField, Range(0, 3)] float _baseForwardSpeed = 0.55f;
    [SerializeField, Range(0, 3)] float _slowdownForwardSpeed = 0.25f;
    [SerializeField, Range(0, 3)] float _baseBackWardSpeed = 0.25f;
    [SerializeField, Range(0, 3)] float _slowdownBackwardSpeed = 0.25f;
    [SerializeField, Range(0, 3)] float _accelerationModifier = 0.55f;

    Rigidbody2D thisRigidbody;

    private void Awake()
    {
        thisRigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float move = Input.GetAxis(GameInput.VERTICAL);
        float turn = Input.GetAxis(GameInput.HORIZONTAL);
        float dotProduct = Vector3.Dot(thisRigidbody.velocity, transform.up);

        if (turn != 0)
            thisRigidbody.AddTorque(turn * -dotProduct * Time.deltaTime * _turnSpeed);

        if (move != 0 && thisRigidbody.velocity.magnitude < _maxSpeedMagnitude)
        {
            if (move > 0)
                SetNewSpeed(move, _baseForwardSpeed, _slowdownForwardSpeed, dotProduct);
            else
                SetNewSpeed(move, _baseBackWardSpeed, _slowdownBackwardSpeed, dotProduct);
        }
    }

    private void SetNewSpeed(float move, float forwardSpeedType, float backwardSpeedType, float dotProduct)
    {
        Vector2 newVelocity = (Vector2)transform.up * move * Time.deltaTime;

        if (move > 0)
            newVelocity *= forwardSpeedType + Mathf.Abs(dotProduct) * _accelerationModifier;
        else
            newVelocity *= backwardSpeedType;

        thisRigidbody.velocity += newVelocity;
    }
}