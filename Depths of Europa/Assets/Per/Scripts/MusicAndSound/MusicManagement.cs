using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManagement : MonoBehaviour {

    [Header("The sources playing music and the volume that they should have when the level starts")]
    [SerializeField] private AudioSource[] _sources;
    [SerializeField] private bool[] _standardActiveStems;
    private bool[] _currentActiveStems;

    [Header("The time in seconds it takes for the music to fade in/out")]
    [SerializeField] [Range(0.1f, 5f)] private float _levelStartFade;
    [SerializeField] [Range(0.1f, 5f)] private float _levelEndFade;
    [SerializeField] [Range(0.1f, 5f)] private float _midLevelTransitionFade;

    private float[] _changePerFrame;

    private Timer _fadeTimer;

    private bool _ignoreNewTasks = false;

    private void Awake()
    {
        // Make sure all array have the same length
        if (_sources.Length != _standardActiveStems.Length)
        {
            throw new System.Exception("The MusicManager " + gameObject.ToString() + " have too few/many active stems compared to the number of connected audio sources.");
        }
        _currentActiveStems = new bool[_sources.Length];
        _changePerFrame = new float[_sources.Length];

        // Set the current volume to 0 and calculate the change per frame for the initiating fade
        for (int i = 0; i < _sources.Length; i++)
        {
            _sources[i].volume = 0;
            _currentActiveStems[i] = _standardActiveStems[i];

            if (_currentActiveStems[i])
            {
                _changePerFrame[i] = 1 / _levelStartFade;
            }
            else
            {
                _changePerFrame[i] = 0;
            }
        }

        // Start the timer
        _fadeTimer = new Timer(_levelStartFade);
    }

    private void Update()
    {
        if (!_fadeTimer.Expired())
        {
            _fadeTimer.Time += Time.deltaTime;
            if (_fadeTimer.Expired())
            {
                // Set the volume to the exact target
                for (int i = 0; i < _sources.Length; i++)
                {
                    if (_currentActiveStems[i])
                    {
                        _sources[i].volume = 1;
                    }
                    else
                    {
                        _sources[i].volume = 0;
                    }
                }
            }
            else
            {
                // Step the volume towards the target
                for (int i = 0; i < _sources.Length; i++)
                {
                    _sources[i].volume += _changePerFrame[i] * Time.deltaTime;
                }
            }
        }
    }

    public void AdjustVolumes(bool[] activeStems)
    {
        if (!_ignoreNewTasks)
        {
            for (int i = 0; i < _currentActiveStems.Length; i++)
            {
                if (i < activeStems.Length)
                {
                    // This stem has a designated bool value. If it is active, change volume towards 1. Else, change towards 0;
                    _currentActiveStems[i] = activeStems[i];
                    if (_currentActiveStems[i])
                    {
                        _changePerFrame[i] = (1 - _sources[i].volume) / _midLevelTransitionFade;
                    }
                    else
                    {
                        _changePerFrame[i] = (0 - _sources[i].volume) / _midLevelTransitionFade;
                    }
                }
                // Stems with no designated bool value will not be altered in any way
            }
            _fadeTimer.Duration = _midLevelTransitionFade;
            _fadeTimer.Reset();
        }
    }

    public void ResetVolumes()
    {
        if (!_ignoreNewTasks)
        {
            AdjustVolumes(_standardActiveStems);
        }
    }

    public void EndMusic()
    {
        // This function should be called when this music player is supposed to stop playing.
        // Have all stems transition towards 0 volume.
        bool[] zeroVolume = new bool[_standardActiveStems.Length];
        for (int i = 0; i < zeroVolume.Length; i++)
        {
            zeroVolume[i] = false;
        }
        AdjustVolumes(zeroVolume);
        _fadeTimer.Duration = _levelEndFade;

        // All further calls to this manager will be ignored.
        _ignoreNewTasks = true;
    }
}