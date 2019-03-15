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

    [SerializeField] AudioSource _leftBubbleAudio;
    [SerializeField] AudioSource _rightBubbleAudio;

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
        {
            AddForce(Vector2.left, _rightParticleSystem, ref _rightStopped);
            TurnOnAudio(_rightBubbleAudio);
        }   
        else
        {
            TurnOff(_rightParticleSystem, ref _rightStopped, true);
            TurnOffAudio(_rightBubbleAudio);
        }
            
        if (right)
        {
            AddForce(Vector2.right, _leftParticleSystem, ref _leftStopped);
            TurnOnAudio(_leftBubbleAudio);
        }
            
        else
        {
            TurnOff(_leftParticleSystem, ref _leftStopped);
            TurnOffAudio(_leftBubbleAudio);
        }
    }     

    private void TurnOnAudio(AudioSource source)
    {
        if (!source.isPlaying)
            source.Play();
    }

    private void TurnOffAudio(AudioSource source)
    {
        if (source.isPlaying)
            source.Pause();
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

    private void TurnOff(ParticleSystem particleSystem, ref bool stopped, bool isRight = false)
    {
        if (particleSystem.isPlaying)
        {
            stopped = true;
            particleSystem.Stop();
        }   
    }
}
