using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Statics;

public class SpawnChaser : MonoBehaviour
{
    [Header("Settings")]

	[SerializeField, Range(0.1f, 10f)] float _minAttackTime = 3f;
    [SerializeField, Range(0.1f, 10f)] float _timeInLightBeforeAggro;
    [SerializeField, Range(0, 5)] float _startPositionBack;
    [SerializeField, Range(0.1f, 10)] float _timeToWaitAfterReturning;
    [SerializeField, Range(0, 5f)] float _enterSpawnSpeed = 0.25f;

    [Header("Drop")]

    [SerializeField] Transform _spawnLoc;
    [SerializeField] Transform _spawnGoBack;
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
	Timer _minAttackTimer;

    private void Awake()
    {
		_minAttackTimer = new Timer (_minAttackTime);
        _startPosition = _sprite.localPosition - new Vector3(_startPositionBack, 0, 0);
        _sprite.localPosition = _startPosition;
    }

    private void Start()
    {
        _player = GameManager.ShipObject.transform;
    }

    private void Update()
    {
        if (_canSpawn)
        {
            _sprite.localPosition = _startPosition + new Vector3(_currentAggroTime / _timeInLightBeforeAggro * _startPositionBack, 0, 0);

            if (_inLight && !_spawned)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, _player.position - transform.position, Vector2.Distance(transform.position, _player.position), LayerMask.GetMask(Layers.DEFAULT, Layers.BASE, Layers.FLOATING_OBJECT));

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

		if (_spawned)
			_minAttackTimer.Time += Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
		if (other.CompareTag (Tags.LIGHT) || other.CompareTag (Tags.FLARE_TRIGGER))
			_inLight = true;
		else if (other.transform == _fish && _fishHasLeft && _minAttackTimer.Expired())
			ResetSpawn();
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

        _fish = (Instantiate(_chaserFish, _spawnLoc.position, Quaternion.identity) as GameObject).transform;
        _fish.right = GameManager.ShipObject.transform.position - _fish.position;

        BasicChaserFish0 script = _fish.GetComponent<BasicChaserFish0>();
        script.SetSpawn(transform);

        _fish.gameObject.SetActive(true);
        _sprite.gameObject.SetActive(false);
    }

    private void ResetSpawn()
    {
        StartCoroutine(WaitForNextSpawn());
    }

    IEnumerator WaitForNextSpawn()
    {
        _fish.GetComponent<BasicChaserFish0>().ReachedSpawn();
        
        while (_fish.position != _spawnGoBack.position)
        {
            _fish.position = Vector3.MoveTowards(_fish.position, _spawnGoBack.position, Time.deltaTime * _enterSpawnSpeed);

            yield return new WaitForEndOfFrame();
        }

        _fishHasLeft = false;
        _sprite.gameObject.SetActive(true);
        Destroy(_fish.gameObject);
        _spawned = false;
        _currentAggroTime = 0;

        yield return new WaitForSeconds(_timeToWaitAfterReturning);

        _canSpawn = true;
    }
}
