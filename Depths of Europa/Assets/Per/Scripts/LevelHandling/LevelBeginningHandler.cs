using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBeginningHandler : MonoBehaviour {

    // Maybe a bit overkill to have this be a singleton
    private static LevelBeginningHandler singletonLevelBeginner = null;

    // [SerializeField] private GameObject _shipPrefab = null;

    [SerializeField] private GameObject _playerShip = null;
    [SerializeField] private GameObject _mainCamera = null;

    [SerializeField] private Transform _spawnPoint = null;

    private void Awake()
    {
        if (singletonLevelBeginner == null)
        {
            singletonLevelBeginner = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        if(_spawnPoint == null)
        {
            throw new System.Exception("The LevelBeginningHandler could not find any spawn point");
        }

        /*if(_shipPrefab == null)
        {
            throw new System.Exception("The LevelBeginningHandler did not have any prefab for the ship");
        }*/
        else if(_playerShip == null)
        {
            throw new System.Exception("The LevelBeginningHandler could not find the Player Ship Object");
        }
        else if(_mainCamera == null)
        {
            throw new System.Exception("The LevelBeginningHandler could not find the Main Camera Object");
        }
        else
        {
            // Spawn the submarine and camera at the location of the spawn point
            // Instantiate(_shipPrefab, _spawnPoint.position, _spawnPoint.rotation);

            _playerShip.transform.position = _spawnPoint.transform.position;
            _playerShip.transform.rotation = _spawnPoint.transform.rotation;
            _mainCamera.transform.position = _spawnPoint.transform.position;
        }
    }

	private void Start ()
    {
		
	}
	
	private void Update ()
    {
		
	}
}
