using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManagement : MonoBehaviour {

    [Header("The sources playing music and the volume that they should have when the level starts")]
    [SerializeField] private AudioSource[] _sources;
    [SerializeField] [Range(0, 1)] private float[] _sourceVolumes;
    private float[] _targetVolumes;

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
        if (_sources.Length != _sourceVolumes.Length)
        {
            throw new System.Exception("The MusicManager " + gameObject.ToString() + " does not have the same amount of AudioSources as it's SourceVolumes.");
        }
        _targetVolumes = _sourceVolumes;
        _changePerFrame = _sourceVolumes;

        // Set the current volume to 0 and calculate the change per frame for the initiating fade
        for (int i = 0; i < _sources.Length; i++)
        {
            _sources[i].volume = 0;
            if (_targetVolumes[i] != _sources[i].volume)
            {
                _changePerFrame[i] = (_sources[i].volume - _targetVolumes[i]) / _levelStartFade;
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
                // Set the volume to the exakt target
                for (int i = 0; i < _sources.Length; i++)
                {
                    _sources[i].volume = _targetVolumes[i];
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

    public void AdjustVolumes(float[] volumes)
    {
        if (!_ignoreNewTasks)
        {
            for (int i = 0; i < _targetVolumes.Length; i++)
            {
                if (i < volumes.Length)
                {
                    // Specified volumes are set as target volumes
                    _targetVolumes[i] = volumes[i];
                    if (_targetVolumes[i] != _sources[i].volume)
                    {
                        _changePerFrame[i] = (_sources[i].volume - _targetVolumes[i]) / _levelStartFade;
                    }
                    else
                    {
                        _changePerFrame[i] = 0;
                    }
                }
                else
                {
                    // Unspecified volumes are set to 0
                    _targetVolumes[i] = 0;
                }
            }
            _fadeTimer.Duration = _midLevelTransitionFade;
            _fadeTimer.Reset();
        }
    }

    public void ResetVolumes()
    {
        if (!_ignoreNewTasks)
        {
            AdjustVolumes(_sourceVolumes);
        }
    }

    public void EndMusic()
    {
        float[] zeroVolume = new float[1];
        zeroVolume[0] = 0;
        AdjustVolumes(zeroVolume);
        _fadeTimer.Duration = _levelEndFade;
        _ignoreNewTasks = true;
    }
}