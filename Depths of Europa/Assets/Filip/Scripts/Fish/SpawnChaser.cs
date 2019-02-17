﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Statics;

public class SpawnChaser : MonoBehaviour
{
    [Header("Settings")]

    [SerializeField, Range(0.1f, 10f)] float _timeInLightBeforeAggro;
    [SerializeField, Range(0, 5)] float _startPositionBack;
    [SerializeField, Range(0.1f, 10)] float _timeToWaitAfterReturning;

    [Header("Drop")]

    [SerializeField] GameObject _chaserFish;
    [SerializeField] Transform _sprite;

    Transform _fish;
    Transform _player;
    float _currentAggroTime = 0f;
    bool _inLight = false;
    Vector3 _startPosition;

    bool _spawned = false;
    bool _fishHasLeft = false;

    bool _canSpawn = true;


    private void Awake()
    {
        _startPosition = _sprite.localPosition - new Vector3(_startPositionBack, 0, 0);
        _sprite.localPosition = _startPosition;
    }

    private void Start()
    {
        _player = GameManager.ShipObject.transform;
    }

    private void OnEnable()
    {
        if (_fish != null)
            _fish.GetComponent<BasicChaserFish0>().FishOffScreenEvent += ResetSpawn;
    }

    private void OnDisable()
    {
        if (_fish != null)
            _fish.GetComponent<BasicChaserFish0>().FishOffScreenEvent -= ResetSpawn;
    }

    private void Update()
    {
        if (_canSpawn)
        {
            _sprite.localPosition = _startPosition + new Vector3(_currentAggroTime / _timeInLightBeforeAggro * _startPositionBack, 0, 0);

            if (_inLight && !_spawned)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, _player.position - transform.position, Vector2.Distance(transform.position, _player.position), LayerMask.GetMask(Layers.DEFAULT, Layers.CHASER_SPAWN));
                
                Debug.Log(hit.collider);


                if (hit.collider == null)
                {
                    _currentAggroTime += Time.deltaTime;

                    if (_currentAggroTime >= _timeInLightBeforeAggro)
                        SpawnFish();
                }
            }
            else if (_currentAggroTime > 0)
                _currentAggroTime -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(Tags.LIGHT) || other.CompareTag(Tags.FLARE_TRIGGER))
            _inLight = true;
        else
        {
            BasicChaserFish0 fish = other.GetComponent<BasicChaserFish0>();

            if (fish != null && _fishHasLeft)
                ResetSpawn();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(Tags.LIGHT) || other.CompareTag(Tags.FLARE_TRIGGER))
            _inLight = false;
        else if (!_fishHasLeft && other.GetComponent<BasicChaserFish0>() != null)
            _fishHasLeft = true;
    }

    private void SpawnFish()
    {
        _sprite.localPosition = _startPosition;

        _canSpawn = false;
        _spawned = true;

        _fish = (Instantiate(_chaserFish, transform.position, Quaternion.identity) as GameObject).transform;
        _fish.right = GameManager.ShipObject.transform.position - _fish.position;

        BasicChaserFish0 script = _fish.GetComponent<BasicChaserFish0>();
        script.FishOffScreenEvent += ResetSpawn;
        script.SetSpawn(transform);

        _fish.gameObject.SetActive(true);
        _sprite.gameObject.SetActive(false);
    }

    private void ResetSpawn()
    {
        _fishHasLeft = false;
        _sprite.gameObject.SetActive(true);
        Destroy(_fish.gameObject);
        _spawned = false;
        _currentAggroTime = 0;

        StartCoroutine(WaitForNextSpawn());
    }

    IEnumerator WaitForNextSpawn()
    {
        yield return new WaitForSeconds(_timeToWaitAfterReturning);
        _canSpawn = true;
    }
}
