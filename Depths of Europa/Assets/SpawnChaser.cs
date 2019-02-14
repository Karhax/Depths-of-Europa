using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Statics;

public class SpawnChaser : MonoBehaviour
{
    [Header("Settings")]

    [SerializeField] float _timeInLightBeforeAggro;

    [Header("Drop")]

    [SerializeField] GameObject _chaserFish;

    Timer _timer;
    bool _inLight = false;

    bool _spawned = false;

    private void Awake()
    {
        _timer = new Timer(_timeInLightBeforeAggro);
    }

    private void Update()
    {
        if (_inLight)
        {
            _timer.Time += Time.deltaTime;

            if (_timer.Expired() && !_spawned)
                SpawnFish();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(Tags.LIGHT))
            _inLight = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(Tags.LIGHT))
        {
            _inLight = false;
            _timer.Reset();
        }
    }

    private void SpawnFish()
    {
        _spawned = true;

        Transform fish = (Instantiate(_chaserFish, transform.position, Quaternion.identity) as GameObject).transform;

        fish.right = GameManager.ShipObject.transform.position - fish.position;
    }
}
