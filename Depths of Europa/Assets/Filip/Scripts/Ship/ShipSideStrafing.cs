using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Statics;

public class ShipSideStrafing : MonoBehaviour
{
    [Header("Settings")]

    [SerializeField] bool _usePowerUp = true;
    [SerializeField, Range(0, 5)] float _strafeForce = 0.5f;

    [Header("Drop")]

    [SerializeField] ParticleSystem _leftParticleSystem;
    [SerializeField] ParticleSystem _rightParticleSystem;

    Rigidbody2D _thisRigidbody;
    bool _leftStopped, _rightStopped = false;

    private void Awake()
    {
        if (!_usePowerUp)
            Destroy(this);

        _thisRigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        bool left = Input.GetButton(GameInput.SIDE_STRAFE_LEFT);
        bool right = Input.GetButton(GameInput.SIDE_STRAFE_RIGHT);

        if (left)
            AddForce(Vector2.left, _rightParticleSystem, ref _rightStopped);
        else
            TurnOff(_rightParticleSystem, ref _rightStopped);
        if (right)
            AddForce(Vector2.right, _leftParticleSystem, ref _leftStopped);
        else
            TurnOff(_leftParticleSystem, ref _leftStopped);
    }

    private void AddForce(Vector2 direction, ParticleSystem particleSystem, ref bool stopped)
    {
        _thisRigidbody.AddRelativeForce(direction * _strafeForce);

        if (!particleSystem.isPlaying || stopped)
        {
            stopped = false;
            particleSystem.Play();
        }  
    }

    private void TurnOff(ParticleSystem particleSystem, ref bool stopped)
    {
        if (particleSystem.isPlaying)
        {
            stopped = true;
            particleSystem.Stop();
        }   
    }
}
