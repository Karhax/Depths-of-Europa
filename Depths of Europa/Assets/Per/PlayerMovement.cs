using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    
    [SerializeField] private float _moveSpeed = 150f;
    private Vector2 _currentSpeed = new Vector2(0, 0);

    [SerializeField] private float _ladderMoveSpeed = 120f;
    private bool _onLadder = false;

    private float _normalGravity = 0;

    private Rigidbody2D _rigidbody;

	private void Start ()
    {
        _rigidbody = gameObject.GetComponent<Rigidbody2D>();
        _normalGravity = _rigidbody.gravityScale;
	}
	
	private void Update () {
        // Handle input and movement for X Axis
        int directionMultiplier = (int)Input.GetAxisRaw(Statics.GameInput.HORIZONTAL);
        CalculateNewSpeedX(directionMultiplier);

        // if on ladder, handle input and movement for Y Axis
        if (_onLadder)
        {
            directionMultiplier = (int)Input.GetAxisRaw(Statics.GameInput.VERTICAL);
            CalculateNewSpeedY(directionMultiplier);
        }


        _rigidbody.velocity = _currentSpeed;
	}

    private void CalculateNewSpeedX(int directionMultiplier)
    {
        if (directionMultiplier == 0)
        {
            // No direction means character should stop moving.
            _currentSpeed = new Vector2(0, _rigidbody.velocity.y);
        }
        else
        {
            if (Mathf.Sign(_currentSpeed.x) != Mathf.Sign(directionMultiplier))
            {
                // A change in direction was detected. Set speed to a low speed in the new direction
                _currentSpeed = new Vector2(_moveSpeed / 10 * directionMultiplier * Time.deltaTime, _rigidbody.velocity.y);
            }
            else
            {
                // For continuous movement in a direction, have speed be the predefined speed
                _currentSpeed = new Vector2(_moveSpeed * directionMultiplier * Time.deltaTime, _rigidbody.velocity.y);
            }
        }
    }
    private void CalculateNewSpeedY(int directionMultiplier)
    {
        if (directionMultiplier == 0)
        {
            // No direction means character should stop moving.
            _currentSpeed = new Vector2(_currentSpeed.x, 0);
        }
        else
        {
            if (Mathf.Sign(_currentSpeed.y) != Mathf.Sign(directionMultiplier))
            {
                // A change in direction was detected. Set speed to a low speed in the new direction
                _currentSpeed = new Vector2(_currentSpeed.x, _ladderMoveSpeed / 10 * directionMultiplier * Time.deltaTime);
            }
            else
            {
                // For continuous movement in a direction, have speed be the predefined speed
                _currentSpeed = new Vector2(_currentSpeed.x, _ladderMoveSpeed * directionMultiplier * Time.deltaTime);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(Statics.Tags.LADDER))
        {
            _onLadder = true;
            _rigidbody.gravityScale = 0;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(Statics.Tags.LADDER))
        {
            _onLadder = false;
            _rigidbody.gravityScale = _normalGravity;
        }
    }
}
