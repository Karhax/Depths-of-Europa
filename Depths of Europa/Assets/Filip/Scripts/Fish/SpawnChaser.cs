using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Statics;

public class SpawnChaser : MonoBehaviour
{
    [Header("Settings")]

    [SerializeField, Range(0.1f, 10f)] float _timeInLightBeforeAggro;

    [Header("Drop")]

    [SerializeField] GameObject _chaserFish;

    Transform _fish;
    Transform _player;
    float _currentAggroTime = 0f;
    bool _inLight = false;

    bool _spawned = false;

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
        if (_inLight)
        {
            if (_currentAggroTime < _timeInLightBeforeAggro)
                _currentAggroTime += Time.deltaTime;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, _player.position - transform.position, Vector2.Distance(transform.position, _player.position), LayerMask.GetMask(Layers.DEFAULT));

            if (hit.collider == null && _currentAggroTime >= _timeInLightBeforeAggro && !_spawned)
                    SpawnFish();
        }
        else if (_currentAggroTime > 0)
            _currentAggroTime -= Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(Tags.LIGHT))
            _inLight = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(Tags.LIGHT))
            _inLight = false;
    }

    private void SpawnFish()
    {
        _spawned = true;

        _fish = (Instantiate(_chaserFish, transform.position, Quaternion.identity) as GameObject).transform;
        _fish.right = GameManager.ShipObject.transform.position - _fish.position;
        _fish.GetComponent<BasicChaserFish0>().FishOffScreenEvent += ResetSpawn;
    }

    private void ResetSpawn()
    {
        Destroy(_fish.gameObject);
        _spawned = false;
        _currentAggroTime = 0;
    }
}
