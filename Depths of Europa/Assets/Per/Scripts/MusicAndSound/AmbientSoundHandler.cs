using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientSoundHandler : MonoBehaviour
{
    [Header("One list for the sounds, and one list for how probable they are")]
    [SerializeField] private AudioClip[] _audioArray;// A list of all ambient audio clips
    [SerializeField] [Range(1, 100)] private int[] _audioRandomWeight;// Used to determine how likely it is that the corresponding clip is played.

    private int _totalWeight = 0;

    private Timer _audioCooldownTimer;

    [SerializeField] [Range(1, 59)] private float _minCooldownTime;
    [SerializeField] [Range(2, 60)] private float _maxCooldownTime;

    [SerializeField] [Range(3f, 99f)] private float _minDistance = 3;
    [SerializeField] [Range(4f, 100f)] private float _maxDistance = 4;

    [SerializeField] [Range(0.5f, 1.9f)] private float _minPitch = 0.8f;
    [SerializeField] [Range(0.6f, 2f)] private float _maxPitch = 1.5f;

    [SerializeField] private bool _playing = true;
    private AudioSource _audioSource;

    private bool _noFatalErrors = true;

    void Start()
    {
        if (_audioArray.Length == 0)
        {
            Debug.LogWarning("The Ambient Sound Handler does not have any audio clips to play.");
            _noFatalErrors = false;
        }

        for (int i = 0; i < _audioRandomWeight.Length; i++)
        {
            _totalWeight += _audioRandomWeight[i];
        }
        if (_totalWeight == 0)
        {
            Debug.LogWarning("The Ambient Sound Handler has detected faulty weighting of Audio Randomisation. No Ambient sounds have weight, and therefore will not be played");
            _noFatalErrors = false;
        }

        if (_minCooldownTime >= _maxCooldownTime)
        {
            _maxCooldownTime = _minCooldownTime + 1;
        }
        _audioCooldownTimer = new Timer(Random.Range(_minCooldownTime, _maxCooldownTime));

        if (_minDistance >= _maxDistance)
        {
            _maxDistance = _minDistance + 1;
        }

        if (_audioArray.Length != _audioRandomWeight.Length)
        {
            Debug.LogWarning("The Ambient Sound Handler has detected that the amount of audio clips differ from the amount of weight values. Randomisation will not be accurate.");
        }

        _audioSource = gameObject.GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.LogWarning("The Ambient Sound Handler does not have any Audio Source to play sounds from.");
            _noFatalErrors = false;
        }
    }

    void Update()
    {
        if (_noFatalErrors && _playing)
        {
            if (!_audioSource.isPlaying)
            {
                if (!_audioCooldownTimer.Expired())
                {
                    _audioCooldownTimer.Time += Time.deltaTime;
                }
                else
                {
                    RandomiseLocationOffset();
                    RandomiseSound();
                    RandomisePitch();
                    _audioSource.Play();
                    RandomiseCooldown();
                    _audioCooldownTimer.Reset();
                }
            }
        }
    }

    private void RandomiseSound()
    {
        int randomValue = Random.Range(1, _totalWeight);
        int index = -1;

        // Calculate the randomised index by decreasing a random value by the weight of the current index until 0 is reached.
        while (randomValue > 0)
        {
            index++;
            if (index >= _audioRandomWeight.Length)
            {
                index = 0;
            }
            randomValue -= _audioRandomWeight[index];
        }

        if (index >= _audioArray.Length)
        {
            // Make sure index is not too large
            index = index % _audioArray.Length;
        }

        _audioSource.clip = _audioArray[index];
    }

    private void RandomiseCooldown()
    {
        float randomValue = Random.Range(_minCooldownTime, _maxCooldownTime);
        _audioCooldownTimer.Duration = randomValue;
    }

    private void RandomiseLocationOffset()
    {
        // Note that this will rotate the parent. Make sure that the parent can be rotated without issues!
        float randomValue = Random.Range(_minDistance, _maxDistance);

        transform.localPosition = new Vector3(randomValue, transform.localPosition.y, transform.localPosition.z);

        randomValue = Random.Range(0, 360);

        transform.parent.localRotation = Quaternion.AngleAxis(randomValue, Vector3.forward);
    }

    private void RandomisePitch()
    {
        _audioSource.pitch = Random.Range(_minPitch, _maxPitch);
    }

    public void ToggleAmbientSound()
    {
        _playing = !_playing;
    }
}