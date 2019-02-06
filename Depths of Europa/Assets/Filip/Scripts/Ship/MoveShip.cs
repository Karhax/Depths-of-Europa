using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Statics;

public class MoveShip : MonoBehaviour
{
    [Header("Turn speed behaves differently by collider size! Big range to make work"), Space]
    [SerializeField, Range(0, 250)] float _slowTurnSpeed = 60f;
    [SerializeField, Range(0, 250)] float _mediumTurnSpeed = 60f;
    [SerializeField, Range(0, 250)] float _fastTurnSpeed = 25f;
    [Space]

    [Header("Settings work with Linear Drag and Angular Drag, change in sync"), Space]

    [SerializeField, Range(0, 4)] float _slowSpeedMagnitude = 0.75f;
    [SerializeField, Range(0, 4)] float _mediumSpeedMagnitude = 1.5f;
    [SerializeField, Range(0, 4)] float _maxSpeedMagnitude = 2.25f;

    [SerializeField, Range(0, 3)] float _baseForwardSpeed = 0.55f;
    [SerializeField, Range(0, 3)] float _slowdownForwardSpeed = 0.25f;
    [SerializeField, Range(0, 3)] float _baseBackWardSpeed = 0.25f;
    [SerializeField, Range(0, 3)] float _slowdownBackwardSpeed = 0.25f;
    [SerializeField, Range(0, 3)] float _accelerationModifier = 0.55f;

    [SerializeField] float _highSpeedTriggerModifier;
    [SerializeField] float _lowSpeedTriggerModifier;

    [Header("Drop")]

    [SerializeField] CircleCollider2D _highSpeedTrigger;
    [SerializeField] CircleCollider2D _lowSpeedTrigger;

    Rigidbody2D _thisRigidbody;
    float _highSpeedTriggerNormalRadius;
    float _lowSpeedTriggerNormalRadius;

    private void Awake()
    {
        _highSpeedTriggerNormalRadius = _highSpeedTrigger.radius;
        _lowSpeedTriggerNormalRadius = _lowSpeedTrigger.radius;

        _thisRigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        float move = Input.GetAxis(GameInput.VERTICAL);
        float turn = Input.GetAxis(GameInput.HORIZONTAL);
        float dotProduct = Vector3.Dot(_thisRigidbody.velocity, transform.up);

        if (turn != 0)
            Turn(turn, dotProduct);

        if (move != 0 && _thisRigidbody.velocity.magnitude < _maxSpeedMagnitude)
        {
            if (move > 0)
                SetNewSpeed(move, _baseForwardSpeed, _slowdownForwardSpeed, dotProduct);
            else
                SetNewSpeed(move, _baseBackWardSpeed, _slowdownBackwardSpeed, dotProduct);
        }

        ChangeSpeedTriggers();
    }

    private void Turn(float turn, float dotProduct)
    {
        float speedMagnitude = _thisRigidbody.velocity.magnitude;
        float speedModifier = 1;

        if (speedMagnitude <= _slowSpeedMagnitude)
            speedModifier = _slowTurnSpeed;
        else if (speedMagnitude <= _mediumSpeedMagnitude)
            speedModifier = _mediumTurnSpeed;
        else
            speedModifier = _fastTurnSpeed;

        _thisRigidbody.AddTorque(turn * -dotProduct * Time.deltaTime * speedModifier);
    }

    private void SetNewSpeed(float move, float forwardSpeedType, float backwardSpeedType, float dotProduct)
    {
        Vector2 newVelocity = (Vector2)transform.up * move * Time.deltaTime;

        if (move > 0)
            newVelocity *= forwardSpeedType + Mathf.Abs(dotProduct) * _accelerationModifier;
        else
            newVelocity *= backwardSpeedType;

        _thisRigidbody.velocity += newVelocity;
    }

    private void ChangeSpeedTriggers()
    {
        float velocityMagnitude = _thisRigidbody.velocity.magnitude;

        _highSpeedTrigger.radius = _highSpeedTriggerNormalRadius + velocityMagnitude * _highSpeedTriggerModifier;
        _lowSpeedTrigger.radius = _lowSpeedTriggerNormalRadius - velocityMagnitude * _lowSpeedTriggerModifier;

    }
}
