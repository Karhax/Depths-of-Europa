using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Statics;

public class DragonCrashBaseSound : MonoBehaviour
{
    [SerializeField, Range(0, 10)] float _timeFromSceneLoadToScream;
    [SerializeField] AudioSource _monsterHitBaseAudio;
    [SerializeField] AudioSource _screamAudio;

    private void Start()
    {
        StartCoroutine(Scream());
    }

    IEnumerator Scream()
    {
        yield return new WaitForSeconds(_timeFromSceneLoadToScream);
        _screamAudio.Play();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(Layers.FISH_UNDER) && !_monsterHitBaseAudio.isPlaying)
            _monsterHitBaseAudio.Play();
    }

}
