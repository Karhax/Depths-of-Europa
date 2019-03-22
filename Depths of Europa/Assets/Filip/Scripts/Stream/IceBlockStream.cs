using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBlockStream : MonoBehaviour
{
    [SerializeField, Range(0, 200)] float _strength = 12.5f;
    [SerializeField] Vector2 _direction = Vector2.down;

    Rigidbody2D _thisRigidbody;

    private void Awake()
    {
        _thisRigidbody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate ()
    {
        _thisRigidbody.AddForce(_direction * Time.deltaTime * _strength);
    }
}
