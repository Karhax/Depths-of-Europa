using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Statics;

public class SpawnFrozenFish : MonoBehaviour
{
    [SerializeField] GameObject _fish;
    [SerializeField, Range(0.1f, 10f)] float _timeToWaitAfterCollisionBeforeSpawn;
    [SerializeField, Range(0, 3)] float _minForceToBreak;

    bool _canSpawn = true;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag(Tags.PLAYER_OUTSIDE) && _canSpawn && other.relativeVelocity.magnitude > _minForceToBreak)
            StartCoroutine(SpawnFish());       
    }

    IEnumerator SpawnFish()
    {
        Timer waitTimer = new Timer(_timeToWaitAfterCollisionBeforeSpawn);
        _canSpawn = false;

        while (!waitTimer.Expired())
        {
            waitTimer.Time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        Instantiate(_fish, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
