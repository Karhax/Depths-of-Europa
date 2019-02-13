using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientSoundHandler : MonoBehaviour
{

    [SerializeField] private AudioClip[] _audioArray;// A list of all ambient audio clips
    [SerializeField] [Range(1, 100)] private int[] _audioRandomWeight;// Used to determine how likely it is that the corresponding clip is played.

    private int _totalWeight = 0;

    private Timer _audioCooldownTimer;

    [SerializeField] [Range(1, 59)] private float _minCooldownTime;
    [SerializeField] [Range(2, 60)] private float _maxCooldownTime;

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
        if (_noFatalErrors)
        {
            if (!_audioSource.isPlaying)
            {
                if (!_audioCooldownTimer.Expired())
                {
                    _audioCooldownTimer.Time += Time.deltaTime;
                }
                else
                {
                    RandomiseSound();
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
}