using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveShip : MonoBehaviour
{
    [SerializeField, Range(0, 250)] float _turnSpeed = 60f;
    [SerializeField, Range(0, 3)] float _baseForwardSpeed = 0.55f;
    [SerializeField, Range(0, 3)] float _baseBackWardSpeed = 0.25f;
    [SerializeField, Range(0, 3)] float _accelerationModifier = 0.55f;

    Rigidbody2D thisRigidbody;

    private void Awake()
    {
        thisRigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float move = Input.GetAxis("Vertical");
        float turn = Input.GetAxis("Horizontal");

        float dotProduct = Vector3.Dot(thisRigidbody.velocity, transform.up);

        if (turn != 0)
            thisRigidbody.AddTorque(turn * -dotProduct * Time.deltaTime * _turnSpeed);

        if (move != 0)
        {
            if (move > 0)
                SetNewSpeed(move, _baseForwardSpeed, dotProduct);
            else
                SetNewSpeed(move, _baseBackWardSpeed, dotProduct);

        }
    }

    private void SetNewSpeed(float move, float speedType, float dotProduct)
    {
        thisRigidbody.velocity += (Vector2)transform.up * move * Time.deltaTime * (speedType + Mathf.Abs(dotProduct) * _accelerationModifier);
    }

}
