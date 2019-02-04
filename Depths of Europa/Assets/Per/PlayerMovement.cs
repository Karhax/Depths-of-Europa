using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    [SerializeField] private float _moveAcceleration = 2.5f;
    [SerializeField] private float _moveSpeedMax = 3f;
    private float _moveForce = 0f;

    private Rigidbody2D _rigidbody;

	private void Start ()
    {
        _rigidbody = gameObject.GetComponent<Rigidbody2D>();
	}

    private void FixedUpdate()
    {
        // Add force to rigidbody
        _rigidbody.AddForce(new Vector2(_moveForce, 0));
        LimitSpeed();   
    }
	
	private void Update () {
        // Take input and calculate force to be added
        _moveForce = _moveAcceleration * Input.GetAxisRaw(Statics.GameInput.HORIZONTAL) * Time.deltaTime;
	}

    private void LimitSpeed()
    {
        float absoluteVelocity = Mathf.Abs(_rigidbody.velocity.x);
        if (absoluteVelocity > _moveSpeedMax)
        {
            _rigidbody.velocity = new Vector2(_moveSpeedMax * Mathf.Sign(_rigidbody.velocity.x), 0);
        }
    }
}
