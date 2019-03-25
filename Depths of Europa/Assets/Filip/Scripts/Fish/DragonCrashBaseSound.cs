using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Statics;

public class DragonCrashBaseSound : MonoBehaviour
{
    [SerializeField] bool _doCollision = true;
    [SerializeField] bool _doInterval = true;
    [SerializeField, Range(0, 20)] float _firstScreamTime = 3.5f;
    [SerializeField, Range(0, 20)] float _interval = 5f;
    [SerializeField] AudioSource _monsterHitBaseAudio;
    [SerializeField] AudioSource _screamAudio;

    Timer _intervalTimer;
    bool _firstScream = false;

    private void Awake()
    {
        _intervalTimer = new Timer(_interval);
    }

    private void Start()
    {
        StartCoroutine(Scream());
    }

    private void Update()
    {
        if (_firstScream)
        {
            _intervalTimer.Time += Time.deltaTime;

            if (_intervalTimer.Expired() && _doInterval)
            {
                _intervalTimer.Reset();
                _screamAudio.Play();
            }
        } 
    }


    IEnumerator Scream()
    {
        yield return new WaitForSeconds(_firstScreamTime);
        _screamAudio.Play();

        if (!_doInterval && !_doCollision)
            Destroy(this);
        else
            _firstScream = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(Layers.FISH_UNDER) && !_monsterHitBaseAudio.isPlaying && _doCollision)
            _monsterHitBaseAudio.Play();
    }
}
