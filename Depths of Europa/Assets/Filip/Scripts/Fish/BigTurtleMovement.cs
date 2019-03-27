using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Statics;

public class BigTurtleMovement : MonoBehaviour
{
    [Header("Settings")]

    [SerializeField] bool _circlePath = true;

    [SerializeField, Range(0, 10)] float _reachedTolerance = 0.5f;
    [SerializeField, Range(0, 1000)] float _speed = 1;
    [SerializeField, Range(0, 15)] float _rotateSpeed = 0.25f;
    [SerializeField, Range(0, 100)] int _damage = 15;
    [SerializeField, Range(0, 5)] float _waitBeforeDamageAgain = 1f;

    [Header("Drop")]

    [SerializeField] Transform[] _path;

    Rigidbody2D _thisRigidbody;
    int _currentPathIndex = 0;
    Vector2 _direction;
    Timer _waitTimer;

    private void Awake()
    {
        _waitTimer = new Timer(_waitBeforeDamageAgain, _waitBeforeDamageAgain);
        _thisRigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        _waitTimer.Time += Time.deltaTime;

        SetDirection();
        Move();
        Rotate();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        DamageShip shipScript = collision.transform.GetComponent<DamageShip>();

        if (shipScript != null && _waitTimer.Expired())
        {
            _waitTimer.Reset();
            shipScript.Hit(_damage);
        }   
    }

    private void Move()
    {
        _thisRigidbody.AddForce(_direction * _speed);

        if (_thisRigidbody.position.x <= _path[_currentPathIndex].position.x + _reachedTolerance && _thisRigidbody.position.x >= _path[_currentPathIndex].position.x - _reachedTolerance &&
            _thisRigidbody.position.y <= _path[_currentPathIndex].position.y + _reachedTolerance && _thisRigidbody.position.y >= _path[_currentPathIndex].position.y - _reachedTolerance)
        {
            NextPathPosition();
        }
    }

    private void NextPathPosition()
    {
        if (++_currentPathIndex == _path.Length)
        {
            if (_circlePath)
                _currentPathIndex = 0;
            else
                Destroy(transform.parent.gameObject);
        }       
    }

    private void SetDirection()
    {
        _direction = (_path[_currentPathIndex].position - transform.position).normalized;
    }

    private void Rotate()
    {
        Vector3 newRightVector = Vector2.Lerp(transform.right, _direction, Time.deltaTime * _rotateSpeed);
        transform.right = newRightVector;

        if (transform.rotation.eulerAngles.y != 0)
            transform.rotation = Quaternion.Euler(0, 0, 180);
    }
}
